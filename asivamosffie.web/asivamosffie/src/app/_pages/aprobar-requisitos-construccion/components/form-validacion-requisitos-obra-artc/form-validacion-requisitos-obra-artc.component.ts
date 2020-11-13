import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-validacion-requisitos-obra-artc',
  templateUrl: './form-validacion-requisitos-obra-artc.component.html',
  styleUrls: ['./form-validacion-requisitos-obra-artc.component.scss']
})
export class FormValidacionRequisitosObraArtcComponent implements OnInit {

  contrato: Contrato;

  constructor ( private faseDosConstruccionSvc: FaseUnoConstruccionService,
                private activatedRoute: ActivatedRoute )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  };

  getContrato () {
    this.faseDosConstruccionSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
      .subscribe( response => {
        this.contrato = response;
        console.log( this.contrato );
        for ( let contratacion of this.contrato.contratacion.contratacionProyecto ) {

          //Semaforo Diagnostico
          contratacion.proyecto.contratoConstruccion[0].semaforoDiagnostico = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoSupervisor !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoSupervisor === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoSupervisor === false ) 
              ) 
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoDiagnostico = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionDiagnosticoSupervisor === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesDiagnosticoSupervisor === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoDiagnostico = 'en-proceso';
          };

          //Semaforo planes y programas
          contratacion.proyecto.contratoConstruccion[0].semaforoPlanes = "sin-diligenciar";

          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasSupervisor !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasSupervisor === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasSupervisor === false ) 
          ) 
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoPlanes = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionPlanesProgramasSupervisor === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesPlanesProgramasSupervisor === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoPlanes = 'en-proceso';
          };

          //Semaforo manejo de anticipo
          contratacion.proyecto.contratoConstruccion[0].semaforoManejo = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoSupervisor !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoSupervisor === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoSupervisor === false ) 
          ) 
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoManejo = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionManejoAnticipoSupervisor === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesManejoAnticipoSupervisor === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoManejo = 'en-proceso';
          };

          //Semaforo perfiles CV
          let perfilSinDiligenciar = 0;
          let perfilCompleto = 0;
          for ( let perfil of contratacion.proyecto.contratoConstruccion[0].construccionPerfil ) {
            perfil.semaforoPerfil = "sin-diligenciar";
            if  ( perfil.tieneObservacionesSupervisor !== undefined 
                  && (  perfil.tieneObservacionesSupervisor === true 
                        || perfil.tieneObservacionesSupervisor === false )
            ) 
            {
              perfil.semaforoPerfil = 'completo';
              if ( perfil.observacionSupervisor === undefined && perfil.tieneObservacionesSupervisor === true )
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
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraSupervisor !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraSupervisor === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraSupervisor === false ) 
          )
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoProgramacion = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionProgramacionObraSupervisor === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesProgramacionObraSupervisor === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoProgramacion = 'en-proceso';
          };

          //Semaforo flujo inversion de recursos
          contratacion.proyecto.contratoConstruccion[0].semaforoFlujo = "sin-diligenciar";
          if  ( contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionSupervisor !== undefined 
                && (  contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionSupervisor === true 
                      || contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionSupervisor === false ) 
          )
          {
            contratacion.proyecto.contratoConstruccion[0].semaforoFlujo = 'completo';
            if ( contratacion.proyecto.contratoConstruccion[0].observacionFlujoInversionSupervisor === undefined && contratacion.proyecto.contratoConstruccion[0].tieneObservacionesFlujoInversionSupervisor === true )
              contratacion.proyecto.contratoConstruccion[0].semaforoFlujo = 'en-proceso';
          };

        };
      } );
  };

  Cargar ( seGuardo: boolean ) {
    if ( seGuardo ) {
      this.contrato = null;
      this.getContrato();
    };
  };

};