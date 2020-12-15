import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-datos-factura',
  templateUrl: './form-datos-factura.component.html',
  styleUrls: ['./form-datos-factura.component.scss']
})
export class FormDatosFacturaComponent implements OnInit {
  addressForm = this.fb.group({
    numeroFactura: [null, Validators.required],
    fechaFactura: [null, Validators.required]
  });
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  onSubmit() {

  }
}
