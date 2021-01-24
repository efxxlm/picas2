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
    tiposDescuentoArray: Dominio[] = [];
    solicitudPagoFaseFactura: any[] = [];
    solicitudPagoFase: any;

    get descuentos() {
        return this.addressForm.get( 'descuentos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe( response => this.tiposDescuentoArray = response );
    }

    ngOnInit(): void {
        this.getDatosFactura();
    }

    getDatosFactura() {
        this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        this.addressForm.get( 'numeroDescuentos' ).valueChanges
            .subscribe(
                value => {
                    value = Number( value );
                    if ( value > 0 ) {
                        if ( this.descuentos.dirty === false ) {
                            this.descuentos.clear();
                            for ( let i = 0; i < value; i++ ) {
                                this.descuentos.push(
                                    this.fb.group(
                                        {
                                            solicitudPagoFaseDescuentoId: [ 0 ],
                                            solicitudPagoFaseFacturaId: [ 0 ],
                                            tipoDescuentoCodigo: [ null ],
                                            valorDescuento: [ null ]
                                        }
                                    )
                                );
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
        this.descuentos.controls.forEach( control => {
            if ( control.value.valorDescuento !== null ) {
                totalDescuentos += control.value.valorDescuento;
            }
        } );
        if ( totalDescuentos > 0 ) {
            this.addressForm.get( 'valorAPagarDespues' ).setValue( totalDescuentos );
        } else {
            this.addressForm.get( 'valorAPagarDespues' ).setValue( null );
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
                    solicitudPagoFaseDescuentoId: [ 0 ],
                    solicitudPagoFaseFacturaId: [ 0 ],
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
                            this.openDialog( '', '<b>Falta el servicio.</b>' );
                            // this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        }
                    }
                }
            );
    }

    onSubmit() {
        const solicitudPagoFaseFactura = [
            {
                solicitudPagoFaseFacturaId: 0,
                solicitudPagoFaseId: 0,
                fecha: '',
                valorFacturado: 15,
                numero: '',
                tieneDescuento: false,
                solicitudPagoFaseDescuento: [
                    {
                        solicitudPagoFaseDescuentoId: 0,
                        solicitudPagoFaseFacturaId: 0,
                        tipoDescuentoCodigo: 0,
                        valorDescuento: 0
                    }
                ]
            }
        ]
    }

}
