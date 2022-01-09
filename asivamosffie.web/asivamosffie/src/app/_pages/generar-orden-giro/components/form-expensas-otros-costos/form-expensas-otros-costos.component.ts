import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

@Component({
  selector: 'app-form-expensas-otros-costos',
  templateUrl: './form-expensas-otros-costos.component.html',
  styleUrls: ['./form-expensas-otros-costos.component.scss']
})
export class FormExpensasOtrosCostosComponent implements OnInit {

    contrato: any;
    solicitudPago: any;
    ordenGiroTercero: any;
    esRegistroNuevo: boolean;
    esVerDetalle = false;
    esExpensas = false;
    registroCompletoTerceroGiro = false;
    semaforoInfoGeneral = 'sin-diligenciar';
    semaforoDetalle = 'en-alerta';

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenPagoSvc: OrdenPagoService,
        private commonSvc: CommonService )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            // Get PATH formulario expensas
            if ( urlSegment.path === 'generarOrdenExpensas' ) {
                this.esRegistroNuevo = true;
                this.esExpensas = true;
                return;
            }
            if ( urlSegment.path === 'editarOrdenExpensas' ) {
                this.esRegistroNuevo = false;
                this.esExpensas = true;
                return;
            }
            if ( urlSegment.path === 'detalleOrdenExpensas' ) {
                this.esVerDetalle = true;
                this.esExpensas = true;
                return;
            }
            // Get PATH formulario otros costos y servicios
            if ( urlSegment.path === 'generarOrdenOtrosCostos' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'editarOrdenOtrosCostos' ) {
                this.esRegistroNuevo = false;
                return;
            }
            if ( urlSegment.path === 'detalleOrdenOtrosCostos' ) {
                this.esVerDetalle = true;
                return;
            }
        } );
        this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id , false)
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
                                            this.registroCompletoTerceroGiro = true;
                                            this.semaforoDetalle = 'sin-diligenciar';
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
                                            this.registroCompletoTerceroGiro = true;
                                            this.semaforoDetalle = 'sin-diligenciar';
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            );
    }

    ngOnInit(): void {
    }

}
