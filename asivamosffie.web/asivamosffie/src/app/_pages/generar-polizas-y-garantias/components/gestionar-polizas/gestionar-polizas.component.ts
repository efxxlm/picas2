import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-gestionar-polizas',
  templateUrl: './gestionar-polizas.component.html',
  styleUrls: ['./gestionar-polizas.component.scss']
})
export class GestionarPolizasComponent {
  addressForm = this.fb.group({
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(50)])
    ],
    numeroPoliza: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(20)])
    ],
    numeroCertificado: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(20)])
    ],
    fecha: [null, Validators.required],
    vigenciaPoliza: [null, Validators.required],
    vigenciaAmparo: [null, Validators.required],
    valorAmparo: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(20)])
    ],
  });

  states = [
    {name: 'Alabama', value: 'AL'},
    {name: 'Alaska', value: 'AK'},
    {name: 'American Samoa', value: 'AS'},
    {name: 'Arizona', value: 'AZ'},
    {name: 'Arkansas', value: 'AR'},
    {name: 'California', value: 'CA'},
    {name: 'Colorado', value: 'CO'}
  ];

  constructor(private fb: FormBuilder) {
    this.minDate = new Date();
  }

  minDate: Date;

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
