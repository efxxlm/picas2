import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-verificacion-requisitos',
  templateUrl: './form-verificacion-requisitos.component.html',
  styleUrls: ['./form-verificacion-requisitos.component.scss']
})
export class FormVerificacionRequisitosComponent implements OnInit {

  contrato: Contrato;
  fechaPoliza: string;

  completoDiagnostico: string;

  constructor(
    private faseUnoConstruccionService: FaseUnoConstruccionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,) {
    this.getContrato();

    if (this.router.getCurrentNavigation().extras.state)
      this.fechaPoliza = this.router.getCurrentNavigation().extras.state.fechaPoliza;
  }

  ngOnInit(): void {
  }

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  getContrato() {
    this.faseUnoConstruccionService.getContratoByContratoId(this.activatedRoute.snapshot.params.id)
      .subscribe(response => {
        this.contrato = response;
        console.log(this.contrato);
        for ( let contratacion of this.contrato.contratacion.contratacionProyecto ) {

          //Semaforo Diagnostico
          contratacion.proyecto.contratoConstruccion[0].semaforoDiagnostico = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoApoyo !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoApoyo === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoApoyo === false ) 
              ) 
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoDiagnostico = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionDiagnosticoApoyo === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoApoyo === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoDiagnostico = 'en-proceso';
          };

          //Semaforo planes y programas
          contratacion.proyecto.contratoConstruccion[0].semaforoPlanes = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasApoyo !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasApoyo === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasApoyo === false ) 
          ) 
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoPlanes = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionPlanesProgramasApoyo === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasApoyo === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoPlanes = 'en-proceso';
          };

          //Semaforo manejo de anticipo
          contratacion.proyecto.contratoConstruccion[0].semaforoManejo = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoApoyo !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoApoyo === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoApoyo === false ) 
          ) 
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoManejo = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionManejoAnticipo === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoApoyo === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoManejo = 'en-proceso';
          };
          //Semaforo perfiles CV
          let perfilSinDiligenciar = 0;
          let perfilCompleto = 0;
          for ( let perfil of contratacion.proyecto.contratoConstruccion[0].construccionPerfil ) {
            perfil.semaforoPerfil = "sin-diligenciar";
            if  ( perfil.tieneObservacionesApoyo !== undefined 
                  && (  perfil.tieneObservacionesApoyo === true 
                        || perfil.tieneObservacionesApoyo === false )
            ) 
            {
              perfil.semaforoPerfil = 'completo';
              if ( perfil.observacionApoyo === undefined && perfil.tieneObservacionesApoyo === true )
                perfil.semaforoPerfil = 'en-proceso';
            };
            if ( perfil.semaforoPerfil === 'sin-diligenciar' ) {
              perfilSinDiligenciar++;
            };
            if ( perfil.semaforoPerfil === 'completo' ) {
              perfilCompleto++;
            };
          };
          if ( perfilSinDiligenciar > 0 && ( perfilSinDiligenciar === contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length ) ) {
            contratacion.proyecto.contratoConstruccion[0].semaforoPerfiles = "sin-diligenciar";
          };
          if ( perfilCompleto > 0 && ( perfilCompleto === contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length ) ) {
            contratacion.proyecto.contratoConstruccion[0].semaforoPerfiles = "completo";
          };
          if (  perfilSinDiligenciar === perfilCompleto
                || ( perfilSinDiligenciar > 0 && perfilSinDiligenciar < contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length )
                || ( perfilCompleto > 0 && perfilCompleto < contratacion.proyecto.contratoConstruccion[0].construccionPerfil.length ) ) {
            contratacion.proyecto.contratoConstruccion[0].semaforoPerfiles = "en-proceso";
          };

          //Semaforo programacion de obra
          contratacion.proyecto.contratoConstruccion[0].semaforoProgramacion = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraApoyo !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraApoyo === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraApoyo === false ) 
          )
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoProgramacion = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionProgramacionObraApoyo === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraApoyo === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoProgramacion = 'en-proceso';
          };

          //Semaforo flujo inversion de recursos
          contratacion.proyecto.contratoConstruccion[0].semaforoFlujo = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionApoyo !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionApoyo === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionApoyo === false ) 
          )
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoFlujo = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionFlujoInversionApoyo === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionApoyo === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoFlujo = 'en-proceso';
          };

        };
      });
  };

  Cargar ( seGuardo: boolean ) {
    if ( seGuardo ) {
      this.contrato = null;
      this.getContrato();
    };
  };

};