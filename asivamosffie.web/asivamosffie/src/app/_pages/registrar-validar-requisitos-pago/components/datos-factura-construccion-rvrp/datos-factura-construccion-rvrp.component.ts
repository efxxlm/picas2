import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-datos-factura-construccion-rvrp',
  templateUrl: './datos-factura-construccion-rvrp.component.html',
  styleUrls: ['./datos-factura-construccion-rvrp.component.scss']
})
export class DatosFacturaConstruccionRvrpComponent implements OnInit {

    @Input() solicitudPago: any;
    addressForm = this.fb.group({
        numeroFactura: [null, Validators.required],
        fechaFactura: [null, Validators.required],
        aplicaDescuento: [null, Validators.required],
        numeroDescuentos: [ '' ],
        descuentos: this.fb.array( [] ),
        valorAPagarDespues: [null, Validators.required]
    });
    valorFacturado = 0;
    tiposDescuentoArray: Dominio[] = [];
    solicitudPagoFaseFacturaDescuento: any[] = [];
    solicitudPagoFaseFacturaId = 0;
    solicitudPagoFaseFactura: any;
    solicitudPagoFase: any;

    get descuentos() {
        return this.addressForm.get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe( response => this.tiposDescuentoArray = response );
    }

    ngOnInit(): void {
        this.getDatosFactura();
    }

    getDatosFactura() {
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
        if ( this.solicitudPagoFaseFactura !== undefined ) {
            this.solicitudPagoFaseFacturaId = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaId;
            this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento;
            this.addressForm.get( 'numeroFactura' ).setValue( this.solicitudPagoFaseFactura.numero !== undefined ? this.solicitudPagoFaseFactura.numero : null );
            this.addressForm.get( 'fechaFactura' ).setValue( this.solicitudPagoFaseFactura.fecha !== undefined ? new Date( this.solicitudPagoFaseFactura.fecha ) : null );
            this.addressForm.get( 'aplicaDescuento' ).setValue( this.solicitudPagoFaseFactura.tieneDescuento !== undefined ? this.solicitudPagoFaseFactura.tieneDescuento : null );
            this.addressForm.get( 'numeroDescuentos' ).setValue( `${ this.solicitudPagoFaseFacturaDescuento.length }` );
            this.addressForm.get( 'valorAPagarDespues' ).setValue( this.solicitudPagoFaseFactura.valorFacturadoConDescuento !== undefined ? this.solicitudPagoFaseFactura.valorFacturadoConDescuento : null );
            for ( const descuento of this.solicitudPagoFaseFacturaDescuento ) {
                this.descuentos.push(
                    this.fb.group(
                        {
                            solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                            solicitudPagoFaseFacturaId: [ descuento.solicitudPagoFaseFacturaId ],
                            tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                            valorDescuento: [ descuento.valorDescuento ]
                        }
                    )
                );
            }
        }
        for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
            this.valorFacturado += criterio.valorFacturado;
        }
        this.addressForm.get( 'numeroDescuentos' ).valueChanges
            .subscribe(
                value => {
                    value = Number( value );
                    if ( this.solicitudPagoFaseFactura !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0 ) {
                        if ( value > 0 ) {
                            this.descuentos.clear();
                            for ( const descuento of this.solicitudPagoFaseFacturaDescuento ) {
                                this.descuentos.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseFacturaDescuentoId: [ descuento.solicitudPagoFaseFacturaDescuentoId ],
                                            solicitudPagoFaseFacturaId: [ descuento.solicitudPagoFaseFacturaId ],
                                            tipoDescuentoCodigo: [ descuento.tipoDescuentoCodigo ],
                                            valorDescuento: [ descuento.valorDescuento ]
                                        }
                                    )
                                );
                            }
                        }
                    } else {
                        if ( value > 0 ) {
                            if ( this.descuentos.dirty === false ) {
                                this.descuentos.clear();
                                for ( let i = 0; i < value; i++ ) {
                                    this.descuentos.push(
                                        this.fb.group(
                                            {
                                                solicitudPagoFaseFacturaDescuentoId: [ 0 ],
                                                solicitudPagoFaseFacturaId: [ this.solicitudPagoFaseFacturaId ],
                                                tipoDescuentoCodigo: [ null ],
                                                valorDescuento: [ null ]
                                            }
                                        )
                                    );
                                }
                            }
                        }
                    }
                }
            );
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    validateNumber( value: any ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.addressForm.get( 'numeroDescuentos' ).setValue( '' );
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
          e.editor.deleteText(n, e.editor.getLength());
        }
    }

    totalPagoDescuentos() {
        let totalDescuentos = 0;
        let valorDespuesDescuentos = 0;
        this.descuentos.controls.forEach( control => {
            if ( control.value.valorDescuento !== null ) {
                totalDescuentos += control.value.valorDescuento;
            }
        } );
        if ( totalDescuentos > 0 ) {
            if ( totalDescuentos > this.valorFacturado ) {
                this.openDialog( '', '<b>El valor total de los descuentos es mayor al valor facturado.</b>' );
                this.addressForm.get( 'valorAPagarDespues' ).setValue( null );
                return;
            } else {
                valorDespuesDescuentos = this.valorFacturado - totalDescuentos;
                this.addressForm.get( 'valorAPagarDespues' ).setValue( valorDespuesDescuentos );
                return;
            }
        } else {
            this.addressForm.get( 'valorAPagarDespues' ).setValue( null );
            return;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    addDescuento() {
        this.descuentos.push(
            this.fb.group(
                {
                    solicitudPagoFaseFacturaDescuentoId: [ 0 ],
                    solicitudPagoFaseFacturaId: [ this.solicitudPagoFaseFacturaId ],
                    tipoDescuentoCodigo: [ null ],
                    valorDescuento: [ null ]
                }
            )
        );
    }

    deleteDescuento( index: number, descuentoId: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                response => {
                    if ( response === true ) {
                        if ( descuentoId === 0 ) {
                            this.descuentos.removeAt( index );
                            this.addressForm.get( 'numeroDescuentos' ).setValue( `${ this.descuentos.length }` );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        } else {
                            this.descuentos.removeAt( index );
                            this.addressForm.get( 'numeroDescuentos' ).setValue( `${ this.descuentos.length }` );
                            this.registrarPagosSvc.deleteSolicitudPagoFaseFacturaDescuento( descuentoId )
                                .subscribe(
                                    () => this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' ),
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                );
                        }
                    }
                }
            );
    }

    onSubmit() {
        const getSolicitudPagoFaseFacturaDescuento = () => {
            if ( this.descuentos.length > 0 ) {
                if ( this.addressForm.get( 'aplicaDescuento' ).value === true ) {
                    return this.descuentos.value;
                } else {
                    return [];
                }
            } else {
                return [];
            }
        }

        const solicitudPagoFaseFactura = [
            {
                solicitudPagoFaseFacturaId: this.solicitudPagoFaseFacturaId,
                solicitudPagoFaseId: this.solicitudPagoFase.solicitudPagoFaseId,
                fecha: new Date( this.addressForm.get( 'fechaFactura' ).value ).toISOString(),
                valorFacturado: this.valorFacturado,
                numero: this.addressForm.get( 'numeroFactura' ).value,
                tieneDescuento: this.addressForm.get( 'aplicaDescuento' ).value,
                valorFacturadoConDescuento: this.addressForm.get( 'valorAPagarDespues' ).value,
                solicitudPagoFaseFacturaDescuento: getSolicitudPagoFaseFacturaDescuento()
            }
        ]
        console.log( solicitudPagoFaseFactura );
        this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseFactura = solicitudPagoFaseFactura;
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
