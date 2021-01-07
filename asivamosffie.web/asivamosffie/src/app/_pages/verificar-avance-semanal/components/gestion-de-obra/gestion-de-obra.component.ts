import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-gestion-de-obra',
  templateUrl: './gestion-de-obra.component.html',
  styleUrls: ['./gestion-de-obra.component.scss']
})
export class GestionDeObraComponent implements OnInit {

    @Input() seguimientoSemanal: any;
    @Input() esVerDetalle = false;
    @Input() tipoObservaciones: any;
    @Output() estadoSemaforo = new EventEmitter<number>();
    semaforoGestionAmbiental = 'sin-diligenciar';
    semaforoGestionCalidad = 'sin-diligenciar';
    semaforoGestionSst = 'sin-diligenciar';
    semaforoGestionSocial = 'sin-diligenciar';
    semaforoAlertasRelevantes = 'sin-diligenciar';

    constructor() { }

    ngOnInit(): void {
      if ( this.seguimientoSemanal !== undefined ) {
        //variable gestiones completadas
        let totalGestion = 0;
        // Semaforo gestion ambiental
        const gestionAmbiental = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental[0];
        let totalCompletados = 0;
        let totalActividades = 0;
        if ( gestionAmbiental.registroCompletoObservacionApoyo === false ) {
          this.semaforoGestionAmbiental = 'en-proceso';
        }
        if ( gestionAmbiental.registroCompletoObservacionApoyo === true ) {
          this.semaforoGestionAmbiental = 'completo';
          totalGestion++;

        }
        if ( gestionAmbiental.manejoMaterialesInsumo !== undefined ) {
          totalActividades++;
          const manejoMateriales = gestionAmbiental.manejoMaterialesInsumo;
          if ( manejoMateriales.registroCompletoObservacionApoyo === true ) {
            totalCompletados++;
          }
        }
        if ( gestionAmbiental.manejoResiduosConstruccionDemolicion !== undefined ) {
          totalActividades++;
          const residuosConstruccion = gestionAmbiental.manejoResiduosConstruccionDemolicion;
          if ( residuosConstruccion.registroCompletoObservacionApoyo === true ) {
            totalCompletados++;
          }
        }
        if ( gestionAmbiental.manejoResiduosPeligrososEspeciales !== undefined ) {
          totalActividades++;
          const residuosPeligrosos = gestionAmbiental.manejoResiduosPeligrososEspeciales;
          if ( residuosPeligrosos.registroCompletoObservacionApoyo === true ) {
            totalCompletados++;
          }
        }
        if ( gestionAmbiental.manejoOtro !== undefined ) {
          totalActividades++;
          const manejoOtros = gestionAmbiental.manejoOtro;
          if ( manejoOtros.registroCompletoObservacionApoyo === true ) {
            totalCompletados++;
          }
        }
        if ( totalActividades > 0 && ( totalCompletados < totalActividades ) ) {
          this.semaforoGestionAmbiental = 'en-proceso';
        }
        if ( totalActividades > 0 && ( totalCompletados === totalActividades ) ) {
          this.semaforoGestionAmbiental = 'completo';
          totalGestion++;
        }
        // Semaforo gestion de calidad
        const gestionCalidad = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
        let completadosCalidad = 0;
        if ( gestionCalidad.registroCompletoObservacionApoyo === false ) {
          this.semaforoGestionCalidad = 'en-proceso';
        }
        if ( gestionCalidad.registroCompletoObservacionApoyo === true ) {
          this.semaforoGestionCalidad = 'completo';
          totalGestion++;
        }
        for ( const ensayo of gestionCalidad.gestionObraCalidadEnsayoLaboratorio ) {
          if ( ensayo.registroCompletoObservacionApoyo === true ) {
            completadosCalidad++;
          }
        }
        if ( gestionCalidad.gestionObraCalidadEnsayoLaboratorio.length > 0 && ( completadosCalidad < gestionCalidad.gestionObraCalidadEnsayoLaboratorio.length ) ) {
          this.semaforoGestionCalidad = 'en-proceso';
        }
        if ( gestionCalidad.gestionObraCalidadEnsayoLaboratorio.length > 0 && ( completadosCalidad === gestionCalidad.gestionObraCalidadEnsayoLaboratorio.length ) ) {
          this.semaforoGestionCalidad = 'completo';
          totalGestion++;
        }
        // Semaforo gestion sst
        const gestionSst = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
        if ( gestionSst.registroCompletoObservacionApoyo === false ) {
          this.semaforoGestionSst = 'en-proceso';
        }
        if ( gestionSst.registroCompletoObservacionApoyo === true ) {
          this.semaforoGestionSst = 'completo';
          totalGestion++;
        }
        // Semaforo gestion social
        const gestionSocial = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial[0];
        if ( gestionSocial.registroCompletoObservacionApoyo === false ) {
          this.semaforoGestionSocial = 'en-proceso';
        }
        if ( gestionSocial.registroCompletoObservacionApoyo === true ) {
          this.semaforoGestionSocial = 'completo';
          totalGestion++;
        }
        // Semaforo alertas relevantes
        const alertasRelevantes = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAlerta[0];
        if ( alertasRelevantes.registroCompletoObservacionApoyo === false ) {
          this.semaforoAlertasRelevantes = 'en-proceso';
        }
        if ( alertasRelevantes.registroCompletoObservacionApoyo === true ) {
          this.semaforoAlertasRelevantes = 'completo';
          totalGestion++;
        }
        this.estadoSemaforo.emit( totalGestion );
      }
    }

}
