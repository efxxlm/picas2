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
    solicitudPagoRegistrarSolicitudPago: any;
    listaSemaforos = {
        semaforoEstrategiaPago: 'sin-diligenciar',
        semaforoDescuentosDireccionTecnica: 'sin-diligenciar',
        semaforoTerceroCausacion: 'sin-diligenciar',
        semaforoDescuentosDireccionTecnicaConstruccion: 'sin-diligenciar',
        semaforoTerceroCausacionConstruccion: 'sin-diligenciar',
        semaforoOrigen: 'sin-diligenciar',
        semaforoObservacion: 'sin-diligenciar',
        semaforoSoporteUrl: 'sin-diligenciar'
    };

    constructor() { }

    ngOnInit(): void {
        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
        // Verificar si se diligenciaron descuentos desde la direccion tecnica en CU 4.1.7
        const solicitudPagoFaseFactura = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseFactura[0];
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
                            if ( registroCompleto > 0 && registroCompleto < ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length ) {
                                this.listaSemaforos.semaforoTerceroCausacion = 'en-proceso';
                            }
                            if ( registroCompleto > 0 && registroCompleto === ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length ) {
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
        setTimeout(() => {
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
        }, 4000);
    }

    checkSemaforoOrigen( value: boolean ) {
        if ( value === false ) {
            delete this.listaSemaforos.semaforoOrigen;
        }
    }

    checkTieneDescuentos( esPreconstruccion: boolean ) {
        const solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === esPreconstruccion );
        
        if ( solicitudPagoFase !== undefined ) {
            if ( solicitudPagoFase.solicitudPagoFaseFactura[ 0 ].tieneDescuento === true ) {
                return true;
            }

            if ( solicitudPagoFase.solicitudPagoFaseFactura[ 0 ].tieneDescuento === false ) {
                if ( esPreconstruccion === true ) {
                    if ( this.listaSemaforos.semaforoDescuentosDireccionTecnica !== undefined ) {
                        delete this.listaSemaforos.semaforoDescuentosDireccionTecnica
                    }
                }
                if ( esPreconstruccion === false ) {
                    if ( this.listaSemaforos.semaforoDescuentosDireccionTecnicaConstruccion !== undefined ) {
                        delete this.listaSemaforos.semaforoDescuentosDireccionTecnicaConstruccion
                    }
                }

                return false;
            }
        }
    }

    checkSemaforoDescuentos( esPreconstruccion: boolean ) {
        let semaforo = 'sin-diligenciar';

        if ( this.ordenGiro !== undefined ) {
            if ( this.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    const ordenGiroDetalle = this.ordenGiro.ordenGiroDetalle[0];

                    if ( ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                            const ordenGiroDetalleDescuentoTecnica: any[] = ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.filter( ordenGiroDetalleDescuentoTecnica => ordenGiroDetalleDescuentoTecnica.esPreconstruccion === esPreconstruccion );
                            let enProceso = 0;
                            let completo = 0;

                            if ( ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                                ordenGiroDetalleDescuentoTecnica.forEach( descuentoTecnico => {
                                    if ( descuentoTecnico.registroCompleto === false ) {
                                        enProceso++;
                                    }

                                    if ( descuentoTecnico.registroCompleto === true ) {
                                        completo++;
                                    }
                                } )
                            }

                            if ( enProceso > 0 && enProceso === ordenGiroDetalleDescuentoTecnica.length ) {
                                semaforo = 'en-proceso';
                            }
                            if ( enProceso > 0 && enProceso < ordenGiroDetalleDescuentoTecnica.length ) {
                                semaforo = 'en-proceso';
                            }
                            if ( completo > 0 && completo < ordenGiroDetalleDescuentoTecnica.length ) {
                                semaforo = 'en-proceso';
                            }
                            if ( completo > 0 && completo === ordenGiroDetalleDescuentoTecnica.length ) {
                                semaforo = 'completo';
                            }
                        }
                    }
                }
            }
        }

        return semaforo;
    }

    checkSemaforoTercero( esPreconstruccion: boolean ) {
        let semaforo = 'sin-diligenciar';

        if ( this.ordenGiro.ordenGiroDetalle !== undefined ) {
            if ( this.ordenGiro.ordenGiroDetalle.length > 0 ) {
                const ordenGiroDetalle = this.ordenGiro.ordenGiroDetalle[0];

                if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                    if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                        const ordenGiroDetalleTerceroCausacion = ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.filter( terceroCausacion => terceroCausacion.esPreconstruccion === esPreconstruccion );
                        let enProceso = 0;
                        let completo = 0;

                        if ( ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                                if ( terceroCausacion.registroCompleto === false ) {
                                    enProceso++;
                                }

                                if ( terceroCausacion.registroCompleto === true ) {
                                    completo++;
                                }
                            } )
                        }

                        if ( enProceso > 0 && enProceso === ordenGiroDetalleTerceroCausacion.length ) {
                            semaforo = 'en-proceso';
                        }
                        if ( enProceso > 0 && enProceso < ordenGiroDetalleTerceroCausacion.length ) {
                            semaforo = 'en-proceso';
                        }
                        if ( completo > 0 && completo < ordenGiroDetalleTerceroCausacion.length ) {
                            semaforo = 'en-proceso';
                        }
                        if ( completo > 0 && completo === ordenGiroDetalleTerceroCausacion.length ) {
                            semaforo = 'completo';
                        }
                    }
                }
            }
        }

        return semaforo;
    }

}
