import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { SesionComiteTema, SesionParticipante, TemaCompromiso } from 'src/app/_interfaces/technicalCommitteSession';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { forkJoin } from 'rxjs';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-form-otros-temas',
  templateUrl: './form-otros-temas.component.html',
  styleUrls: ['./form-otros-temas.component.scss']
})
export class FormOtrosTemasComponent implements OnInit {

  @Input() sesionComiteTema: SesionComiteTema;
  @Input() listaMiembros: SesionParticipante[];
  @Output() validar: EventEmitter<boolean> = new EventEmitter();

  listaResponsables: Dominio[] = [];
  responsable: Dominio = {}

  tieneVotacion: boolean = true;
  cantidadAprobado: number = 0;
  cantidadNoAprobado: number = 0;
  resultadoVotacion: string = '';

  addressForm = this.fb.group({
    estadoSolicitud: [null, Validators.required],
    observaciones: [null, Validators.required],
    observacionesDecision: [null, Validators.required],
    url: null,
    tieneCompromisos: [null, Validators.required],
    cuantosCompromisos: [null, Validators.required],
    compromisos: this.fb.array([])
  });

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

  get compromisos() {
    return this.addressForm.get('compromisos') as FormArray;
  }

  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,
              private technicalCommitteSessionService: TechnicalCommitteSessionService,
              public dialog: MatDialog,
              private router: Router,

             ) 
  {

  }

  ngOnInit(): void 
  {

    let estados: string[] = ['1', '3', '5', '8']

    forkJoin([
      this.commonService.listaEstadoSolicitud(),
      this.commonService.listaMiembrosComiteTecnico()
    ])   
      .subscribe(response => {

        this.estadosArray = response[0].filter(s => estados.includes(s.codigo));
        this.listaResponsables = response[1];
      })

    this.responsable = this.listaResponsables.find( r => r.codigo == this.sesionComiteTema.responsableCodigo )


  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
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

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaCompromiso() {
    this.compromisos.push(this.crearCompromiso());
  }

  crearCompromiso() {
    return this.fb.group({
      temaCompromisoId: [],
      sesionTemaId: [],
      tarea: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      fecha: [null, Validators.required]
    });
  }

  CambioCantidadCompromisos() {
    const FormGrupos = this.addressForm.value;
    if (FormGrupos.cuantosCompromisos > this.compromisos.length && FormGrupos.cuantosCompromisos < 100) {
      while (this.compromisos.length < FormGrupos.cuantosCompromisos) {
        this.compromisos.push(this.crearCompromiso());
      }
    } else if (FormGrupos.cuantosCompromisos <= this.compromisos.length && FormGrupos.cuantosCompromisos >= 0) {
      while (this.compromisos.length > FormGrupos.cuantosCompromisos) {
        this.borrarArray(this.compromisos, this.compromisos.length - 1);
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
    this.technicalCommitteSessionService.createEditTemasCompromiso(tema)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`)
        this.validar.emit( respuesta.data );
        
        if (respuesta.code == "200" && !respuesta.data)
          this.router.navigate(['/comiteTecnico/crearActa', this.sesionComiteTema.comiteTecnicoId])
      })
  }

  cargarRegistro() {

    if ( this.sesionComiteTema.estadoTemaCodigo == EstadosSolicitud.AprobadaPorComiteTecnico ){
      this.estadosArray = this.estadosArray.filter( e => e.codigo == EstadosSolicitud.AprobadaPorComiteTecnico)
    }else if ( this.sesionComiteTema.estadoTemaCodigo == EstadosSolicitud.RechazadaPorComiteTecnico ){
      this.estadosArray = this.estadosArray.filter( e => [EstadosSolicitud.RechazadaPorComiteTecnico, EstadosSolicitud.DevueltaPorComiteTecnico].includes( e.codigo ))
    }

    this.responsable = this.listaResponsables.find( r => r.codigo == this.sesionComiteTema.responsableCodigo )

    let estadoSeleccionado = this.estadosArray.find(e => e.codigo == this.sesionComiteTema.estadoTemaCodigo)

    this.addressForm.get('observaciones').setValue( this.sesionComiteTema.observaciones ),
    this.addressForm.get('estadoSolicitud').setValue( estadoSeleccionado ),
    this.addressForm.get('observacionesDecision').setValue( this.sesionComiteTema.observacionesDecision ),
    this.addressForm.get('tieneCompromisos').setValue( this.sesionComiteTema.generaCompromiso ),
    this.addressForm.get('cuantosCompromisos').setValue( this.sesionComiteTema.cantCompromisos ),
    
    this.commonService.listaUsuarios().then((respuesta) => {

      this.listaMiembros.forEach(m => {
        let usuario: Usuario = respuesta.find(u => u.usuarioId == m.usuarioId);
        m.nombre = `${usuario.nombres} ${usuario.apellidos}`

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
      })

      this.sesionComiteTema.sesionTemaVoto.forEach( sv => {
        if (sv.esAprobado)
          this.cantidadAprobado++;
        else
          this.cantidadNoAprobado++;
      })
  
      if ( this.cantidadNoAprobado > 0 )
        this.resultadoVotacion = 'No Aprobó'
      else
        this.resultadoVotacion = 'Aprobó'
  
        this.tieneVotacion = this.sesionComiteTema.requiereVotacion;

    });
  }

}
