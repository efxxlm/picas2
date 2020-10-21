import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SesionComiteSolicitud, ComiteTecnico, SesionSolicitudVoto, SesionSolicitudObservacionProyecto, SesionSolicitudObservacionActualizacionCronograma } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Router } from '@angular/router';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';

@Component({
  selector: 'app-votacion-solicitud-actualiza_cronograma',
  templateUrl: './votacion-solicitud-actualiza_cronograma.component.html',
  styleUrls: ['./votacion-solicitud-actualiza_cronograma.component.scss']
})
export class VotacionSolicitudActualizaCronogramaComponent implements OnInit {
  miembros: any[] = ['Juan Lizcano Garcia', 'Fernando José Aldemar Rojas', 'Gonzalo Díaz Mesa'];

  addressForm = this.fb.group({
    aprobaciones: this.fb.array([]),
    proyectos: this.fb.array([]),
  });

  get aprobaciones() {
    return this.addressForm.get('aprobaciones') as FormArray;
  }

  get proyectos() {
    return this.addressForm.get('proyectos') as FormArray;
  }

  observaciones(i: number) {
    return this.proyectos.controls[i].get('observaciones') as FormArray;
  }

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  crearAprobaciones() {
    return this.fb.group({
      nombreParticipante: [],
      sesionSolicitudVotoId: [],
      sesionParticipanteId: [],
      sesionComiteSolicitudId: [],
      aprobacion: [null, Validators.required]

    });
  }

  crearProyectos() {
    return this.fb.group({
      llaveMen: [],
      nombreInstitucion: [],
      nombreSede: [],
      observaciones: this.fb.array([]),

    });
  }

