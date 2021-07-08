import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-accordion-detalle-giro-gog',
  templateUrl: './accordion-detalle-giro-gog.component.html',
  styleUrls: ['./accordion-detalle-giro-gog.component.scss']
})
export class AccordionDetalleGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() proyecto: any;
    @Output() estadoSemaforo = new EventEmitter<any>();
    ordenGiro: any;
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
    tieneObsDescuentos = false;
    tieneObsDescuentosConstruccion = false;
    tieneObsTercero = false;
    tieneObsTerceroConstruccion = false;

    constructor() { }

    ngOnInit(): void {
        console.log( this.proyecto );
        let tieneFasePreconstruccion = false;
        let tieneFaseConstruccion = false;
        // Verificar las fases diligenciadas en solicitud de pago;
        if ( this.proyecto.fases !== undefined ) {
            if ( this.proyecto.fases.length > 0 ) {
                const fasePreconstruccion = this.proyecto.fases.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === true );
                const faseConstruccion = this.proyecto.fases.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === false );

                if ( fasePreconstruccion !== undefined ) {
                    tieneFasePreconstruccion = true;
                }
                if ( faseConstruccion !== undefined ) {
                    tieneFaseConstruccion = true;
                }
            }
        }

        if ( tieneFasePreconstruccion === false ) {
            delete this.listaSemaforos.semaforoDescuentosDireccionTecnica
            delete this.listaSemaforos.semaforoTerceroCausacion
        }
        if ( tieneFaseConstruccion === false ) {
            delete this.listaSemaforos.semaforoDescuentosDireccionTecnicaConstruccion
            delete this.listaSemaforos.semaforoTerceroCausacionConstruccion
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

        this.estadoSemaforo.emit( this.listaSemaforos )
    }

    checkSemaforoOrigen( value: boolean ) {
        if ( value === false ) {
            delete this.listaSemaforos.semaforoOrigen;
        }
    }

    checkTieneDescuentos( esPreconstruccion: boolean ) {
        const solicitudPagoFase = this.proyecto.fases.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === esPreconstruccion );
        
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

        if ( esPreconstruccion === true && this.tieneObsDescuentos === true ) {
            semaforo = 'en-proceso';
            this.listaSemaforos.semaforoDescuentosDireccionTecnica = semaforo;

            return semaforo;
        }

        if ( esPreconstruccion === false && this.tieneObsDescuentosConstruccion === true ) {
            semaforo = 'en-proceso';
            this.listaSemaforos.semaforoDescuentosDireccionTecnicaConstruccion = semaforo;
            return semaforo;
        }

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

        if ( esPreconstruccion === true ) {
            this.listaSemaforos.semaforoDescuentosDireccionTecnica = semaforo;
        }
        if ( esPreconstruccion === false ) {
            this.listaSemaforos.semaforoDescuentosDireccionTecnicaConstruccion = semaforo;
        }
        this.estadoSemaforo.emit( this.listaSemaforos );

        return semaforo;
    }

    checkSemaforoTercero( esPreconstruccion: boolean ) {
        let semaforo = 'sin-diligenciar';

        if ( esPreconstruccion === true && this.tieneObsTercero === true ) {
            semaforo = 'en-proceso';
            this.listaSemaforos.semaforoTerceroCausacion = semaforo;

            return semaforo;
        }

        if ( esPreconstruccion === false && this.tieneObsTerceroConstruccion === true ) {
            semaforo = 'en-proceso';
            this.listaSemaforos.semaforoTerceroCausacionConstruccion = semaforo;

            return semaforo;
        }

        if ( this.ordenGiro !== undefined ) {
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
        }

        if ( esPreconstruccion === true ) {
            this.listaSemaforos.semaforoTerceroCausacion = semaforo;
        }
        if ( esPreconstruccion === false ) {
            this.listaSemaforos.semaforoTerceroCausacionConstruccion = semaforo;
        }
        this.estadoSemaforo.emit( this.listaSemaforos );

        return semaforo;
    }

}
