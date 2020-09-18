import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ComiteTecnico, SesionComiteSolicitud, SesionSolicitudCompromiso, SesionParticipante, TiposSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { EstadosProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-solicitud',
  templateUrl: './form-solicitud.component.html',
  styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent implements OnInit {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() listaMiembros: SesionParticipante[];

  tiposSolicitud = TiposSolicitud;

  fechaSolicitud: Date;
  numeroSolicitud: string;
  justificacion: string;

  tieneVotacion: boolean = true;
  cantidadAprobado: number = 0;
  cantidadNoAprobado: number = 0;
  resultadoVotacion: string = '';



  addressForm = this.fb.group({
    estadoSolicitud: [null, Validators.required],
    observaciones: [null, Validators.required],
    url: null,
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

  ) 
  {

  }

  getMostrarProyectos(){
    if ( this.sesionComiteSolicitud.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion )
      return 'block';
    else
      return 'none';
  }
  ngOnInit(): void {

    let estados: string[] = ['1', '3', '5']

    this.commonService.listaEstadoSolicitud()
      .subscribe(response => {

        this.estadosArray = response.filter(s => estados.includes(s.codigo));
      })

  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
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
      sesionSolicitudCompromisoId: [],
      sesionComiteSolicitudId: [],
      tarea: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
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

    let Solicitud: SesionComiteSolicitud = {
      sesionComiteSolicitudId: this.sesionComiteSolicitud.sesionComiteSolicitudId,
      estadoCodigo: this.addressForm.get('estadoSolicitud').value ? this.addressForm.get('estadoSolicitud').value.codigo : null,
      observaciones: this.addressForm.get('observaciones').value,
      rutaSoporteVotacion: this.addressForm.get('url').value,
      generaCompromiso: this.addressForm.get('tieneCompromisos').value,
      cantCompromisos: this.addressForm.get('cuantosCompromisos').value,
      sesionSolicitudCompromiso: []

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

    console.log(Solicitud)

    this.technicalCommitteSessionService.createEditActasSesionSolicitudCompromiso(Solicitud)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message)
        if (respuesta.code == "200")
          this.router.navigate(['/comiteTecnico/crearActa', this.sesionComiteSolicitud.comiteTecnicoId])
      })

  }

  cargarRegistro() {

    let estadoSeleccionado = this.estadosArray.find(e => e.codigo == this.sesionComiteSolicitud.estadoCodigo)

    this.addressForm.get('estadoSolicitud').setValue(estadoSeleccionado)
    this.addressForm.get('observaciones').setValue(this.sesionComiteSolicitud.observaciones)
    this.addressForm.get('url').setValue(this.sesionComiteSolicitud.rutaSoporteVotacion)
    this.addressForm.get('tieneCompromisos').setValue(this.sesionComiteSolicitud.generaCompromiso)
    this.addressForm.get('cuantosCompromisos').setValue(this.sesionComiteSolicitud.cantCompromisos)

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

    this.sesionComiteSolicitud.sesionSolicitudVoto.forEach( sv => {
      if (sv.esAprobado)
        this.cantidadAprobado++;
      else
        this.cantidadNoAprobado++;
    })

    if ( this.cantidadNoAprobado > 0 )
      this.resultadoVotacion = 'No Aprobó'
    else
      this.resultadoVotacion = 'Aprobó'

    let btnSolicitudMultiple = document.getElementById( 'btnSolicitudMultiple' );
    
    btnSolicitudMultiple.click();
    

    if (this.sesionComiteSolicitud.tipoSolicitudCodigo == TiposSolicitud.AperturaDeProcesoDeSeleccion){
      this.justificacion = this.sesionComiteSolicitud.procesoSeleccion.justificacion
    }
    

  }


}
