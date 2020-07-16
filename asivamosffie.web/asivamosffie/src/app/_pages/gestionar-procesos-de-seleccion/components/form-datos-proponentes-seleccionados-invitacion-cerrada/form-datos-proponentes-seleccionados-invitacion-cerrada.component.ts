import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados-invitacion-cerrada',
  templateUrl: './form-datos-proponentes-seleccionados-invitacion-cerrada.component.html',
  styleUrls: ['./form-datos-proponentes-seleccionados-invitacion-cerrada.component.scss']
})
export class FormDatosProponentesSeleccionadosInvitacionCerradaComponent {
  addressForm = this.fb.group({
    
  });

  constructor(private fb: FormBuilder) {}

  onSubmit() {
    alert('Thanks!');
  }
}
