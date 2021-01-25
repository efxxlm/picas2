import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ComiteTecnico, SesionComiteSolicitud, SesionSolicitudCompromiso, SesionParticipante, TiposSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { EstadosProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { ContratacionProyecto, EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { opendir } from 'fs';

@Component({
  selector: 'app-form-solicitud',
  templateUrl: './form-solicitud.component.html',
  styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent implements OnInit, OnChanges {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() listaMiembros: SesionParticipante[];
  @Input() fechaComite: Date;

  @Output() validar: EventEmitter<boolean> = new EventEmitter();

  minDate: Date;
  estaEditando = false;
  tiposSolicitud = TiposSolicitud;

  fechaSolicitud: Date;
  numeroSolicitud: string;
  justificacion: string;

  tieneVotacion: boolean = true;
  cantidadAprobado: number = 0;
  cantidadNoAprobado: number = 0;
  resultadoVotacion: string = '';

  proyectos: ContratacionProyecto[];

  addressForm = this.fb.group({
    desarrolloSolicitud: [],
    estadoSolicitud: [null, Validators.required],
    observaciones: [null, Validators.required],
    url: [null, Validators.required],
    tieneCompromisos: [null, Validators.required],
    cuantosCompromisos: [null, Validators.required],
    compromisos: this.fb.array([])
  });

  estadosArray: Dominio[] = [];

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

  get compromisos() {
    return this.addressForm.get('compromisos') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private technicalCommitteSessionService: TechnicalCommitteSessionService,
    public dialog: MatDialog,
    private router: Router,

  ) {

  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.sesionComiteSolicitud.currentValue)
      this.cargarRegistro();
  }

  ActualizarProyectos(lista) {
    this.proyectos = lista;
  }

  getMostrarProyectos() {
    if (this.sesionComiteSolicitud.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion)
      return 'block';
    else
      return 'none';
  }

  getMostrarActulizacionCronograma() {
    if (this.sesionComiteSolicitud.tipoSolicitudCodigo == this.tiposSolicitud.ActualizacionCronogramaProcesoseleccion)
      return 'block';
    else
      return 'none';
  }

  ngOnInit(): void {
    this.minDate = new Date();
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

  textoLimpioString(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  borrarArray(borrarForm: any, i: number) {
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este compromiso?</b>', i);
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      // console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.eliminarCompromisos(e);
      }
    });
  }

  agregaCompromiso() {
    this.compromisos.push(this.crearCompromiso());
  }

  crearCompromiso() {
    return this.fb.group({
      sesionSolicitudCompromisoId: [],
      sesionComiteSolicitudId: [],
      tarea: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(500)])
      ],
      responsable: [null, Validators.required],
      fecha: [null, Validators.required]
    });
  }

  validarCompromisosDiligenciados(): boolean {
    let vacio = true;
    this.compromisos.controls.forEach(control => {
      if (control.value.tarea ||
        control.value.responsable ||
        control.value.fecha
      )
        vacio = false;
    })

    return vacio;
  }

  borrarCompromiso(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  CambioCantidadCompromisos() {
    const FormGrupos = this.addressForm.value;
    if (FormGrupos.cuantosCompromisos > this.compromisos.length && FormGrupos.cuantosCompromisos < 100) {
      while (this.compromisos.length < FormGrupos.cuantosCompromisos) {
        this.compromisos.push(this.crearCompromiso());
      }
    } else if (FormGrupos.cuantosCompromisos <= this.compromisos.length && FormGrupos.cuantosCompromisos >= 0) {
      if (this.validarCompromisosDiligenciados()) {

        while (this.compromisos.length > FormGrupos.cuantosCompromisos) {
          this.borrarCompromiso(this.compromisos, this.compromisos.length - 1);
        }

      }
      else {

        this.openDialog('', 'Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos');
        this.addressForm.get('cuantosCompromisos').setValue(this.compromisos.length);

      }
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  getObservableEstadoSolicitud() {
    return this.addressForm.get('estadoSolicitud').valueChanges;
  }

  changeCompromisos(requiereCompromisos) {

    if (requiereCompromisos.value === false) {
      // console.log(requiereCompromisos.value);
      this.technicalCommitteSessionService.eliminarCompromisosSolicitud(this.sesionComiteSolicitud.sesionComiteSolicitudId)
        .subscribe(respuesta => {
          if (respuesta.code == "200") {
            this.compromisos.clear();
            this.addressForm.get("cuantosCompromisos").setValue(null);
          }
        })
    }
  }

  eliminarCompromisos(i) {

    let compromiso = this.compromisos.controls[i];

    this.technicalCommitteSessionService.deleteSesionComiteCompromiso(compromiso.get('sesionSolicitudCompromisoId').value)
      .subscribe(respuesta => {
        if (respuesta.code == "200") {
          this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>');
          this.compromisos.removeAt(i)
          this.addressForm.get("cuantosCompromisos").setValue(this.compromisos.length);
        }
      })
  }

  onSubmit() {
    this.estaEditando = true;
    if (this.proyectos)
      this.proyectos.forEach(p => {
        let proyecto = p.proyecto
        p.proyecto = {
          EstadoProyectoCodigo: proyecto.estadoProyectoCodigo,
          proyectoId: proyecto.proyectoId,


        }
        p.proyecto.estadoProyecto = p.proyecto.estadoProyecto ? p.proyecto.estadoProyecto.codigo : null
      })

    let Solicitud: SesionComiteSolicitud = {
      sesionComiteSolicitudId: this.sesionComiteSolicitud.sesionComiteSolicitudId,
      comiteTecnicoId: this.sesionComiteSolicitud.comiteTecnicoId,
      estadoCodigo: this.addressForm.get('estadoSolicitud').value,
      observaciones: this.addressForm.get('observaciones').value,
      rutaSoporteVotacion: this.addressForm.get('url').value,
      generaCompromiso: this.addressForm.get('tieneCompromisos').value,
      cantCompromisos: this.addressForm.get('cuantosCompromisos').value,
      desarrolloSolicitud: this.addressForm.get('desarrolloSolicitud').value,
      tipoSolicitud: this.sesionComiteSolicitud.tipoSolicitudCodigo,
      sesionSolicitudCompromiso: [],
      contratacion: {
        contratacionProyecto: this.proyectos ? this.proyectos : null
      }

    }

    this.compromisos.controls.forEach(control => {
      let sesionSolicitudCompromiso: SesionSolicitudCompromiso = {
        tarea: control.get('tarea').value,
        responsableSesionParticipanteId: control.get('responsable').value ? control.get('responsable').value.sesionParticipanteId : null,
        fechaCumplimiento: control.get('fecha').value,
        sesionSolicitudCompromisoId: control.get('sesionSolicitudCompromisoId').value,
        sesionComiteSolicitudId: this.sesionComiteSolicitud.sesionComiteSolicitudId,

      }

      Solicitud.sesionSolicitudCompromiso.push(sesionSolicitudCompromiso);
    })

    // console.log(Solicitud)

    this.technicalCommitteSessionService.createEditActasSesionSolicitudCompromiso(Solicitud)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`)
        // console.log(respuesta.data)
        this.validar.emit(respuesta.data);
        if (respuesta.code == "200" && !respuesta.data)
          this.router.navigate(['/comiteTecnico/crearActa', this.sesionComiteSolicitud.comiteTecnicoId])
      })

  }

  cargarRegistro() {

    // console.log(this.sesionComiteSolicitud)

    let estados: string[] = ['1', '3', '5']

    this.commonService.listaEstadoSolicitud()
      .subscribe(response => {

        this.estadosArray = response.filter(s => estados.includes(s.codigo));

        if ( this.sesionComiteSolicitud.requiereVotacion ){
          this.sesionComiteSolicitud.sesionSolicitudVoto.forEach(sv => {
            if (sv.esAprobado)
              this.cantidadAprobado++;
            else
              this.cantidadNoAprobado++;
          })
      
          if (this.cantidadNoAprobado == 0){
            this.resultadoVotacion = 'Aprobó'
            this.estadosArray = this.estadosArray.filter(e => e.codigo == EstadosSolicitud.AprobadaPorComiteTecnico)
          }else if ( this.cantidadAprobado == 0 ){
            this.resultadoVotacion = 'No Aprobó'
            this.estadosArray = this.estadosArray.filter(e => [EstadosSolicitud.RechazadaPorComiteTecnico, EstadosSolicitud.DevueltaPorComiteTecnico].includes(e.codigo))
          }else if ( this.cantidadAprobado > this.cantidadNoAprobado ){
            this.resultadoVotacion = 'Aprobó'
          }else if ( this.cantidadAprobado <= this.cantidadNoAprobado ){
            this.resultadoVotacion = 'No Aprobó'
          }
        }
        
      })



    //let estadoSeleccionado = this.estadosArray.find(e => e.codigo == this.sesionComiteSolicitud.estadoCodigo)

    this.addressForm.get('estadoSolicitud').setValue(this.sesionComiteSolicitud.estadoCodigo)
    this.addressForm.get('observaciones').setValue(this.sesionComiteSolicitud.observaciones)
    this.addressForm.get('url').setValue(this.sesionComiteSolicitud.rutaSoporteVotacion)
    this.addressForm.get('tieneCompromisos').setValue(this.sesionComiteSolicitud.generaCompromiso)
    this.addressForm.get('cuantosCompromisos').setValue(this.sesionComiteSolicitud.cantCompromisos)
    this.addressForm.get('desarrolloSolicitud').setValue(this.sesionComiteSolicitud.desarrolloSolicitud)


    this.commonService.listaUsuarios().then((respuesta) => {

      this.listaMiembros.forEach(m => {
        let usuario: Usuario = respuesta.find(u => u.usuarioId == m.usuarioId);
        m.nombre = `${usuario.nombres} ${usuario.apellidos}`

      })

      this.sesionComiteSolicitud.sesionSolicitudCompromiso.forEach(c => {
        let grupoCompromiso = this.crearCompromiso();
        let responsableSeleccionado = this.listaMiembros.find(m => m.sesionParticipanteId == c.responsableSesionParticipanteId)

        grupoCompromiso.get('tarea').setValue(c.tarea);
        grupoCompromiso.get('responsable').setValue(responsableSeleccionado);
        grupoCompromiso.get('fecha').setValue(c.fechaCumplimiento);
        grupoCompromiso.get('sesionSolicitudCompromisoId').setValue(c.sesionSolicitudCompromisoId);
        grupoCompromiso.get('sesionComiteSolicitudId').setValue(this.sesionComiteSolicitud.sesionComiteSolicitudId);

        this.compromisos.push(grupoCompromiso)
      })

    });

    
    this.tieneVotacion = this.sesionComiteSolicitud.requiereVotacion;

    // let btnSolicitudMultiple = document.getElementsByName( 'btnSolicitudMultiple' );

    // btnSolicitudMultiple.forEach( element =>{
    //   element.click();
    // })


    if (this.sesionComiteSolicitud.tipoSolicitudCodigo == TiposSolicitud.AperturaDeProcesoDeSeleccion) {
      this.justificacion = this.sesionComiteSolicitud.procesoSeleccion.justificacion
    }


  }


}
