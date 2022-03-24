import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
//import { SesionComiteTema, SesionParticipante, TemaCompromiso } from 'src/app/_interfaces/technicalCommitteSession';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { forkJoin } from 'rxjs';
import { SesionComiteTema, SesionParticipante, TemaCompromiso } from 'src/app/_interfaces/technicalCommitteSession';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { promise } from 'protractor';

@Component({
  selector: 'app-form-otros-temas',
  templateUrl: './form-otros-temas.component.html',
  styleUrls: ['./form-otros-temas.component.scss']
})
export class FormOtrosTemasComponent implements OnInit {

  @Input() sesionComiteTema: SesionComiteTema;
  @Input() listaMiembros: SesionParticipante[];
  @Input() fechaComite: Date;
  @Input() esRegistroNuevo: boolean;
  @Output() validar: EventEmitter<boolean> = new EventEmitter();

  listaResponsables: Dominio[] = [];
  responsable: Dominio = {}

  addressForm = this.fb.group({
    estadoSolicitud: [null, Validators.required],
    observaciones: [null, Validators.required],
    observacionesDecision: [null, Validators.required],
    url: null,
    tieneCompromisos: [null, Validators.required],
    cuantosCompromisos: [null, [Validators.required, Validators.max(10)]],
    compromisos: this.fb.array([])
  });

  tieneVotacion: boolean = true;
  cantidadAprobado: number = 0;
  cantidadNoAprobado: number = 0;
  resultadoVotacion: string = '';

  estadosArray = [];

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  estaEditando = false;

  get compromisos() {
    return this.addressForm.get('compromisos') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private technicalCommitteSessionService: TechnicalCommitteSessionService,
    public dialog: MatDialog,
    private router: Router,

  ) {

  }

  async ngOnInit() {

    let estados: string[] = ['2', '4', '6', '8']

    return new Promise<void>(resolve => {

      forkJoin([
        this.commonService.listaEstadoSolicitud(),
        this.commonService.listaMiembrosComiteTecnico()
      ])
        .subscribe(response => {

          this.estadosArray = response[0].filter(s => estados.includes(s.codigo));
          this.listaResponsables = response[1];

          this.responsable = this.listaResponsables.find(r => r.codigo == this.sesionComiteTema.responsableCodigo)

          // this.estaEditando = true;
          // this.addressForm.markAllAsTouched();
          resolve();

        })

    });

  }

