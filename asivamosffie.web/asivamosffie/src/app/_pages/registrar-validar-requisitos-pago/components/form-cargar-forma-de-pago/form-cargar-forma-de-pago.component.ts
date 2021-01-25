import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-cargar-forma-de-pago',
  templateUrl: './form-cargar-forma-de-pago.component.html',
  styleUrls: ['./form-cargar-forma-de-pago.component.scss']
})
export class FormCargarFormaDePagoComponent implements OnInit {

    @Input() contrato: any;
    @Input() tipoSolicitud: string;
    @Input() esUnEditar = false;
    addressForm = this.fb.group({
      formaPagoPreconstruccion: [null, Validators.required],
      formaPagoConstruccion: [null, Validators.required]
    });
    formaDePago: any;
    formaPagoArray: Dominio[] = [];
    solicitudPagoId = 0;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoCargarFormaPagoId = 0;
    tieneFase1 = false;

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
    }

    ngOnInit(): void {
        this.commonSvc.formasDePago()
            .subscribe( response => {
                this.formaPagoArray = response;

                if ( this.contrato.plazoFase1PreDias !== undefined ) {
                    this.tieneFase1 = true;
                }
                if ( this.contrato.solicitudPagoOnly !== undefined ) {
                    this.solicitudPagoId = this.contrato.solicitudPagoOnly.solicitudPagoId;
                    this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                    this.solicitudPagoCargarFormaPagoId = this.solicitudPagoCargarFormaPago.solicitudPagoCargarFormaPagoId;
                    // Get values seleccionados
                    if ( this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo !== undefined ) {
                        const formaPreConstruccionSeleccionada = this.formaPagoArray.filter( forma => forma.codigo === this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo );
                        this.addressForm.get( 'formaPagoPreconstruccion' ).setValue( formaPreConstruccionSeleccionada.length > 0 ? formaPreConstruccionSeleccionada[0] : null );
                        if ( this.solicitudPagoCargarFormaPago.registroCompleto === true ) {
                            this.addressForm.get( 'formaPagoPreconstruccion' ).disable();
                        }
                    }

                    if ( this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo !== undefined ) {
                        const formaConstruccionSeleccionada = this.formaPagoArray.filter( forma => forma.codigo === this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo );
                        this.addressForm.get( 'formaPagoConstruccion' ).setValue( formaConstruccionSeleccionada.length > 0 ? formaConstruccionSeleccionada[0] : null );
                        if ( this.solicitudPagoCargarFormaPago.registroCompleto === true ) {
                            this.addressForm.get( 'formaPagoConstruccion' ).disable();
                        }
                    }
                }
            } );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
      const pSolicitudPago = {
        solicitudPagoId: this.solicitudPagoId,
        tipoSolicitudCodigo: this.tipoSolicitud,
        contratoId: this.contrato.contratoId,
        solicitudPagoCargarFormaPago: [
            {
                solicitudPagoId: this.solicitudPagoId,
                solicitudPagoCargarFormaPagoId: this.solicitudPagoCargarFormaPagoId,
                fasePreConstruccionFormaPagoCodigo: this.addressForm.get( 'formaPagoPreconstruccion' ).value !== null ?  this.addressForm.get( 'formaPagoPreconstruccion' ).value.codigo : '',
                faseConstruccionFormaPagoCodigo: this.addressForm.get( 'formaPagoConstruccion' ).value !== null ?  this.addressForm.get( 'formaPagoConstruccion' ).value.codigo : '',
                tieneFase1: this.tieneFase1
            }
        ]
      }
      console.log( pSolicitudPago );
      this.registrarPagosSvc.createEditNewPayment( pSolicitudPago )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                if ( this.esUnEditar === false ) {
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate( [ '/registrarValidarRequisitosPago' ] )
                    );
                }
                if ( this.esUnEditar === true ) {

                }
                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                    () => this.routes.navigate(
                        [
                            '/registrarValidarRequisitosPago/verDetalleEditar', this.contrato.contratoId, this.solicitudPagoId
                        ]
                    )
                );
            },
            err => this.openDialog( '', `<b>${ err.message }</b>` )
        );
    }

}
