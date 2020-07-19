import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { min } from 'rxjs/operators';

@Component({
  selector: 'app-form-seleccion-proponente-a-invitar',
  templateUrl: './form-seleccion-proponente-a-invitar.component.html',
  styleUrls: ['./form-seleccion-proponente-a-invitar.component.scss']
})
export class FormSeleccionProponenteAInvitarComponent {
  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ],
    url: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ]
  });

  constructor(private fb: FormBuilder) {}

  onSubmit() {
    alert('Thanks!');
  }
}