  ngDoCheck(): void {
    this.addressForm.valueChanges
      .subscribe(value => {
        if (value.cuantosCompromisos > 10) { value.cuantosCompromisos = 10; }
      });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaCompromiso() {
    this.compromisos.push(this.crearCompromiso());
    if ( this.estaEditando) this.addressForm.markAllAsTouched();
  }

  crearCompromiso() {
    return this.fb.group({
      temaCompromisoId: [],
      sesionTemaId: [],
      tarea: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(500)])
      ],
      responsable: [null, Validators.required],
      fecha: [null, Validators.required]
    });
  }

  changeCompromisos( requiereCompromisos ){

    if ( requiereCompromisos.value === false )
    {
      console.log( requiereCompromisos.value );
      this.technicalCommitteSessionService.eliminarCompromisosTema( this.sesionComiteTema.sesionTemaId )
        .subscribe( respuesta => {
          if (respuesta.code == "200"){
            this.compromisos.clear();
            this.addressForm.get("cuantosCompromisos").setValue(null);
          }
        })
    }
  }

  eliminarCompromisos(i) {

    let compromiso = this.compromisos.controls[i];

    this.technicalCommitteSessionService.deleteTemaCompromiso(compromiso.get('temaCompromisoId').value)
      .subscribe(respuesta => {
        if (respuesta.code == "200") {
          this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>');
          this.compromisos.removeAt(i)
          this.addressForm.get("cuantosCompromisos").setValue(this.compromisos.length);
        }
      })
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.eliminarCompromisos(e);
      }
    });
  }

  EliminarCompromiso(i: number) {
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este compromiso?</b>', i);
  }

  validarCompromisosDiligenciados(): boolean {
    let vacio = true;
    this.compromisos.controls.forEach(control => {
      if (  control.value.tarea ||
            control.value.responsable ||
            control.value.fecha
      )
        vacio = false;
    })

    return vacio;
  }

  CambioCantidadCompromisos() {
    if ( this.estaEditando) this.compromisos.markAllAsTouched();
    const FormGrupos = this.addressForm.value;
    if (FormGrupos.cuantosCompromisos > this.compromisos.length && FormGrupos.cuantosCompromisos < 100) {
      while (this.compromisos.length < FormGrupos.cuantosCompromisos) {
        this.compromisos.push(this.crearCompromiso());
        if ( this.estaEditando) this.addressForm.markAllAsTouched();
      }
    } else if (FormGrupos.cuantosCompromisos <= this.compromisos.length && FormGrupos.cuantosCompromisos >= 0) {
      if (this.validarCompromisosDiligenciados()) {

        while (this.compromisos.length > FormGrupos.cuantosCompromisos) {
          this.borrarArray(this.compromisos, this.compromisos.length - 1);
        }

      }
      else {

        this.openDialog('', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos</b>');
        this.addressForm.get('cuantosCompromisos').setValue( this.compromisos.length );

      }

    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let tema: SesionComiteTema = {

      sesionTemaId: this.sesionComiteTema.sesionTemaId,
      comiteTecnicoId: this.sesionComiteTema.comiteTecnicoId,
      observaciones: this.addressForm.get('observaciones').value,
      estadoTemaCodigo: this.addressForm.get('estadoSolicitud').value ? this.addressForm.get('estadoSolicitud').value.codigo : null,
      observacionesDecision: this.addressForm.get('observacionesDecision').value,
      generaCompromiso: this.addressForm.get('tieneCompromisos').value,
      cantCompromisos: this.addressForm.get('cuantosCompromisos').value,

      temaCompromiso: [],

    }

    this.compromisos.controls.forEach(control => {
      let temacompromiso: TemaCompromiso = {

        temaCompromisoId: control.get('temaCompromisoId').value,
        sesionTemaId: this.sesionComiteTema.sesionTemaId,
        responsable: control.get('responsable').value ? control.get('responsable').value.sesionParticipanteId : null,
        fechaCumplimiento: control.get('fecha').value,
        tarea: control.get('tarea').value,

      }

      tema.temaCompromiso.push(temacompromiso);
    })

    console.log(tema)
    this.fiduciaryCommitteeSessionService.createEditTemasCompromiso(tema)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`)
        this.validar.emit(respuesta.data);

        if (respuesta.code == "200" && !respuesta.data)
          this.router.navigate(['/comiteFiduciario/crearActa', this.sesionComiteTema.comiteTecnicoId])
      })
  }

  cargarRegistro() {

    let estados: string[] = ['2', '4', '6', '8']

    this.commonService.listaEstadoSolicitud()
      .subscribe(response => {

        this.estadosArray = response.filter(s => estados.includes(s.codigo));
        if ( this.sesionComiteTema.requiereVotacion ){
          this.sesionComiteTema.sesionTemaVoto.filter(R=> R.noAplica != true).forEach(sv => {
            if (sv.esAprobado)
              this.cantidadAprobado++;
            else
              this.cantidadNoAprobado++;
          })

          if(this.cantidadNoAprobado == 0 && this.cantidadAprobado == 0)
          {
            this.resultadoVotacion = 'No Aplica'
          }  else  if (this.cantidadNoAprobado == 0 && this.cantidadAprobado > 0){
            this.resultadoVotacion = 'Aprobó'
            this.estadosArray = this.estadosArray.filter(e => e.codigo == EstadosSolicitud.AprobadaPorComiteFiduciario)
          }else if ( this.cantidadAprobado == 0 && this.cantidadNoAprobado > 0){
            this.resultadoVotacion = 'No Aprobó'
            this.estadosArray = this.estadosArray.filter(e => [EstadosSolicitud.RechazadaPorComiteFiduciario, EstadosSolicitud.DevueltaPorComiteFiduciario].includes(e.codigo))
          }else if ( this.cantidadAprobado > this.cantidadNoAprobado ){
            this.resultadoVotacion = 'Aprobó'
          }else if ( this.cantidadAprobado <= this.cantidadNoAprobado ){
            this.resultadoVotacion = 'No Aprobó'
          }
        }

        this.responsable = this.listaResponsables.find(r => r.codigo == this.sesionComiteTema.responsableCodigo)

        let estadoSeleccionado = this.estadosArray.find(e => e.codigo == this.sesionComiteTema.estadoTemaCodigo)

          this.addressForm.get('observaciones').setValue(this.sesionComiteTema.observaciones);
          this.addressForm.get('estadoSolicitud').setValue(estadoSeleccionado);
          this.addressForm.get('observacionesDecision').setValue(this.sesionComiteTema.observacionesDecision);
          this.addressForm.get('tieneCompromisos').setValue(this.sesionComiteTema.generaCompromiso);
          this.addressForm.get('cuantosCompromisos').setValue(this.sesionComiteTema.cantCompromisos);

          if(!this.esRegistroNuevo){
            this.estaEditando = true;
            this.addressForm.markAllAsTouched();
          }

          //this.estaEditando = true;
          //this.addressForm.markAllAsTouched();

          this.commonService.listaUsuarios().then((respuesta) => {

            this.listaMiembros.forEach(m => {
              let usuario: Usuario = respuesta.find(u => u.usuarioId == m.usuarioId);
              m.nombre = `${usuario.primerNombre} ${usuario.primerApellido}`

            })

            this.sesionComiteTema.temaCompromiso.forEach(c => {
              let grupoCompromiso = this.crearCompromiso();
              let responsableSeleccionado = this.listaMiembros.find(m => m.sesionParticipanteId.toString() == c.responsable)

              grupoCompromiso.get('tarea').setValue(c.tarea);
              grupoCompromiso.get('responsable').setValue(responsableSeleccionado);
              grupoCompromiso.get('fecha').setValue(c.fechaCumplimiento);
              grupoCompromiso.get('temaCompromisoId').setValue(c.temaCompromisoId);
              grupoCompromiso.get('sesionTemaId').setValue(this.sesionComiteTema.sesionTemaId);

              this.compromisos.push(grupoCompromiso)
              if ( this.estaEditando) this.addressForm.markAllAsTouched();
            })

          });

          this.tieneVotacion = this.sesionComiteTema.requiereVotacion;

      })

  }

}
