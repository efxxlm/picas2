import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-estrateg-pagos-gog',
  templateUrl: './form-estrateg-pagos-gog.component.html',
  styleUrls: ['./form-estrateg-pagos-gog.component.scss']
})
export class FormEstrategPagosGogComponent implements OnInit {

  estrategiaPagoArray = [
    { name: 'Transferencia electrónica', value: '1' }
  ];
  addressForm = this.fb.group({});
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      estrategiaPago: [null, Validators.required]
    })
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
