import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-form-generar-orden-giro',
  templateUrl: './form-generar-orden-giro.component.html',
  styleUrls: ['./form-generar-orden-giro.component.scss']
})
export class FormGenerarOrdenGiroComponent implements OnInit {

    solicitudPago: any;
    contrato: any;
    ordenGiroTercero: any;
    esRegistroNuevo: boolean;
    esVerDetalle = false;
    ordenGiro: any;
    modalidadContratoArray: Dominio[] = [];
    listaDetalleGiro: { contratacionProyectoId: number, llaveMen: string, fases: any[], semaforoDetalle: string }[] = [];
    estadoSemaforos = {
        acordeonInformacionGeneral: 'sin-diligenciar',
        acordeonEstrategiaPago: 'sin-diligenciar',
        acordeonObservacion: 'sin-diligenciar',
        acordeonSoporteOrdenGiro: 'sin-diligenciar'
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenPagoSvc: OrdenPagoService,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'generacionOrdenGiro' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'verDetalleEditarOrdenGiro' ) {
                this.esRegistroNuevo = false;
                return;
            }
            if ( urlSegment.path === 'verDetalleOrdenGiro' ) {
                this.esVerDetalle = true;
            }
        } );
        this.commonSvc.modalidadesContrato()
        .subscribe( response => {
            this.modalidadContratoArray = response;
            this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id )
                .subscribe(
                    async response => {
                        this.solicitudPago = response;
                        this.contrato = response[ 'contratoSon' ];
                        console.log( this.solicitudPago );
                        /*
                            Se crea un arreglo de proyectos asociados a una fase y unos criterios que estan asociados a esa fase y al proyecto para
                            el nuevo flujo de Orden de Giro el cual los acordeones de "Estrategia de pagos, Observaciones y Soporte de orden de giro" ya no son hijos del acordeon
                            "Detalle de giro" y el detalle de giro se diligencia por proyectos el cual tendra como hijo directo las fases y los criterios asociados a esa fase y al proyecto.
                        */
                        // Peticion asincrona de los proyectos por contratoId
                        const getProyectosByIdContrato: any[] = await this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId ).toPromise();
                        const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[1];
                        const solicitudPagoFase: any[] = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase;

                        LISTA_PROYECTOS.forEach( proyecto => {
                            // Objeto Proyecto que se agregara al array listaDetalleGiro
                            const PROYECTO = {
                                semaforoDetalle: 'sin-diligenciar',
                                contratacionProyectoId: proyecto.contratacionProyectoId,
                                llaveMen: proyecto.llaveMen,
                                fases: []
                            }

                            const listFase = solicitudPagoFase.filter( fase => fase.contratacionProyectoId === proyecto.contratacionProyectoId )
                            if ( listFase.length > 0 ) {
                                listFase.forEach( fase => {
                                    fase.estadoSemaforo = 'sin-diligenciar'
                                    fase.estadoSemaforoCausacion = 'sin-diligenciar'
                                })
                            }
                            PROYECTO.fases = listFase

                            if ( PROYECTO.fases.length > 0 ) {
                                this.listaDetalleGiro.push( PROYECTO )
                            }
                        } )

                        // Get semaforo acordeones
                        // Get semaforo informacion general
                        if ( this.solicitudPago.ordenGiro !== undefined ) {
                            if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                                if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                                    this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
                
                                    if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                                        if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                                            const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];
                
                                            if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === false ) {
                                                this.estadoSemaforos.acordeonInformacionGeneral = 'en-proceso';
                                            }
                                            if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === true ) {
                                                this.estadoSemaforos.acordeonInformacionGeneral = 'completo';
                                            }
                                        }
                                    }
                
                                    if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                                        if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                                            const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];
                
                                            if ( ordenGiroTerceroChequeGerencia.registroCompleto === false ) {
                                                this.estadoSemaforos.acordeonInformacionGeneral = 'en-proceso';
                                            }
                                            if ( ordenGiroTerceroChequeGerencia.registroCompleto === true ) {
                                                this.estadoSemaforos.acordeonInformacionGeneral = 'completo';
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // Get semaforo estrategia de pago, observaciones y soporte de orden de giro
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
                                                this.estadoSemaforos.acordeonEstrategiaPago = 'en-proceso';
                                            }
                                            if ( ordenGiroDetalleEstrategiaPago.registroCompleto === true ) {
                                                this.estadoSemaforos.acordeonEstrategiaPago = 'completo';
                                            }
                                        }
                                    }
                                    // Get semaforo observaciones
                                    if ( ordenGiroDetalle.ordenGiroDetalleObservacion !== undefined ) {
                                        if ( ordenGiroDetalle.ordenGiroDetalleObservacion.length > 0 ) {
                                            const ordenGiroObservacion = ordenGiroDetalle.ordenGiroDetalleObservacion[0];

                                            if ( ordenGiroObservacion.registroCompleto === false ) {
                                                this.estadoSemaforos.acordeonObservacion = 'en-proceso';
                                            }
                                            if ( ordenGiroObservacion.registroCompleto === true ) {
                                                this.estadoSemaforos.acordeonObservacion = 'completo';
                                            }
                                        }
                                    }
                                    // Get semaforo soporte url
                                    if ( ordenGiroDetalle.ordenGiroSoporte !== undefined ) {
                                        if ( ordenGiroDetalle.ordenGiroSoporte.length > 0 ) {
                                            const ordenGiroSoporte = ordenGiroDetalle.ordenGiroSoporte[0];

                                            if ( ordenGiroSoporte.registroCompleto === false ) {
                                                this.estadoSemaforos.acordeonSoporteOrdenGiro = 'en-proceso';
                                            }
                                            if ( ordenGiroSoporte.registroCompleto === true ) {
                                                this.estadoSemaforos.acordeonSoporteOrdenGiro = 'completo';
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                );
        } );
    }

    ngOnInit(): void {
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

    checkSemaforoDetalle( listaSemaforosDetalle: any ) {
        const tieneSinDiligenciar = Object.values( listaSemaforosDetalle ).includes( 'sin-diligenciar' );
        const tieneEnProceso = Object.values( listaSemaforosDetalle ).includes( 'en-proceso' );
        const tieneCompleto = Object.values( listaSemaforosDetalle ).includes( 'completo' );

        /*
        if ( tieneEnProceso === true ) {
            this.semaforoDetalle = 'en-proceso';
        }
        if ( tieneSinDiligenciar === true && tieneCompleto === true ) {
            this.semaforoDetalle = 'en-proceso';
        }
        if ( tieneSinDiligenciar === false && tieneEnProceso === false && tieneCompleto === true ) {
            this.semaforoDetalle = 'completo';
        }
        */
    }

    checkTieneDescuentos( esPreconstruccion: boolean, proyecto ) {
        const solicitudPagoFase = proyecto.fases.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === esPreconstruccion );
        
        if ( solicitudPagoFase !== undefined ) {
            if ( solicitudPagoFase.tieneDescuento === true ) {
                return true;
            }
            if ( solicitudPagoFase.tieneDescuento === false ) {
                return false;
            }
        }
    }

    semaforoDetalleDelGiro(proyecto) {
        const arraySemaforoelement = []
        proyecto.fases.forEach(element => {
            arraySemaforoelement.push(element.estadoSemaforoCausacion)
            if (this.checkTieneDescuentos( element.esPreconstruccion, proyecto )) {
                arraySemaforoelement.push(element.estadoSemaforo)
            }
        });
        if (arraySemaforoelement.every(n => n === 'completo')) return 'completo'
        if (arraySemaforoelement.some(n => n === 'sin-diligenciar')) return 'sin-diligenciar'
        else return 'sin-diligenciar'
    }
}
