import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-solicitar-contratacion',
  templateUrl: './form-solicitar-contratacion.component.html',
  styleUrls: ['./form-solicitar-contratacion.component.scss']
})
export class FormSolicitarContratacionComponent {

  verBusqueda = false;

  addressForm = this.fb.group({
    esMultiple: ['free', Validators.required],
    tipoInterventor: [null],
    llaveMEN: [null],
    region: [null],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null],
    sede: [null],
  });

  selectTipoInterventor = [{
    name: 'Mejoramiento',
    value: 1
  }];

  selectRegion = [{
    name: 'Mejoramiento',
    value: 1
  }];

  selectDepartamento = [{
    name: 'Mejoramiento',
    value: 1
  }];

  selectMunicipio = [{
    name: 'Mejoramiento',
    value: 1
  }];

  selectinstitucionEducativa = [{
    name: 'Mejoramiento',
    value: 1
  }];

  selectSede = [{
    name: 'Mejoramiento',
    value: 1
  }];

  constructor(private fb: FormBuilder) {}

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
