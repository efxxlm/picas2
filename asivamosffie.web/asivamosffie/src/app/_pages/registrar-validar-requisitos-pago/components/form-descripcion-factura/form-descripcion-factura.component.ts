import { FormBuilder, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-descripcion-factura',
  templateUrl: './form-descripcion-factura.component.html',
  styleUrls: ['./form-descripcion-factura.component.scss']
})
export class FormDescripcionFacturaComponent implements OnInit {

    @Input() contrato: any;
    solicitudPago: any;
    estaEditando = false;
    solicitudPagoId = 0;
    addressForm = this.fb.group(
        {
            esFactura: [ null, Validators.required ],
            numeroDocumento: [ null, Validators.required ],
            fechaDocumento: [ null, Validators.required ]
        }
    );

    constructor(
        private fb: FormBuilder,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private routes: Router,
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        this.solicitudPago = this.contrato.solicitudPagoOnly
        this.solicitudPagoId = this.solicitudPago.solicitudPagoId
        const solicitudPagoFactura = this.contrato.solicitudPagoOnly.solicitudPagoFactura[ 0 ]

        if ( this.solicitudPago.esFactura !== undefined ) {
            this.addressForm.get( 'esFactura' ).setValue( this.solicitudPago.esFactura )
        }

        if ( solicitudPagoFactura !== undefined ) {
            this.addressForm.get( 'numeroDocumento' ).setValue( solicitudPagoFactura.numero )
            this.addressForm.get( 'fechaDocumento' ).setValue( new Date( solicitudPagoFactura.fecha ) )
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    guardar() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();

        const pSolicitudPago = this.solicitudPago
        if ( pSolicitudPago.solicitudPagoFactura !== undefined ) {
            if ( pSolicitudPago.solicitudPagoFactura.length > 0 ) {
                pSolicitudPago.esFactura = this.addressForm.get( 'esFactura' ).value
                pSolicitudPago.solicitudPagoFactura[ 0 ].numero = this.addressForm.get( 'numeroDocumento' ).value
                pSolicitudPago.solicitudPagoFactura[ 0 ].fecha = new Date( this.addressForm.get( 'fechaDocumento' ).value ).toISOString()
            } else {
                pSolicitudPago.esFactura = this.addressForm.get( 'esFactura' ).value
                pSolicitudPago.solicitudPagoFactura = [
                    {
                        solicitudPagoFacturaId: 0,
                        solicitudPagoId: this.solicitudPagoId,
                        numero: this.addressForm.get( 'numeroDocumento' ).value,
                        fecha: new Date( this.addressForm.get( 'fechaDocumento' ).value ).toISOString()
                    }
                ]
            }
        } else {
            pSolicitudPago.esFactura = this.addressForm.get( 'esFactura' ).value
            pSolicitudPago.solicitudPagoFactura = [
                {
                    solicitudPagoFacturaId: 0,
                    solicitudPagoId: this.solicitudPagoId,
                    numero: this.addressForm.get( 'numeroDocumento' ).value,
                    fecha: new Date( this.addressForm.get( 'fechaDocumento' ).value ).toISOString()
                }
            ]
        }


        this.registrarPagosSvc.createEditNewPayment( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );

                    this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPagoId )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () => this.routes.navigate(
                                        [
                                            '/registrarValidarRequisitosPago/verDetalleEditar',  this.contrato.contratoId, this.solicitudPagoId
                                        ]
                                    )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
