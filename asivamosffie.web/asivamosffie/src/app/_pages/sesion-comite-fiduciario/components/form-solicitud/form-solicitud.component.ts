import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { SesionComiteSolicitud, SesionParticipante, SesionSolicitudCompromiso, TiposSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-form-solicitud',
  templateUrl: './form-solicitud.component.html',
  styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent implements OnInit {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() listaMiembros: SesionParticipante[];
  @Output() validar: EventEmitter<boolean> = new EventEmitter();

  tiposSolicitud = TiposSolicitud;

  fechaSolicitud: Date;
  numeroSolicitud: string;
  justificacion: string;

  tieneVotacion: boolean = true;
  cantidadAprobado: number = 0;
  cantidadNoAprobado: number = 0;
  resultadoVotacion: string = '';



  addressForm = this.fb.group({
    desarrolloSolicitud: [],
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
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
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

    let estados: string[] = ['2', '4', '6']

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
      comiteTecnicoId: this.sesionComiteSolicitud.comiteTecnicoId,
      comiteTecnicoFiduciarioId: this.sesionComiteSolicitud.comiteTecnicoFiduciarioId,
      estadoCodigo: this.addressForm.get('estadoSolicitud').value ? this.addressForm.get('estadoSolicitud').value.codigo : null,
      
      observacionesFiduciario: this.addressForm.get('observaciones').value,
      rutaSoporteVotacionFiduciario: this.addressForm.get('url').value,
      generaCompromisoFiduciario: this.addressForm.get('tieneCompromisos').value,
      cantCompromisosFiduciario: this.addressForm.get('cuantosCompromisos').value,
      desarrolloSolicitudFiduciario: this.addressForm.get('desarrolloSolicitud').value,
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

    this.fiduciaryCommitteeSessionService.createEditActasSesionSolicitudCompromiso(Solicitud)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message)
        console.log( respuesta.data ) 
        this.validar.emit( respuesta.data );
        if (respuesta.code == "200" && !respuesta.data)
          this.router.navigate(['/comiteFiduciario/crearActa', this.sesionComiteSolicitud.comiteTecnicoFiduciarioId])
      })

  }

  cargarRegistro() {

    console.log( this.sesionComiteSolicitud )

    if ( this.sesionComiteSolicitud.estadoCodigo == EstadosSolicitud.AprobadaPorComiteFiduciario ){
      this.estadosArray = this.estadosArray.filter( e => e.codigo == EstadosSolicitud.AprobadaPorComiteFiduciario)
    }else if ( this.sesionComiteSolicitud.estadoCodigo == EstadosSolicitud.RechazadaPorComiteFiduciario ){
      this.estadosArray = this.estadosArray.filter( e => [EstadosSolicitud.RechazadaPorComiteFiduciario, EstadosSolicitud.DevueltaPorComiteFiduciario].includes( e.codigo ))
    }

    let estadoSeleccionado = this.estadosArray.find(e => e.codigo == this.sesionComiteSolicitud.estadoCodigo)

    this.addressForm.get('estadoSolicitud').setValue(estadoSeleccionado)
    this.addressForm.get('observaciones').setValue(this.sesionComiteSolicitud.observacionesFiduciario)
    this.addressForm.get('url').setValue(this.sesionComiteSolicitud.rutaSoporteVotacionFiduciario)
    this.addressForm.get('tieneCompromisos').setValue(this.sesionComiteSolicitud.generaCompromisoFiduciario)
    this.addressForm.get('cuantosCompromisos').setValue(this.sesionComiteSolicitud.cantCompromisosFiduciario)
    this.addressForm.get('desarrolloSolicitud').setValue(this.sesionComiteSolicitud.desarrolloSolicitudFiduciario)
    

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

    this.sesionComiteSolicitud.sesionSolicitudVoto.filter( sv => sv.comiteTecnicoFiduciarioId == this.sesionComiteSolicitud.comiteTecnicoFiduciarioId ).forEach( sv => {
      if (sv.esAprobado)
        this.cantidadAprobado++;
      else
        this.cantidadNoAprobado++;
    })

    if ( this.cantidadNoAprobado > 0 )
      this.resultadoVotacion = 'No Aprobó'
    else
      this.resultadoVotacion = 'Aprobó'

    let btnSolicitudMultiple = document.getElementsByName( 'btnSolicitudMultiple' );
    
    btnSolicitudMultiple.forEach( element =>{
      element.click();
    })
    

    if (this.sesionComiteSolicitud.tipoSolicitudCodigo == TiposSolicitud.AperturaDeProcesoDeSeleccion){
      this.justificacion = this.sesionComiteSolicitud.procesoSeleccion.justificacion
    }
    

  }


}
