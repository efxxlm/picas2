import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestion-de-obra',
  templateUrl: './gestion-de-obra.component.html',
  styleUrls: ['./gestion-de-obra.component.scss']
})
export class GestionDeObraComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    semaforoAmbiental = 'sin-diligenciar';
    semaforoCalidad = 'sin-diligenciar';
    semaforoSst = 'sin-diligenciar';
    semaforoSocial = 'sin-diligenciar';
    semaforoAlertas = 'sin-diligenciar';

    constructor() { }

    ngOnInit(): void {
      if ( this.seguimientoSemanal !== undefined && this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
        const seguimientoSemanalObra = this.seguimientoSemanal.seguimientoSemanalGestionObra[0];
        // Semaforo ambiental
        // Por integrar
        // Semaforo Calidad
        if ( seguimientoSemanalObra.seguimientoSemanalGestionObraCalidad.length > 0 ) {
          const gestionCalidad = seguimientoSemanalObra.seguimientoSemanalGestionObraCalidad[0];
          if ( gestionCalidad.seRealizaronEnsayosLaboratorio !== undefined && gestionCalidad.registroCompleto === false ) {
            this.semaforoCalidad = 'en-proceso';
          }
          if ( gestionCalidad.registroCompleto === true ) {
            this.semaforoCalidad = 'completo';
          }
        }
        // Semaforo SST
        if ( seguimientoSemanalObra.seguimientoSemanalGestionObraSeguridadSalud.length > 0 ) {
          const gestionSst = seguimientoSemanalObra.seguimientoSemanalGestionObraSeguridadSalud[0];
          if ( gestionSst.cantidadAccidentes !== undefined && gestionSst.registroCompleto === false ) {
            this.semaforoSst = 'en-proceso';
          }
          if ( gestionSst.registroCompleto === true ) {
            this.semaforoSst = 'completo';
          }
        }
        // Semaforo Social
        if ( seguimientoSemanalObra.seguimientoSemanalGestionObraSocial.length > 0 ) {
          const gestionSocial = seguimientoSemanalObra.seguimientoSemanalGestionObraSocial[0];
          if ( gestionSocial.cantidadEmpleosDirectos !== undefined && gestionSocial.registroCompleto === false ) {
            this.semaforoSocial = 'en-proceso';
          }
          if ( gestionSocial.registroCompleto === true ) {
            this.semaforoSocial = 'completo';
          }
        }
        // Semaforo alertas
        if ( seguimientoSemanalObra.seguimientoSemanalGestionObraAlerta.length > 0 ) {
          const gestionAlertas = seguimientoSemanalObra.seguimientoSemanalGestionObraAlerta[0];
          if ( gestionAlertas.seIdentificaronAlertas !== undefined && gestionAlertas.registroCompleto === false ) {
            this.semaforoAlertas = 'en-proceso';
          }
          if ( gestionAlertas.registroCompleto === true ) {
            this.semaforoAlertas = 'completo';
          }
        }
      }
    }

}
