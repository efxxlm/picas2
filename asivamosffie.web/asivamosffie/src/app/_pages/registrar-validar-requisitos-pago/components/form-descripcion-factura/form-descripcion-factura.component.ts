import { FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-descripcion-factura',
  templateUrl: './form-descripcion-factura.component.html',
  styleUrls: ['./form-descripcion-factura.component.scss']
})
export class FormDescripcionFacturaComponent implements OnInit {

    addressForm = this.fb.group(
        {
            esFactura: [ null, Validators.required ],
            numeroDocumento: [ null, Validators.required ],
            fechaDocumento: [ null, Validators.required ]
        }
    );

    constructor(
        private fb: FormBuilder )
    { }

    ngOnInit(): void {
    }

    guardar() {
        console.log( this.addressForm );
    }

}
