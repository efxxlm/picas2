import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-accordion-detalle-giro-gog',
  templateUrl: './accordion-detalle-giro-gog.component.html',
  styleUrls: ['./accordion-detalle-giro-gog.component.scss']
})
export class AccordionDetalleGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    ordenGiro: any;
    tieneDescuentosDireccionTecnica = true;
    listaSemaforos = {
        semaforoEstrategiaPago: 'sin-diligenciar',
        semaforoDescuentosDireccionTecnica: 'sin-diligenciar',
        semaforoTerceroCausacion: 'sin-diligenciar',
        semaforoOrigen: 'sin-diligenciar',
        semaforoObservacion: 'sin-diligenciar',
        semaforoSoporteUrl: 'sin-diligenciar'
    };

    constructor() { }

    ngOnInit(): void {
        // Verificar si se diligenciaron descuentos desde la direccion tecnica en CU 4.1.7
        const solicitudPagoFaseFactura = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseFactura[0];
        if ( solicitudPagoFaseFactura.tieneDescuento === false && solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.length === 0 ) {
            this.tieneDescuentosDireccionTecnica = false;
            // Delete propiedad del semaforo "semaforoDescuentosDireccionTecnica" por que no tiene descuentos de direccion tecnica
            delete this.listaSemaforos.semaforoDescuentosDireccionTecnica;
        }
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
                    // Get semaforo descuentos direccion tecnica
                    if ( ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                            let registroCompleto = 0;

                            ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.forEach( descuentoTecnica => {
                                if ( descuentoTecnica.registroCompleto === true ) {
                                    registroCompleto++;
                                }
                            } )

                            if ( registroCompleto === 0 ) {
                                this.listaSemaforos.semaforoDescuentosDireccionTecnica = 'en-proceso';
                            }
                            if ( registroCompleto > 0 && registroCompleto < ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length ) {
                                this.listaSemaforos.semaforoDescuentosDireccionTecnica = 'en-proceso';
                            }
                            if ( registroCompleto > 0 && registroCompleto === ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length ) {
                                this.listaSemaforos.semaforoDescuentosDireccionTecnica = 'completo';
                            }
                        }
                    }
                    // Get semaforo tercero de causacion
                    if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            let registroCompleto = 0;

                            ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                                if ( terceroCausacion.registroCompleto === true ) {
                                    registroCompleto++;
                                }
                            } )

                            if ( registroCompleto === 0 ) {
                                this.listaSemaforos.semaforoTerceroCausacion = 'en-proceso';
                            }
                            if ( registroCompleto > 0 && registroCompleto < ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length ) {
                                this.listaSemaforos.semaforoTerceroCausacion = 'en-proceso';
                            }
                            if ( registroCompleto > 0 && registroCompleto === ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length ) {
                                this.listaSemaforos.semaforoTerceroCausacion = 'completo';
                            }
                        }
                    }
                    // Get semaforo observaciones
                    if ( ordenGiroDetalle.ordenGiroDetalleObservacion !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleObservacion.length > 0 ) {
                            const ordenGiroObservacion = ordenGiroDetalle.ordenGiroDetalleObservacion[0];

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
        // Check semaforo principal
        const tieneSinDiligenciar = Object.values( this.listaSemaforos ).includes( 'sin-diligenciar' );
        const tieneEnProceso = Object.values( this.listaSemaforos ).includes( 'en-proceso' );
        const tieneCompleto = Object.values( this.listaSemaforos ).includes( 'completo' );

        if ( tieneEnProceso === true ) {
            this.estadoSemaforo.emit( 'en-proceso' );
        }
        if ( tieneSinDiligenciar === true && tieneCompleto === true ) {
            this.estadoSemaforo.emit( 'en-proceso' );
        }
        if ( tieneSinDiligenciar === false && tieneEnProceso === false && tieneCompleto === true ) {
            this.estadoSemaforo.emit( 'completo' );
        }
    }

    checkSemaforoOrigen( value: boolean ) {
        if ( value === false ) {
            delete this.listaSemaforos.semaforoOrigen;
        }
    }

}
