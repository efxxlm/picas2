import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-datos-proponentes-seleccionados-invitacion-cerrada',
  templateUrl: './proponentes-seleccionados-invitacion-cerrada.component.html',
  styleUrls: ['./proponentes-seleccionados-invitacion-cerrada.component.scss']
})
export class FormDatosProponentesSeleccionadosInvitacionCerradaComponent {

  nombresProponentesList: string[] = ['Nathalia Aranda', 'Andres Montealegre', 'Ana Sandoval'];

  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2)])
    ],
    nombresProponentes: [null, Validators.required],
    tipoProponente: [null, Validators.required],
    nombre: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    numeroIdentificacon: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    representanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    cedulaRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(12)])
    ],
    departamentoRepresentanteLegal: [null, Validators.required],
    municipioRepresentanteLegal: [null, Validators.required],
    direccionPrincipalRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(100)])
    ],
    telefonoRepresentanteLegal: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    correoRepresentanteLegal: [null, [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(50),
      Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ]],
  });

  constructor(private fb: FormBuilder) {}

  onSubmit() {
    alert('Thanks!');
  }
}
