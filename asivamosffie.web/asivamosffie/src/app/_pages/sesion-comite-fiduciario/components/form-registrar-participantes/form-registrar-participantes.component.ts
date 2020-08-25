import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-form-registrar-participantes',
  templateUrl: './form-registrar-participantes.component.html',
  styleUrls: ['./form-registrar-participantes.component.scss']
})
export class FormRegistrarParticipantesComponent {
  addressForm = this.fb.group({
    miembrosParticipantes: [null, Validators.required],
    invitados: this.fb.array([
      this.fb.group({
        nombre: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(100)])
        ],
        cargo: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(50)])
        ],
        entidad: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(100)])
        ]
      })
    ])
  });

  hasUnitNumber = false;

  miembrosArray = [
    {name: 'Juan Lizcano Garcia', value: '1'},
    {name: 'Fernando jose Aldemar Rojas', value: '2'},
    {name: 'Felipe Otero Macias Betancur', value: '3'},
    {name: 'Paola Andrea Maldonado', value: '4'},
    {name: 'Gonzalo Díaz Mesa', value: '5'},
    {name: 'Maria Fernanda zamora', value: '6'},
    {name: 'Manuel Andres Merchan', value: '7'}
  ];

  constructor(private fb: FormBuilder) {}

  get invitados() {
    return this.addressForm.get('invitados') as FormArray;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaInvitado() {
    this.invitados.push(this.crearInvitado());
  }

  crearInvitado() {
    return this.fb.group({
      nombre: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      cargo: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(50)])
      ],
      entidad: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ]
    });
  }

  onSubmit() {
    alert('Thanks!');
  }
}
