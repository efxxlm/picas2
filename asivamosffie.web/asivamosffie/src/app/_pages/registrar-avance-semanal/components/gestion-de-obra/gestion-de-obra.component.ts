import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-gestion-de-obra',
  templateUrl: './gestion-de-obra.component.html',
  styleUrls: ['./gestion-de-obra.component.scss']
})
export class GestionDeObraComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Output() estadoSemaforoGestionObra = new EventEmitter();
    semaforoAmbiental = 'sin-diligenciar';
    semaforoCalidad = 'sin-diligenciar';
    semaforoSst = 'sin-diligenciar';
    semaforoSocial = 'sin-diligenciar';
    semaforoAlertas = 'sin-diligenciar';

    constructor() { }

    ngOnInit(): void {
        if ( this.esVerDetalle === true ) {
            const seguimientoSemanalObra = this.seguimientoSemanal.seguimientoSemanalGestionObra[0];
            // Semaforo Calidad
            let sinDiligenciar = 0;
            let enProceso = 0;
            if ( seguimientoSemanalObra.seguimientoSemanalGestionObraCalidad.length > 0 ) {
                const gestionCalidad = seguimientoSemanalObra.seguimientoSemanalGestionObraCalidad[0];
                for ( const ensayo of  gestionCalidad.gestionObraCalidadEnsayoLaboratorio ) {
                    if ( ensayo.registroCompletoMuestras === false ) {
                        enProceso++;
                    }
                    if ( ensayo.registroCompletoMuestras === undefined ) {
                        sinDiligenciar++;
                    }
                }

                if ( sinDiligenciar > 0 && ( sinDiligenciar === seguimientoSemanalObra.seguimientoSemanalGestionObraCalidad.length ) ) {
                    this.semaforoCalidad = 'sin-diligenciar';
                }

                if ( enProceso > 0 && ( enProceso < seguimientoSemanalObra.seguimientoSemanalGestionObraCalidad.length ) ) {
                    this.semaforoCalidad = 'en-proceso';
                }
                if ( enProceso === 0 && sinDiligenciar === 0 ) {
                  this.semaforoCalidad = '';
                }
                this.estadoSemaforoGestionObra.emit( this.semaforoCalidad );
                
                // if ( gestionCalidad.seRealizaronEnsayosLaboratorio !== undefined && gestionCalidad.registroCompleto === false ) {
                //     this.semaforoCalidad = 'en-proceso';
                // }
                // if ( gestionCalidad.registroCompleto === true ) {
                //     this.semaforoCalidad = 'completo';
                // }
            }
        }
        if ( this.esVerDetalle === false ) {
            if ( this.seguimientoSemanal !== undefined && this.seguimientoSemanal.seguimientoSemanalGestionObra.length === 0 ) {
                this.estadoSemaforoGestionObra.emit( 'sin-diligenciar' );
              }
              if ( this.seguimientoSemanal !== undefined && this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                let sinDiligenciar = 0;
                let completo = 0;
                const totalAcordeones = 5;
                const seguimientoSemanalObra = this.seguimientoSemanal.seguimientoSemanalGestionObra[0];
                // Semaforo ambiental
                if ( seguimientoSemanalObra.seguimientoSemanalGestionObraAmbiental.length > 0 ) {
                  const gestionAmbiental = seguimientoSemanalObra.seguimientoSemanalGestionObraAmbiental[0];
                  if ( gestionAmbiental.seEjecutoGestionAmbiental !== undefined && gestionAmbiental.registroCompleto === false ) {
                    this.semaforoAmbiental = 'en-proceso';
                  }
                  if ( gestionAmbiental.registroCompleto === true ) {
                    this.semaforoAmbiental = 'completo';
                  }
                }
        
                if ( this.semaforoAmbiental === 'sin-diligenciar' ) {
                  sinDiligenciar++;
                }
        
                if ( this.semaforoAmbiental === 'completo' ) {
                  completo++;
                }
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
        
                if ( this.semaforoCalidad === 'sin-diligenciar' ) {
                  sinDiligenciar++;
                }
        
                if ( this.semaforoCalidad === 'completo' ) {
                  completo++;
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
        
                if ( this.semaforoSst === 'sin-diligenciar' ) {
                  sinDiligenciar++;
                }
        
                if ( this.semaforoSst === 'completo' ) {
                  completo++;
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
        
                if ( this.semaforoSocial === 'sin-diligenciar' ) {
                  sinDiligenciar++;
                }
        
                if ( this.semaforoSocial === 'completo' ) {
                  completo++;
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
        
                if ( this.semaforoAlertas === 'sin-diligenciar' ) {
                  sinDiligenciar++;
                }
        
                if ( this.semaforoAlertas === 'completo' ) {
                  completo++;
                }
        
                if ( totalAcordeones === completo ) {
                  this.estadoSemaforoGestionObra.emit( 'completo' );
                }
                if ( totalAcordeones === sinDiligenciar ) {
                    this.estadoSemaforoGestionObra.emit( 'sin-diligenciar' );
                }
                if ( totalAcordeones > completo ) {
                    this.estadoSemaforoGestionObra.emit( 'en-proceso' );
                }
            }
        }
    }

}
