import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-documento-de-apropiacion',
  templateUrl: './form-documento-de-apropiacion.component.html',
  styleUrls: ['./form-documento-de-apropiacion.component.scss']
})
export class FormDocumentoDeApropiacionComponent {
  addressForm = this.fb.group({
    vigenciaAportante: [null, Validators.required],
    valorIndicadoEnElDocumento: [null, Validators.compose([
      Validators.required, Validators.minLength(4), Validators.maxLength(20)])
    ],
    tipoDocumento: [null, Validators.required],
    numeroDocumento: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    fechaDocumento: [null, Validators.required],
  });

  valorTotalAcuerdo = 85000000;

  vigenciasAportante = [2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024];
  tiposDocumento = [
    { name: 'Resolución 1', value: 1 }, { name: 'Resolución 2', value: 2 }, { name: 'Resolución 3', value: 3 }
  ];

  constructor(private fb: FormBuilder) {}

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
