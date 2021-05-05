import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

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
    semaforoDetalle = 'sin-diligenciar';
    modalidadContratoArray: Dominio[] = [];

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenPagoSvc: OrdenPagoService,
        private commonSvc: CommonService )
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
                    response => {
                        this.solicitudPago = response;
                        this.contrato = response[ 'contratoSon' ];
                        console.log( this.solicitudPago );

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

        if ( tieneEnProceso === true ) {
            this.semaforoDetalle = 'en-proceso';
        }
        if ( tieneSinDiligenciar === true && tieneCompleto === true ) {
            this.semaforoDetalle = 'en-proceso';
        }
        if ( tieneSinDiligenciar === false && tieneEnProceso === false && tieneCompleto === true ) {
            this.semaforoDetalle = 'completo';
        }
    }

}
