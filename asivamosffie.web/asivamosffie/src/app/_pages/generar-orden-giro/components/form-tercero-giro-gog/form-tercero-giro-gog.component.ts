import { ListaMediosPagoCodigo, MediosPagoCodigo } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-tercero-giro-gog',
  templateUrl: './form-tercero-giro-gog.component.html',
  styleUrls: ['./form-tercero-giro-gog.component.scss']
})
export class FormTerceroGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean; 
    medioPagoArray: Dominio[] = [];
    bancosArray: Dominio[] = [];
    addressForm: FormGroup;
    listaMediosPagoCodigo: ListaMediosPagoCodigo = MediosPagoCodigo;
    ordenGiroTercero: any;
    ordenGiroId = 0;
    ordenGiroTerceroId = 0;
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.commonSvc.listaMediosPago()
            .subscribe( listaMediosPago => {
                this.medioPagoArray = listaMediosPago;

                this.commonSvc.listaBancos()
                    .subscribe( listaBancos => {
                        this.bancosArray = listaBancos;

                        if ( this.solicitudPago.ordenGiro !== undefined ) {
                            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
                
                            if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                                if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                                    this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
                                    this.ordenGiroTerceroId = this.ordenGiroTercero.ordenGiroTerceroId;

                                    const medioPago = this.medioPagoArray.find( medio => medio.codigo === this.ordenGiroTercero.medioPagoGiroCodigo );

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

    getMedioPago( codigo: string ) {
        if ( this.medioPagoArray.length > 0 ) {
            const medioPago = this.medioPagoArray.find( medioPago => medioPago.codigo === codigo );

            if ( medioPago !== undefined ) {
                return medioPago.nombre;
            }
        }
    }

    getBanco( codigo: string ) {
        if ( this.bancosArray.length > 0 ) {
            const banco = this.bancosArray.find( banco => banco.codigo === codigo );

            if ( banco !== undefined ) {
                return banco.nombre;
            }
        }
    }

    maxLength(e: any, n: number) {
      console.log(e.editor.getLength()+" "+n);
      if (e.editor.getLength() > n) {
        e.editor.deleteText(n-1, e.editor.getLength());
      }
    }

    textoLimpio(texto,n) {
      if (texto!=undefined) {
        return texto.getLength() > n ? n : texto.getLength();
      }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const ordenGiroTerceroDiligenciado = () => {
            if (this.listaMediosPagoCodigo.transferenciaElectronica === this.addressForm.get( 'medioPagoGiroContrato' ).value ) {
                return [
                    {
                        ordenGiroTerceroId: this.ordenGiroTerceroId,
                        ordenGiroTerceroTransferenciaElectronicaId: this.addressForm.get( 'transferenciaElectronica' ).get( 'ordenGiroTerceroTransferenciaElectronicaId' ).value,
                        titularCuenta: this.addressForm.get( 'transferenciaElectronica' ).get( 'titularCuenta' ).value,
                        titularNumeroIdentificacion: this.addressForm.get( 'transferenciaElectronica' ).get( 'titularNumeroIdentificacion' ).value,
                        numeroCuenta: this.addressForm.get( 'transferenciaElectronica' ).get( 'numeroCuenta' ).value,
                        bancoCodigo: this.addressForm.get( 'transferenciaElectronica' ).get( 'bancoCodigo' ).value,
                        esCuentaAhorros: this.addressForm.get( 'transferenciaElectronica' ).get( 'esCuentaAhorros' ).value
                    }
                ];
            }

            if ( this.listaMediosPagoCodigo.chequeGerencia === this.addressForm.get( 'medioPagoGiroContrato' ).value ) {
                return [
                    {
                        ordenGiroTerceroId: this.ordenGiroTerceroId,
                        ordenGiroTerceroChequeGerenciaId: this.addressForm.get( 'chequeGerencia' ).get( 'ordenGiroTerceroChequeGerenciaId' ).value,
                        nombreBeneficiario: this.addressForm.get( 'chequeGerencia' ).get( 'nombreBeneficiario' ).value,
                        numeroIdentificacionBeneficiario: this.addressForm.get( 'chequeGerencia' ).get( 'numeroIdentificacionBeneficiario' ).value
                    }
                ];
            }
        }

        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroTercero: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroTerceroId: this.ordenGiroTerceroId,
                    medioPagoGiroCodigo: this.addressForm.get( 'medioPagoGiroContrato' ).value,
                    [
                        this.listaMediosPagoCodigo.transferenciaElectronica === this.addressForm.get( 'medioPagoGiroContrato' ).value ? 'ordenGiroTerceroTransferenciaElectronica' : 'ordenGiroTerceroChequeGerencia'
                    ]: ordenGiroTerceroDiligenciado()
                }
            ]
        }
        
        this.ordenPagoSvc.createEditOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/generarOrdenDeGiro/generacionOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
