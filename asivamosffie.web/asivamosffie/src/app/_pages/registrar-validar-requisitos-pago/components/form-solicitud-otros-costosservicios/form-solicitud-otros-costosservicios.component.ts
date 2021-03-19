import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';

@Component({
  selector: 'app-form-solicitud-otros-costosservicios',
  templateUrl: './form-solicitud-otros-costosservicios.component.html',
  styleUrls: ['./form-solicitud-otros-costosservicios.component.scss']
})
export class FormSolicitudOtrosCostosserviciosComponent implements OnInit {

    @Input() tipoSolicitud: string;
    @Input() solicitudPago: any;
    @Input() esUnEditar = false;
    @Input() contrato: any;
    @Input() esVerDetalle = false;
    addressForm = this.fb.group({
      numeroContrato: [null, Validators.required],
      numeroRadicadoSAC: [null, Validators.required],
      numeroFactura: [null, Validators.required],
      valorFacturado: [null, Validators.required],
      tipoPago: [null, Validators.required],
    });
    contratoId = 0;
    solicitudPagoId = 0;
    contratosArray = [];
    tipoPagoArray: Dominio[] = [];
    solicitudPagosOtrosCostosServiciosId = 0;
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private routes: Router,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
    }

    ngOnInit(): void {
        this.commonSvc.tiposDePagoExpensas()
            .subscribe( response => {
                this.tipoPagoArray = response;
                if ( this.solicitudPago !== undefined ) {
                    this.estaEditando = true;
                    this.addressForm.markAllAsTouched();
                    this.solicitudPagoId = this.solicitudPago.solicitudPagoId;
                    this.contratoId = this.contrato.contratoId;
                    const solicitudPagoOtrosCostosServicios = this.solicitudPago.solicitudPagoOtrosCostosServicios[0];
                    this.solicitudPagosOtrosCostosServiciosId = solicitudPagoOtrosCostosServicios.solicitudPagoOtrosCostosServiciosId;
                    this.addressForm.get( 'numeroContrato' ).setValue( this.contrato.numeroContrato );
                    this.addressForm.setValue(
                        {
                            numeroContrato: this.contrato.numeroContrato,
                            numeroRadicadoSAC: solicitudPagoOtrosCostosServicios.numeroRadicadoSac !== undefined ? solicitudPagoOtrosCostosServicios.numeroRadicadoSac : null,
                            numeroFactura: solicitudPagoOtrosCostosServicios.numeroFactura !== undefined ? solicitudPagoOtrosCostosServicios.numeroFactura : null,
                            valorFacturado: solicitudPagoOtrosCostosServicios.valorFacturado !== undefined ? solicitudPagoOtrosCostosServicios.valorFacturado : null,
                            tipoPago: solicitudPagoOtrosCostosServicios.tipoPagoCodigo !== undefined ? this.tipoPagoArray.filter( tipoPago => tipoPago.codigo === solicitudPagoOtrosCostosServicios.tipoPagoCodigo )[0] : null
                        }
                    );
                }
            } );
    }

    seleccionAutocomplete( contrato: any ){
        this.contratoId = contrato.contratoId;
    }

    getContratos( trigger: MatAutocompleteTrigger ) {
        if ( this.addressForm.get( 'numeroContrato' ).value !== null ) {
            if ( this.addressForm.get( 'numeroContrato' ).value.length > 0 ) {
                this.registrarPagosSvc.getContratos( '', '', this.addressForm.get( 'numeroContrato' ).value )
                    .subscribe( response => {
                        this.contratosArray = response;
                        if ( response.length === 0 ) {
                            this.openDialog( '', '<b>No se encontraron contratos relacionados.</b>' );
                        } else {
                            trigger.openPanel();
                        }
                    } );
            }
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
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

        if ( this.contratoId === 0 ) {
            this.openDialog( '', '<b>Debe seleccionar un número de contrato valido.</b>' );
            return;
        }

        const pSolicitudPago = {
            solicitudPagoId: this.solicitudPagoId,
            tipoSolicitudCodigo: this.tipoSolicitud,
            contratoId: this.contratoId,
            solicitudPagoOtrosCostosServicios: [
                {
                    solicitudPagoOtrosCostosServiciosId: this.solicitudPagosOtrosCostosServiciosId,
                    solicitudPagoId: this.solicitudPagoId,
                    numeroRadicadoSac: this.addressForm.get( 'numeroRadicadoSAC' ).value,
                    numeroFactura: this.addressForm.get( 'numeroFactura' ).value,
                    valorFacturado: this.addressForm.get( 'valorFacturado' ).value,
                    tipoPagoCodigo: this.addressForm.get( 'tipoPago' ).value !== null ? this.addressForm.get( 'tipoPago' ).value.codigo : this.addressForm.get( 'tipoPago' ).value
                }
            ]
        };
        this.registrarPagosSvc.createEditOtrosCostosServicios( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.esUnEditar === false ) {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate( [ '/registrarValidarRequisitosPago' ]
                            )
                        );
                    }
                    if ( this.esUnEditar === true ) {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate(
                                [
                                    '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPagoId
                                ]
                            )
                        );
                    }
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
