import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ActivatedRoute } from '@angular/router';
import { Contrato, ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-requisitos-tecnicos-construccion',
  templateUrl: './form-requisitos-tecnicos-construccion.component.html',
  styleUrls: ['./form-requisitos-tecnicos-construccion.component.scss']
})
export class FormRequisitosTecnicosConstruccionComponent implements OnInit {

  probBoolean: boolean = false;
  contrato: Contrato;

  constructor ( private dialog: MatDialog,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService,
                private activatedRoute: ActivatedRoute )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  };

  getContrato () {
    this.faseUnoConstruccionSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
    .subscribe( response => {
      this.contrato = response;
      console.log( this.contrato );
    } );
  };

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  getDiagnostico ( index: number, diagnostico: any ) {
    const diagnosticoForm: any = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      esInformeDiagnostico: diagnostico.esInformeDiagnostico,
      rutaInforme: diagnostico.rutaInforme,
      costoDirecto: diagnostico.costoDirecto,
      administracion: diagnostico.administracion,
      imprevistos: diagnostico.imprevistos,
      utilidad: diagnostico.utilidad,
      valorTotalFaseConstruccion: diagnostico.valorTotalFaseConstruccion,
      requiereModificacionContractual: diagnostico.requiereModificacionContractual,
      numeroSolicitudModificacion: diagnostico.numeroSolicitudModificacion
    };
    if ( this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion.length > 0 ) {
      diagnosticoForm.contratoConstruccionId = this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion[0].contratoConstruccionId;
    }
    this.faseUnoConstruccionSvc.createEditDiagnostico( diagnosticoForm )
      .subscribe( 
        response => {
          this.openDialog( '', response.message );
          this.getContrato();
        },
        err => this.openDialog( '', err.message )
      );

  };

  getPlanesProgramas ( index: number, planesProgramas ) {
    //console.log( index, planesProgramas );
    const planesProgramasForm: any = {};
    planesProgramasForm.planRutaSoporte = planesProgramas.urlSoporte;
    planesProgramasForm.contratoId = this.contrato.contratoId;
    planesProgramasForm.proyectoId = this.contrato.contratacion.contratacionProyecto[index].proyectoId;
    if ( this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion.length > 0 ) {
      planesProgramasForm.contratoConstruccionId = this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion[0].contratoConstruccionId;
    }
    planesProgramas.dataPlanesProgramas.forEach( plan => {
      if ( plan.nombrePlanesProgramas === 'Licencia vigente' ) {
        planesProgramasForm.planLicenciaVigente = plan.recibioRequisito;
        planesProgramasForm.licenciaFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.licenciaFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.licenciaConObservaciones = plan.requiereObservacion;
        planesProgramasForm.licenciaObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === 'Cambio constructor responsable de la licencia' ) {
        planesProgramasForm.planCambioConstructorLicencia = plan.recibioRequisito;
        planesProgramasForm.cambioFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.cambioFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.cambioConObservaciones = plan.requiereObservacion;
        planesProgramasForm.cambioObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === 'Acta aceptación y apropiación diseños' ) {
        planesProgramasForm.planActaApropiacion = plan.recibioRequisito;
        planesProgramasForm.actaApropiacionFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.actaApropiacionFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.actaApropiacionConObservaciones = plan.requiereObservacion;
        planesProgramasForm.actaApropiacionObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con plan de residuos de construcción y demolición (RCD) aprobado?' ) {
        planesProgramasForm.planResiduosDemolicion = plan.recibioRequisito;
        planesProgramasForm.residuosDemolicionFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.residuosDemolicionFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.residuosDemolicionConObservaciones = plan.requiereObservacion;
        planesProgramasForm.residuosDemolicionObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con plan de manejo de tránsito (PMT) aprobado?' ) {
        planesProgramasForm.planManejoTransito = plan.recibioRequisito;
        planesProgramasForm.manejoTransitoFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.manejoTransitoFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.manejoTransitoConObservaciones1 = plan.requiereObservacion;
        planesProgramasForm.manejoTransitoObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con plan de manejo ambiental aprobado?' ) {
        planesProgramasForm.planManejoAmbiental = plan.recibioRequisito;
        planesProgramasForm.manejoAmbientalFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.manejoAmbientalFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.manejoAmbientalConObservaciones = plan.requiereObservacion;
        planesProgramasForm.manejoAmbientalObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con plan de aseguramiento de la calidad de obra aprobado?' ) {
        planesProgramasForm.planAseguramientoCalidad = plan.recibioRequisito;
        planesProgramasForm.aseguramientoCalidadFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.aseguramientoCalidadFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.aseguramientoCalidadConObservaciones = plan.requiereObservacion;
        planesProgramasForm.aseguramientoCalidadObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con programa de Seguridad industrial aprobado?' ) {
        planesProgramasForm.planProgramaSeguridad = plan.recibioRequisito;
        planesProgramasForm.programaSeguridadFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.programaSeguridadFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.programaSeguridadConObservaciones = plan.requiereObservacion;
        planesProgramasForm.programaSeguridadObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con programa de salud ocupacional aprobado?' ) {
        planesProgramasForm.planProgramaSalud = plan.recibioRequisito;
        planesProgramasForm.programaSaludFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.programaSaludFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.programaSaludConObservaciones = plan.requiereObservacion;
        planesProgramasForm.programaSaludObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con un plan inventario arbóreo (talas) aprobado?' ) {
        planesProgramasForm.planInventarioArboreo = plan.recibioRequisito;
        planesProgramasForm.inventarioArboreoFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.inventarioArboreoFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.inventarioArboreoConObservaciones = plan.requiereObservacion;
        planesProgramasForm.inventarioArboreoObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con plan de aprovechamiento forestal aprobado?' ) {
        planesProgramasForm.planAprovechamientoForestal = plan.recibioRequisito;
        planesProgramasForm.aprovechamientoForestalApropiacionFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.aprovechamientoForestalFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.aprovechamientoForestalConObservaciones = plan.requiereObservacion;
        planesProgramasForm.aprovechamientoForestalObservaciones = plan.observaciones;
      }
      if ( plan.nombrePlanesProgramas === '¿Cuenta con plan de manejo de aguas lluvias aprobado?' ) {
        planesProgramasForm.planManejoAguasLluvias = plan.recibioRequisito;
        planesProgramasForm.manejoAguasLluviasFechaRadicado = plan.fechaRadicado ? plan.fechaRadicado.toISOString() : plan.fechaRadicado;
        planesProgramasForm.manejoAguasLluviasFechaAprobacion = plan.fechaAprobacion ? plan.fechaAprobacion.toISOString() : plan.fechaAprobacion;
        planesProgramasForm.manejoAguasLluviasConObservaciones = plan.requiereObservacion;
        planesProgramasForm.manejoAguasLluviasObservaciones = plan.observaciones;
      }
    } )
    console.log( planesProgramasForm );
    this.faseUnoConstruccionSvc.createEditPlanesProgramas( planesProgramasForm )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.getContrato();
        },
        err => this.openDialog( '', err.message )
      )
  };

  getAnticipo ( index: number, anticipo: any ) {
    const anticipoForm: any = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      manejoAnticipoRequiere: anticipo.requiereAnticipo,
      manejoAnticipoPlanInversion: anticipo.planInversionAnticipo,
      manejoAnticipoCronogramaAmortizacion: anticipo.cronogramaAmortizacionAprobado,
      manejoAnticipoRutaSoporte: anticipo.urlSoporte
    };
    if ( this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion.length > 0 ) {
      anticipoForm.contratoConstruccionId = this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion[0].contratoConstruccionId;
    }
    console.log( anticipoForm );
    this.faseUnoConstruccionSvc.createEditManejoAnticipo( anticipoForm )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.getContrato();
        },
        err => this.openDialog( '', err.message )
      )
  };

  getPerfilesContrato ( index: number, perfilContrato: ContratoPerfil[] ) {

    const construccionPerfil: any = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      construccionPerfil: perfilContrato
    };
    if ( this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion.length > 0 ) {
      construccionPerfil.contratoConstruccionId = this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion[0].contratoConstruccionId;
    }
    this.faseUnoConstruccionSvc.createEditConstruccionPerfil( construccionPerfil )
      .subscribe( 
        response => {
          this.openDialog( '', response.message );
          this.getContrato();
        },
        err => this.openDialog( '', err.message )
      );

  };

};