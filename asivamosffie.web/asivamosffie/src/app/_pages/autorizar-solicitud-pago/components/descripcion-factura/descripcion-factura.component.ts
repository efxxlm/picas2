import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-descripcion-factura',
  templateUrl: './descripcion-factura.component.html',
  styleUrls: ['./descripcion-factura.component.scss']
})
export class DescripcionFacturaComponent implements OnInit {

    @Input() contrato: any = undefined;
    solicitudPago: any = undefined;
    addressForm = this.fb.group(
        {
            esFactura: [ null, Validators.required ],
            numeroDocumento: [ null, Validators.required ],
            fechaDocumento: [ null, Validators.required ]
        }
    );

    constructor( private fb: FormBuilder ) { }

    ngOnInit(): void {
        this.solicitudPago = this.contrato.solicitudPagoOnly
        const solicitudPagoFactura = this.contrato.solicitudPagoOnly.solicitudPagoFactura[ 0 ]

        if ( this.solicitudPago.esFactura !== undefined ) {
            this.addressForm.get( 'esFactura' ).setValue( this.solicitudPago.esFactura )
        }

        if ( solicitudPagoFactura !== undefined ) {
            this.addressForm.get( 'numeroDocumento' ).setValue( solicitudPagoFactura.numero )
            this.addressForm.get( 'fechaDocumento' ).setValue( new Date( solicitudPagoFactura.fecha ) )
        }
    }

}
