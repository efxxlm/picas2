import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-costo-variable-gog',
  templateUrl: './form-costo-variable-gog.component.html',
  styleUrls: ['./form-costo-variable-gog.component.scss']
})
export class FormCostoVariableGogComponent implements OnInit {
  tipoAportanteArray = [
    { name: 'Entidad Territorial', value: '1' }
  ];
  nombreAportanteArray = [
    { name: 'Alcaldía de Susacón', value: '1' }
  ];
  fuenteRecursosArray = [
    { name: 'Contingencias', value: '1' }
  ];
  addressForm = this.fb.group({
    tipoAportante: [null, Validators.required],
    nombreAportante: [null, Validators.required],
    fuenteRecursos: [null, Validators.required],
    valorDescuento: [null, Validators.required]
  });
  
  constructor( private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
