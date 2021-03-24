import { Router } from '@angular/router';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-form-soporte-solicitud-url',
  templateUrl: './form-soporte-solicitud-url.component.html',
  styleUrls: ['./form-soporte-solicitud-url.component.scss']
})
export class FormSoporteSolicitudUrlComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esExpensas = false;
    @Input() listaMenusId: any;
    @Input() soporteSolicitudCodigo: string;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    esAutorizar: boolean;
    observacion: any;
    addressForm = this.fb.group({
      urlSoporte: [null, Validators.required]
    });
    solicitudPagoSoporteSolicitudId = 0;
    solicitudPagoId = 0;
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService, )
    { }

    ngOnInit(): void {
        if ( this.solicitudPago !== undefined ) {
            this.solicitudPagoId = this.solicitudPago.solicitudPagoId;
            if ( this.solicitudPago.solicitudPagoSoporteSolicitud.length > 0 ) {
                this.estaEditando = true;
                this.addressForm.markAllAsTouched();
                this.solicitudPagoSoporteSolicitudId = this.solicitudPago.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId;
                this.addressForm.get( 'urlSoporte' ).setValue( this.solicitudPago.solicitudPagoSoporteSolicitud[0].urlSoporte !== undefined ? this.solicitudPago.solicitudPagoSoporteSolicitud[0].urlSoporte : null );

                // Get observacion CU autorizar solicitud de pago 4.1.9
                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                    this.listaMenusId.autorizarSolicitudPagoId,
                    this.solicitudPago.solicitudPagoId,
                    this.solicitudPago.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId,
                    this.soporteSolicitudCodigo )
                    .subscribe(
                        response => {
                            const observacion = response.find( obs => obs.archivada === false );
                            if ( observacion !== undefined ) {
                                this.esAutorizar = true;
                                this.observacion = observacion;

                                if ( this.observacion.tieneObservacion === true ) {
                                    this.semaforoObservacion.emit( true );
                                }
                            }
                        }
                    );

                // Get observacion CU verificar solicitud de pago 4.1.8
                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                    this.listaMenusId.aprobarSolicitudPagoId,
                    this.solicitudPago.solicitudPagoId,
                    this.solicitudPago.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId,
                    this.soporteSolicitudCodigo )
                    .subscribe(
                        response => {
                            const observacion = response.find( obs => obs.archivada === false );
                            if ( observacion !== undefined ) {
                                this.esAutorizar = false;
                                this.observacion = observacion;

                                if ( this.observacion.tieneObservacion === true ) {
                                    this.semaforoObservacion.emit( true );
                                }
                            }
                        }
                    );
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
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        if ( this.solicitudPago.solicitudPagoSoporteSolicitud.length === 0 ) {
            const solicitudPagoSoporteSolicitud = [
                {
                    solicitudPagoSoporteSolicitudId: this.solicitudPagoSoporteSolicitudId,
                    solicitudPagoId: this.solicitudPagoId,
                    urlSoporte: this.addressForm.get( 'urlSoporte' ).value
                }
            ]
            this.solicitudPago.solicitudPagoSoporteSolicitud = solicitudPagoSoporteSolicitud;
        } else {
            this.solicitudPago.solicitudPagoSoporteSolicitud[0].urlSoporte = this.addressForm.get( 'urlSoporte' ).value;
        }

        this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.observacion !== undefined ) {
                        this.observacion.archivada = !this.observacion.archivada;
                        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( this.observacion ).subscribe();
                    }
                    if ( this.esExpensas === false ) {
                        this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPago.solicitudPagoId )
                            .subscribe(
                                () => {
                                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                        () => this.routes.navigate(
                                            [
                                                '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
                                            ]
                                        )
                                    );
                                }
                            );
                    }
                    if ( this.esExpensas === true ) {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate(
                                [
                                    '/registrarValidarRequisitosPago/verDetalleEditarExpensas', this.solicitudPagoId
                                ]
                            )
                        );
                    }
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }
}
