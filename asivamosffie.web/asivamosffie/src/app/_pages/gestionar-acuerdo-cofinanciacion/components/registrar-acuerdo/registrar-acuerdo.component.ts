import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-registrar-acuerdo',
  templateUrl: './registrar-acuerdo.component.html',
  styleUrls: ['./registrar-acuerdo.component.scss']
})
export class RegistrarAcuerdoComponent {
  addressForm = this.fb.group({
    vigenciaEstado: ['', Validators.required],
    numAportes: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
    tipoAportante: ['', Validators.required],
    nombreAportante: ['', Validators.required],
    cauntosDocumentosAportante: ['', [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(99)]],
  });

  vigenciaEstados = [2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024];
  selectTiposAportante = [
    { name: 'primero', value: 1 }, { name: 'segundo', value: 2 }, { name: 'tercero', value: 3 }
  ];
  nombresAportante = [
    { name: 'fundacion 1', value: 1 }, { name: 'fundacion 2', value: 2 }, { name: 'fundacion 3', value: 3 }
  ];

  constructor(private fb: FormBuilder) { }

  onSubmit() {
    if (this.addressForm.valid) {
      console.log(this.addressForm.value);
    }
  }
}
