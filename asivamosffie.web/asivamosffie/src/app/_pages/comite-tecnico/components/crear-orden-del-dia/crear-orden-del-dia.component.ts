import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

export interface SolicitudesContractuales {
  fecha: string;
  numero: string;
  solicitud: string;
}

const ELEMENT_DATA: SolicitudesContractuales[] = [
  {fecha: '23/06/2020', numero: 'SA0006', solicitud: 'Apertura de proceso de selección'},
  {fecha: '22/06/2020', numero: 'SC0005', solicitud: 'Evaluación de proceso de selección'},
  {fecha: '22/06/2020', numero: 'PI0004', solicitud: 'Contratación'},
];

@Component({
  selector: 'app-crear-orden-del-dia',
  templateUrl: './crear-orden-del-dia.component.html',
  styleUrls: ['./crear-orden-del-dia.component.scss']
})

export class CrearOrdenDelDiaComponent {
  addressForm = this.fb.group({
    tema: this.fb.array([
      this.fb.group({
        tema: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(100)])
        ],
        responsable: [null, Validators.required],
        tiempoIntervencion: [null, Validators.compose([
          Validators.required, Validators.minLength(1), Validators.maxLength(3)])
        ],
        url: [null, [
          Validators.required,
          Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
        ]]
      })
    ])
  });


  responsablesArray = [
    {name: 'reponsable 1', value: '1'},
    {name: 'reponsable 2', value: '2'},
    {name: 'reponsable 3', value: '3'}
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog
    ) {}

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get tema() {
    return this.addressForm.get('tema') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  crearTema() {
    return this.fb.group({
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        Validators.required,
        Validators.pattern('/^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/')
      ]],
    });
  }

  onSubmit() {
    if (this.addressForm.invalid) {
      this.openDialog('Falta registrar información', '');
    }
  }
}
