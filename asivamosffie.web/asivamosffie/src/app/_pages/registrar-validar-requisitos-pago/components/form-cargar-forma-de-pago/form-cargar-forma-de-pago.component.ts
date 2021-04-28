import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
    selector: 'app-form-cargar-forma-de-pago',
    templateUrl: './form-cargar-forma-de-pago.component.html',
    styleUrls: ['./form-cargar-forma-de-pago.component.scss']
})
export class FormCargarFormaDePagoComponent implements OnInit {

    @Input() contrato: any;
    @Input() tipoSolicitud: string;
    @Input() esUnEditar = false;
    @Input() cargarFormaPagoCodigo: string;
    @Input() listaMenusId: any;
    formaDePago: any;
    formaPagoArrayPreconstruccion: Dominio[] = [];
    formaPagoArrayConstruccion: Dominio[] = [];
    solicitudPagoId = 0;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoCargarFormaPagoId = 0;
    solicitudPagoObservacionId = 0;
    tieneFase1 = false;
    estaEditando = false;
    esAutorizar: boolean;
    observacion: any;
    addressForm = this.fb.group({
        formaPagoPreconstruccion: [null, Validators.required],
        formaPagoConstruccion: [null, Validators.required]
    });

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService)
    {
    }

    async ngOnInit() {
        this.formaPagoArrayPreconstruccion = await this.registrarPagosSvc.getFormaPagoCodigoByFase( 'False' ).toPromise();
        this.formaPagoArrayConstruccion = await this.registrarPagosSvc.getFormaPagoCodigoByFase( 'True' ).toPromise();
        if (this.contrato.plazoFase1PreDias !== undefined) {
            this.tieneFase1 = true;
        }

        if ( this.contrato.solicitudPago.length > 1 ) {
            const solicitudPago = this.contrato.solicitudPago[0];
            this.solicitudPagoCargarFormaPago = solicitudPago.solicitudPagoCargarFormaPago[0];
            this.solicitudPagoCargarFormaPagoId = this.solicitudPagoCargarFormaPago.solicitudPagoCargarFormaPagoId;

            // Get values seleccionados
            if (this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo !== undefined) {
                this.estaEditando = true;
                this.addressForm.markAllAsTouched();
                const formaPreConstruccionSeleccionada = this.formaPagoArrayPreconstruccion.find(forma => forma.codigo === this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo);
                this.addressForm.get('formaPagoPreconstruccion').setValue(formaPreConstruccionSeleccionada !== undefined ? formaPreConstruccionSeleccionada : null);
                if (this.solicitudPagoCargarFormaPago.registroCompleto === true) {
                    this.addressForm.get('formaPagoPreconstruccion').disable();
                }
            }

            if (this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo !== undefined) {
                const formaConstruccionSeleccionada = this.formaPagoArrayConstruccion.find(forma => forma.codigo === this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo);
                this.addressForm.get('formaPagoConstruccion').setValue(formaConstruccionSeleccionada !== undefined ? formaConstruccionSeleccionada : null);
                if (this.solicitudPagoCargarFormaPago.registroCompleto === true) {
                    this.addressForm.get('formaPagoConstruccion').disable();
                }
            }
        } else {
            if (this.contrato.solicitudPagoOnly !== undefined) {

                this.solicitudPagoId = this.contrato.solicitudPagoOnly.solicitudPagoId;
                this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                this.solicitudPagoCargarFormaPagoId = this.solicitudPagoCargarFormaPago.solicitudPagoCargarFormaPagoId;
                // Get values seleccionados
                if (this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo !== undefined) {
                    this.estaEditando = true;
                    this.addressForm.markAllAsTouched();
                    const formaPreConstruccionSeleccionada = this.formaPagoArrayPreconstruccion.find(forma => forma.codigo === this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo);
                    this.addressForm.get('formaPagoPreconstruccion').setValue(formaPreConstruccionSeleccionada !== undefined ? formaPreConstruccionSeleccionada : null);
                    if (this.solicitudPagoCargarFormaPago.registroCompleto === true) {
                        this.addressForm.get('formaPagoPreconstruccion').disable();
                    }
                }

                if (this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo !== undefined) {
                    const formaConstruccionSeleccionada = this.formaPagoArrayConstruccion.find(forma => forma.codigo === this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo);

                    this.addressForm.get('formaPagoConstruccion').setValue(formaConstruccionSeleccionada !== undefined ? formaConstruccionSeleccionada : null);
                    if (this.solicitudPagoCargarFormaPago.registroCompleto === true) {
                        this.addressForm.get('formaPagoConstruccion').disable();
                    }
                }
            }
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
        const pSolicitudPago = {
            solicitudPagoId: this.solicitudPagoId,
            tipoSolicitudCodigo: this.tipoSolicitud,
            contratoId: this.contrato.contratoId,
            solicitudPagoCargarFormaPago: [
                {
                    solicitudPagoId: this.solicitudPagoId,
                    solicitudPagoCargarFormaPagoId: this.solicitudPagoCargarFormaPagoId,
                    fasePreConstruccionFormaPagoCodigo: this.addressForm.get('formaPagoPreconstruccion').value !== null ? this.addressForm.get('formaPagoPreconstruccion').value.codigo : null,
                    faseConstruccionFormaPagoCodigo: this.addressForm.get('formaPagoConstruccion').value !== null ? this.addressForm.get('formaPagoConstruccion').value.codigo : null,
                    tieneFase1: this.tieneFase1
                }
            ]
        }
        this.registrarPagosSvc.createEditNewPayment(pSolicitudPago)
            .subscribe(
                response => {
                    this.openDialog('', `<b>${response.message}</b>`);
                    if (this.esUnEditar === false) {
                        this.routes.navigateByUrl('/', { skipLocationChange: true }).then(
                            () => this.routes.navigate( [ '/registrarValidarRequisitosPago/verDetalleEditar', response.data.contratoId, response.data.solicitudPagoId ] )
                        );
                    }
                    if (this.esUnEditar === true) {
                        this.registrarPagosSvc.getValidateSolicitudPagoId(this.solicitudPagoId)
                            .subscribe(
                                () => {
                                    this.routes.navigateByUrl('/', { skipLocationChange: true }).then(
                                        () => this.routes.navigate( [ '/registrarValidarRequisitosPago/verDetalleEditar', response.data.contratoId, response.data.solicitudPagoId ] )
                                    );
                                }
                            );
                    }
                },
                err => this.openDialog('', `<b>${err.message}</b>`)
            );
    }

}
