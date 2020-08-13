import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-definir-fuentes-y-usos',
  templateUrl: './definir-fuentes-y-usos.component.html',
  styleUrls: ['./definir-fuentes-y-usos.component.scss']
})

export class DefinirFuentesYUsosComponent implements OnInit {
  addressForm = this.fb.group({
    valorAportanteProyecto: [null, Validators.compose([
      Validators.required, Validators.minLength(4), Validators.maxLength(20)]),
    ],
    componentes: this.fb.array([
      this.fb.group({
        fase: [null, Validators.required],
        componente: [null, Validators.required],
        usos: this.fb.array([
          this.fb.group({
            uso: [null, Validators.compose([
              Validators.required, Validators.minLength(4), Validators.maxLength(20)])],
            valorUso: [null, Validators.compose([
              Validators.required, Validators.minLength(4), Validators.maxLength(20)])
            ]
          })
        ])
      })
    ])
  });

  componentesSelect = [
    {
      value: 1,
      name: 'Obra complementaria'
    }
  ];

  usosSelect = [
    {
      value: 1,
      name: 'Obra'
    }
  ];

  get componentes() {
    return this.addressForm.get('componentes') as FormArray;
  }
  get usos() {
    return this.addressForm.get('usos') as FormArray;
  }

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  addUso() {
    this.usos.push(this.fb.control(null, Validators.compose([
      Validators.required, Validators.minLength(4), Validators.maxLength(20)])
    ));
  }


  addComponent() {
    this.componentes.push(this.createComponent());
  }

  createComponent(): FormGroup {
    return this.fb.group({
      fase: [null, Validators.required],
      componente: [null, Validators.required],
      usos: this.fb.array([
        this.fb.group({
          uso: [null, Validators.compose([
            Validators.required, Validators.minLength(4), Validators.maxLength(20)])],
          valorUso: [null, Validators.compose([
            Validators.required, Validators.minLength(4), Validators.maxLength(20)])
          ]
        })
      ])
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }
}
