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
    semaforoInfoGeneral = 'sin-diligenciar';
    modalidadContratoArray: Dominio[] = [];
    listaDetalleGiro: { contratacionProyectoId: number, llaveMen: string, fases: any[], semaforoDetalle: string }[] = [];

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

                            solicitudPagoFase.forEach( fase => {
                                const CRITERIOS = []
                                // Objeto fase que se agregara al array de fases del objeto proyecto
                                const FASE = {
                                    esPreconstruccion: fase.esPreconstruccion,
                                    solicitudPagoFaseId: fase.solicitudPagoFaseId,
                                    solicitudPagoRegistrarSolicitudPagoId: fase.solicitudPagoRegistrarSolicitudPagoId,
                                    solicitudPagoFaseFactura: fase.solicitudPagoFaseFactura,
                                    criteriosFase: []
                                }

                                fase.solicitudPagoFaseCriterio.forEach( criterio => {
                                    // Busqueda de los proyectos asociados a la fase y criterio
                                    const CRITERIOS_PROYECTO = criterio.solicitudPagoFaseCriterioProyecto.find( criterioProyecto => criterioProyecto.contratacionProyectoId === proyecto.contratacionProyectoId )
                                    
                                    if ( CRITERIOS_PROYECTO !== undefined ) {
                                        CRITERIOS.push( criterio )
                                    }
                                } )

                                FASE.criteriosFase = CRITERIOS

                                if ( FASE.criteriosFase.length > 0 ) {
                                    PROYECTO.fases.push( FASE )
                                }
                            } )

                            if ( PROYECTO.fases.length > 0 ) {
                                this.listaDetalleGiro.push( PROYECTO )
                            }
                        } )

                        // Get semaforo informacion general
                        if ( this.solicitudPago.ordenGiro !== undefined ) {
                            if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                                if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                                    this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
                
                                    if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                                        if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                                            const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];
                
                                            if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === false ) {
                                                this.semaforoInfoGeneral = 'en-proceso';
                                            }
                                            if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === true ) {
                                                this.semaforoInfoGeneral = 'completo';
                                            }
                                        }
                                    }
                
                                    if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                                        if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                                            const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];
                
                                            if ( ordenGiroTerceroChequeGerencia.registroCompleto === false ) {
                                                this.semaforoInfoGeneral = 'en-proceso';
                                            }
                                            if ( ordenGiroTerceroChequeGerencia.registroCompleto === true ) {
                                                this.semaforoInfoGeneral = 'completo';
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

}
