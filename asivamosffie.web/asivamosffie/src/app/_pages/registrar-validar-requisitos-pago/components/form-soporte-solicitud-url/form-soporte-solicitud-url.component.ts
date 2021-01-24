import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-form-soporte-solicitud-url',
  templateUrl: './form-soporte-solicitud-url.component.html',
  styleUrls: ['./form-soporte-solicitud-url.component.scss']
})
export class FormSoporteSolicitudUrlComponent implements OnInit {

    @Input() contrato: any;
    addressForm = this.fb.group({
      urlSoporte: [null, Validators.required]
    });
    solicitudPago: any;
    solicitudPagoSoporteSolicitudId = 0;
    solicitudPagoId = 0;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService, )
    { }

    ngOnInit(): void {
        if ( this.contrato !== undefined ) {
            if ( this.contrato.solicitudPagoOnly !== undefined ) {
                this.solicitudPago = this.contrato.solicitudPagoOnly;
                this.solicitudPagoId = this.solicitudPago.solicitudPagoId;
            }
            if ( this.solicitudPago.solicitudPagoSoporteSolicitud.length > 0 ) {
                this.solicitudPagoSoporteSolicitudId = this.solicitudPago.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId;
                this.addressForm.get( 'urlSoporte' ).setValue( this.solicitudPago.solicitudPagoSoporteSolicitud[0].urlSoporte !== undefined ? this.solicitudPago.solicitudPagoSoporteSolicitud[0].urlSoporte : null );
            }
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        console.log(this.addressForm.value);
        const solicitudPagoSoporteSolicitud = [
            {
                solicitudPagoSoporteSolicitudId: this.solicitudPagoSoporteSolicitudId,
                solicitudPagoId: this.solicitudPagoId,
                urlSoporte: this.addressForm.get( 'urlSoporte' ).value
            }
        ]
        this.solicitudPago.solicitudPagoSoporteSolicitud = solicitudPagoSoporteSolicitud;
        this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }
}
