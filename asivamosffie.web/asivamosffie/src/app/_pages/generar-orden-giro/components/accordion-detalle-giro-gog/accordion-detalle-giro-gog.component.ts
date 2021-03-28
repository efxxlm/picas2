import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-accordion-detalle-giro-gog',
  templateUrl: './accordion-detalle-giro-gog.component.html',
  styleUrls: ['./accordion-detalle-giro-gog.component.scss']
})
export class AccordionDetalleGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    ordenGiro: any;
    listaSemaforos = {
        semaforoEstrategiaPago: 'sin-diligenciar',
        semaforoDescuentosDireccionTecnica: 'sin-diligenciar',
        semaforoTerceroCausacion: 'sin-diligenciar',
        semaforoObservacion: 'sin-diligenciar',
        semaforoSoporteUrl: 'sin-diligenciar'
    };

    constructor() { }

    ngOnInit(): void {
        // Get semaforo acordeones
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiro = this.solicitudPago.ordenGiro;

            if ( this.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    const ordenGiroDetalle = this.ordenGiro.ordenGiroDetalle[0];
                    
                    // Get semaforo estrategia de pago
                    if ( ordenGiroDetalle.ordenGiroDetalleEstrategiaPago !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleEstrategiaPago.length > 0 ) {
                            const ordenGiroDetalleEstrategiaPago = ordenGiroDetalle.ordenGiroDetalleEstrategiaPago[0];

                            if ( ordenGiroDetalleEstrategiaPago.registroCompleto === false ) {
                                this.listaSemaforos.semaforoEstrategiaPago = 'en-proceso';
                            }
                            if ( ordenGiroDetalleEstrategiaPago.registroCompleto === true ) {
                                this.listaSemaforos.semaforoEstrategiaPago = 'completo';
                            }
                        }
                    }
                    // Get semaforo observaciones
                    if ( ordenGiroDetalle.ordenGiroObservacion !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroObservacion.length > 0 ) {
                            const ordenGiroObservacion = ordenGiroDetalle.ordenGiroObservacion[0];

                            if ( ordenGiroObservacion.registroCompleto === false ) {
                                this.listaSemaforos.semaforoObservacion = 'en-proceso';
                            }
                            if ( ordenGiroObservacion.registroCompleto === true ) {
                                this.listaSemaforos.semaforoObservacion = 'completo';
                            }
                        }
                    }
                    // Get semaforo soporte url
                    if ( ordenGiroDetalle.ordenGiroSoporte !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroSoporte.length > 0 ) {
                            const ordenGiroSoporte = ordenGiroDetalle.ordenGiroSoporte[0];
    
                            if ( ordenGiroSoporte.registroCompleto === false ) {
                                this.listaSemaforos.semaforoSoporteUrl = 'en-proceso';
                            }
                            if ( ordenGiroSoporte.registroCompleto === true ) {
                                this.listaSemaforos.semaforoSoporteUrl = 'completo';
                            }
                        }
                    }
                }
            }
        }
    }

}
