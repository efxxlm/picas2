import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-control-de-recursos',
  templateUrl: './control-de-recursos.component.html',
  styleUrls: ['./control-de-recursos.component.scss']
})
export class ControlDeRecursosComponent {
  addressForm = this.fb.group({
    nombreCuenta: [null, Validators.required],
    numeroCuenta: [null, Validators.required],
    rp: [null, Validators.required],
    vigencia: [null, Validators.required],
    fechaConsignacion: [null, Validators.required],
    valorConsignacion: [null, Validators.compose([
      Validators.required, Validators.minLength(4), Validators.maxLength(50)])
    ],
  });

  hasUnitNumber = false;

  NombresDeLaCuenta = [{
      name: 'Recursos corrientes',
      value: 1
    }];

  rpArray = [{
      name: 'Recursos corrientes',
      value: 1
    }];

  constructor(private fb: FormBuilder) {}

  onSubmit() {
    alert('Thanks!');
  }
}