  crearObservaciones() {
    return this.fb.group({
      nombreParticipante: [],
      sesionSolicitudObservacionActualizacionCronogramaId: [],
      sesionComiteSolicitudId: [],
      sesionParticipanteId: [],
      procesoSeleccionCronogramaMonitoreoId: [],
      observacion: [null, Validators.required]

    });
  }

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<VotacionSolicitudActualizaCronogramaComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      sesionComiteSolicitud: SesionComiteSolicitud,
      objetoComiteTecnico: ComiteTecnico
    },
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private projectContractingService: ProjectContractingService,
    public dialog: MatDialog,
    private router: Router,
    private projectService: ProjectService,

  ) {

  }

  ngOnInit(): void {

    this.aprobaciones.clear();

    this.data.sesionComiteSolicitud.sesionSolicitudVoto.forEach(v => {
      let grupoVotacion = this.crearAprobaciones();

      grupoVotacion.get('nombreParticipante').setValue(v.nombreParticipante);
      grupoVotacion.get('aprobacion').setValue(v.esAprobado);

      grupoVotacion.get('sesionSolicitudVotoId').setValue(v.sesionSolicitudVotoId);
      grupoVotacion.get('sesionParticipanteId').setValue(v.sesionParticipanteId);
      grupoVotacion.get('sesionComiteSolicitudId').setValue(v.sesionComiteSolicitudId);

      this.aprobaciones.push(grupoVotacion)
    })

    this.proyectos.clear();

    //this.data.sesionComiteSolicitud.contratacion.contratacionProyecto.forEach( cp => {
    //this.projectContractingService.getContratacionByContratacionId(this.data.sesionComiteSolicitud.contratacion.contratacionId)
    this.fiduciaryCommitteeSessionService.getProcesoSeleccionMonitoreo( this.data.sesionComiteSolicitud.procesoSeleccionMonitoreo.procesoSeleccionMonitoreoId )
    //console.log( this.data.sesionComiteSolicitud, this.data.sesionComiteSolicitud.sesionSolicitudObservacionActualizacionCronograma )

    //this.data.sesionComiteSolicitud.sesionSolicitudObservacionActualizacionCronograma.forEach( cp => {

      .subscribe(respuesta => {

      this.data.sesionComiteSolicitud.procesoSeleccionMonitoreo = respuesta;
      this.data.sesionComiteSolicitud.procesoSeleccionMonitoreo.procesoSeleccionCronogramaMonitoreo.forEach(cp => {
      //  this.data.sesionComiteSolicitud.contratacion.contratacionProyecto.forEach(cp => {

          let grupoProyecto = this.crearProyectos();
          let listaObservaciones = grupoProyecto.get('observaciones') as FormArray;

          grupoProyecto.get('llaveMen').setValue(cp.numeroActividad);

          this.data.sesionComiteSolicitud.sesionSolicitudObservacionActualizacionCronograma
            .filter(o => o.procesoSeleccionCronogramaMonitoreoId == cp.procesoSeleccionCronogramaMonitoreoId)
            .forEach(op => {

              let grupoObservacion = this.crearObservaciones();

              grupoObservacion.get('nombreParticipante').setValue(op.nombreParticipante);
              grupoObservacion.get('sesionSolicitudObservacionActualizacionCronogramaId').setValue(op.sesionSolicitudObservacionActualizacionCronogramaId);
              grupoObservacion.get('sesionComiteSolicitudId').setValue(op.sesionComiteSolicitudId);
              grupoObservacion.get('sesionParticipanteId').setValue(op.sesionParticipanteId);
              grupoObservacion.get('procesoSeleccionCronogramaMonitoreoId').setValue(op.procesoSeleccionCronogramaMonitoreoId);
              grupoObservacion.get('observacion').setValue(op.observacion);

              listaObservaciones.push(grupoObservacion);
            })


          this.proyectos.push(grupoProyecto)
        })
      })
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    let sesionComiteSolicitud: SesionComiteSolicitud = {
      sesionComiteSolicitudId: this.data.sesionComiteSolicitud.sesionComiteSolicitudId,
      comiteTecnicoId: this.data.sesionComiteSolicitud.comiteTecnicoId,
      sesionSolicitudVoto: [],
      sesionSolicitudObservacionProyecto: [],
      sesionSolicitudObservacionActualizacionCronograma: [],
      
    }

    console.log( 'fidu', this.data.sesionComiteSolicitud.comiteTecnicoFiduciarioId)

    this.aprobaciones.controls.forEach(control => {
      let sesionSolicitudVoto: SesionSolicitudVoto = {
        sesionSolicitudVotoId: control.get('sesionSolicitudVotoId').value,
        sesionComiteSolicitudId: control.get('sesionComiteSolicitudId').value,
        sesionParticipanteId: control.get('sesionParticipanteId').value,
        esAprobado: control.get('aprobacion').value,
        comiteTecnicoFiduciarioId: this.data.sesionComiteSolicitud.comiteTecnicoFiduciarioId,
        //observacion: control.get('observaciones').value,

      }

      sesionComiteSolicitud.sesionSolicitudVoto.push(sesionSolicitudVoto); 
    })

    this.proyectos.controls.forEach(controlProyecto => {
      let listaObservaciones = controlProyecto.get('observaciones') as FormArray;

      listaObservaciones.controls.forEach(control => {
        let observacionActualizarCronograma: SesionSolicitudObservacionActualizacionCronograma = {
          sesionSolicitudObservacionActualizacionCronogramaId: control.get('sesionSolicitudObservacionActualizacionCronogramaId').value,
          sesionComiteSolicitudId: control.get('sesionComiteSolicitudId').value,
          sesionParticipanteId: control.get('sesionParticipanteId').value,
          procesoSeleccionCronogramaMonitoreoId: control.get('procesoSeleccionCronogramaMonitoreoId').value,
          observacion: control.get('observacion').value,

        }
        
        sesionComiteSolicitud.sesionSolicitudObservacionActualizacionCronograma.push( observacionActualizarCronograma );
      })

    })

    sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.AprobadaPorComiteFiduciario;
    sesionComiteSolicitud.sesionSolicitudVoto.forEach(sv => {
      if (sv.esAprobado != true)
        sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.RechazadaPorComiteFiduciario;
    })

    console.log(sesionComiteSolicitud);

    this.fiduciaryCommitteeSessionService.createEditSesionSolicitudVoto(sesionComiteSolicitud)
      .subscribe(respuesta => {
        this.openDialog('Comité técnico', respuesta.message)
        if (respuesta.code == "200") {
          this.dialogRef.close(this.data.objetoComiteTecnico);
          //this.router.navigate(['/comiteTecnico/registrarSesionDeComiteTecnico',this.data.objetoComiteTecnico.comiteTecnicoId,'registrarParticipantes'])


        }

      })

  }

}
