import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { MediosPagoCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-form-orden-giro-seleccionada',
  templateUrl: './form-orden-giro-seleccionada.component.html',
  styleUrls: ['./form-orden-giro-seleccionada.component.scss']
})
export class FormOrdenGiroSeleccionadaComponent implements OnInit {

    @Input() ordenGiro: FormGroup;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    balanceFinancieroTrasladoId = 0;
    listaMediosPagoCodigo = MediosPagoCodigo;
    addressForm: FormGroup;
    listaModalidad: Dominio[] = [];
    listaMedioPago: Dominio[] = [];
    listaBancos: Dominio[] = [];
    solicitudPago: any;
    solicitudPagoFase: any;
    ordenGiroTercero: any;
    ordenGiroId: 0;
    ordenGiroTerceroId: 0;
    semaforoDetalle = 'sin-diligenciar';
    semaforoTerceroCausacion: string;
    semaforoDescuentos: string;

    constructor(
        private ordenPagoSvc: OrdenPagoService,
        private commonSvc: CommonService,
        private balanceSvc: FinancialBalanceService,
        private fb: FormBuilder )
    {
        this.addressForm = this.crearFormulario()
    }

    async ngOnInit() {
        this.listaModalidad = await this.commonSvc.modalidadesContrato().toPromise()
        this.listaMedioPago = await this.commonSvc.listaMediosPago().toPromise()
        this.listaBancos = await this.commonSvc.listaBancos().toPromise()
        this.solicitudPago = await this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId( this.ordenGiro.get( 'solicitudPagoId' ).value , false).toPromise()

        console.log( this.solicitudPago )
        this.getDataTerceroGiro();
    }

    crearFormulario() {
        return this.fb.group({
            medioPagoGiroContrato: [null, Validators.required],
            transferenciaElectronica: this.fb.group( {
                ordenGiroTerceroId: [ 0 ],
                ordenGiroTerceroTransferenciaElectronicaId: [ 0 ],
                titularCuenta: [ '' ],
                titularNumeroIdentificacion: [ '' ],
                numeroCuenta: [ '' ],
                bancoCodigo: [ null ],
                esCuentaAhorros: [ null ]
            } ),
            chequeGerencia: this.fb.group( {
                ordenGiroTerceroId: [ 0 ],
                ordenGiroTerceroChequeGerenciaId: [ 0 ],
                nombreBeneficiario: [ '' ],
                numeroIdentificacionBeneficiario: [ '' ]
            } )
        })
    }

    getDataTerceroGiro() {
        this.commonSvc.listaMediosPago()
        .subscribe( listaMediosPago => {
            this.listaMedioPago = listaMediosPago;

            this.commonSvc.listaBancos()
                .subscribe( async listaBancos => {
                    this.listaBancos = listaBancos;

                    if ( this.solicitudPago.ordenGiro !== undefined ) {
                        this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

                        if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                            if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                                this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
                                this.ordenGiroTerceroId = this.ordenGiroTercero.ordenGiroTerceroId;
                                // Get data tercero de giro
                                const medioPago = this.listaMedioPago.find( medio => medio.codigo === this.ordenGiroTercero.medioPagoGiroCodigo );

                                if ( medioPago !== undefined ) {
                                    this.addressForm.get( 'medioPagoGiroContrato' ).setValue( medioPago.codigo );
                                }

                                if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                                    if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                                        const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];

                                        this.addressForm.get( 'transferenciaElectronica' ).setValue(
                                            {
                                                ordenGiroTerceroId: this.ordenGiroTerceroId,
                                                ordenGiroTerceroTransferenciaElectronicaId: ordenGiroTerceroTransferenciaElectronica.ordenGiroTerceroTransferenciaElectronicaId,
                                                titularCuenta: ordenGiroTerceroTransferenciaElectronica.titularCuenta !== undefined ? ordenGiroTerceroTransferenciaElectronica.titularCuenta : '',
                                                titularNumeroIdentificacion: ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion !== undefined ? ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion : '',
                                                numeroCuenta: ordenGiroTerceroTransferenciaElectronica.numeroCuenta !== undefined ? ordenGiroTerceroTransferenciaElectronica.numeroCuenta : '',
                                                bancoCodigo: ordenGiroTerceroTransferenciaElectronica.bancoCodigo !== undefined ? ordenGiroTerceroTransferenciaElectronica.bancoCodigo : null,
                                                esCuentaAhorros: ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros !== undefined ? ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros : null
                                            }
                                        )
                                    }
                                }

                                if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                                    if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                                        const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];

                                        this.addressForm.get( 'chequeGerencia' ).setValue(
                                            {
                                                ordenGiroTerceroId: this.ordenGiroTerceroId,
                                                ordenGiroTerceroChequeGerenciaId: ordenGiroTerceroChequeGerencia.ordenGiroTerceroChequeGerenciaId,
                                                nombreBeneficiario: ordenGiroTerceroChequeGerencia.nombreBeneficiario !== undefined ? ordenGiroTerceroChequeGerencia.nombreBeneficiario : '',
                                                numeroIdentificacionBeneficiario: ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario !== undefined ? ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario : ''
                                            }
                                        )
                                    }
                                }
                            }
                        }
                    }
                } );
        } );
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.listaModalidad.length > 0 ) {
            const modalidad = this.listaModalidad.find( modalidad => modalidad.codigo === modalidadCodigo )

            if ( modalidad !== undefined ) {
                return modalidad.nombre
            }
        }
    }

    getMedioPago( codigo: string ) {
        if ( this.listaMedioPago.length > 0 ) {
            const medioPago = this.listaMedioPago.find( medioPago => medioPago.codigo === codigo )

            if ( medioPago !== undefined ) {
                return medioPago.nombre
            }
        }
    }

    getBanco( codigo: string ) {
        if ( this.listaBancos.length > 0 ) {
            const banco = this.listaBancos.find( banco => banco.codigo === codigo )

            if ( banco !== undefined ) {
                return banco.nombre
            }
        }
    }

}
