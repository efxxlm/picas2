import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-amortizacion-anticipo',
  templateUrl: './form-amortizacion-anticipo.component.html',
  styleUrls: ['./form-amortizacion-anticipo.component.scss']
})
export class FormAmortizacionAnticipoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() contrato: any;
    solicitudPagoFase: any;
    solicitudPagoFaseAmortizacionId = 0;
    valorTotalDelContrato = 0;
    addressForm = this.fb.group({
      porcentajeAmortizacion: [null, Validators.required],
      valorAmortizacion: [ { value: null, disabled: true } , Validators.required]
    });

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.addressForm.get( 'porcentajeAmortizacion' ).valueChanges
            .subscribe(
                value => {
                    if ( this.valorTotalDelContrato > 0 ) {
                        const exampleValue = this.valorTotalDelContrato * 0.20;
                        const porcentajeCalculo = value / 100;
                        const valorAmortizacion = exampleValue * porcentajeCalculo;
                        this.addressForm.get( 'valorAmortizacion' ).setValue( valorAmortizacion );
                    }
                }
            );
    }

    ngOnInit(): void {
        if ( this.contrato.contratacion.disponibilidadPresupuestal.length > 0 ) {
            this.contrato.contratacion.disponibilidadPresupuestal.forEach( ddp => this.valorTotalDelContrato += ddp.valorSolicitud );
        }
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        if ( this.solicitudPagoFase.solicitudPagoFaseAmortizacion.length > 0 ) {
            const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0]
            this.solicitudPagoFaseAmortizacionId = solicitudPagoFaseAmortizacion.solicitudPagoFaseAmortizacionId;
            this.addressForm.setValue(
                {
                    porcentajeAmortizacion: solicitudPagoFaseAmortizacion.porcentajeAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.porcentajeAmortizacion : null,
                    valorAmortizacion: solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.valorAmortizacion : null
                }
            );
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

    numberValidate( value: any ) {
        if ( value > 100 ) {
            this.addressForm.get( 'porcentajeAmortizacion' ).setValue( 100 );
        }
        if ( value < 0 ) {
            this.addressForm.get( 'porcentajeAmortizacion' ).setValue( 0 );
        }
    }

    onSubmit() {
        const solicitudPagoFaseAmortizacion = [
            {
                solicitudPagoFaseAmortizacionId: this.solicitudPagoFaseAmortizacionId,
                solicitudPagoFaseId: this.solicitudPagoFase.solicitudPagoFaseId,
                porcentajeAmortizacion: this.addressForm.get( 'porcentajeAmortizacion' ).value,
                valorAmortizacion: this.addressForm.get( 'valorAmortizacion' ).value
            }
        ];
        this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseAmortizacion = solicitudPagoFaseAmortizacion;
        this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
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
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
