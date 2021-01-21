import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-detallar-solicitud-novedad',
  templateUrl: './form-detallar-solicitud-novedad.component.html',
  styleUrls: ['./form-detallar-solicitud-novedad.component.scss']
})
export class FormDetallarSolicitudNovedadComponent implements OnInit {

  addressForm = this.fb.group({
    nombreAportante: [null, Validators.compose([
      Validators.required, Validators.maxLength(50)])
    ],
    valorAportante: [null, Validators.compose([
      Validators.required, Validators.maxLength(20)])
    ],
    componente: this.fb.array([
      this.fb.group({
        fase: [null, Validators.required],
        componente: [null, Validators.required],
        uso: [null, Validators.required],
        valorUso: [null, Validators.required]
      })
    ]),
    valorUso: [null, Validators.compose([
      Validators.required, Validators.maxLength(20)])
    ]
  });

  get componenteField() {
    return this.addressForm.get('componente') as FormArray;
  }

  estaEditando = false;

  faseArray = [
    { name: 'Diseño', value: '1' },
    { name: 'Obra', value: '2' },
    { name: 'Mejoramiento', value: '3' },
    { name: 'Obra', value: '4' },
    { name: 'complementaria', value: '5' },
  ];
  componenteArray = [
    { name: 'Diseño', value: '1' },
    { name: 'Obra', value: '2' },
    { name: 'Mejoramiento', value: '3' },
    { name: 'Obra', value: '4' },
    { name: 'complementaria', value: '5' },
  ];
  usosArray = [
    { name: 'Diseño', value: '1' },
    { name: 'Obra', value: '2' },
    { name: 'Mejoramiento', value: '3' },
    { name: 'Diseño Obra Complementaria', value: '4' },
    { name: 'Obra Complementaria', value: '5' },
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  eliminarComponente(i: number) {
    const tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', i, tema);
  }

  agregarComponente() {
    this.componenteField.push(this.crearComponente());
  }

  private crearComponente() {
    return this.fb.group({
      fase: [null, Validators.required],
      componente: [null, Validators.required],
      uso: [null, Validators.required],
      valorUso: [null, Validators.required]
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number, grupo: any) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.deleteTema(e);
      }
    });
  }

  deleteTema(i: number) {
    const tema = this.componenteField.controls[i];

    console.log(tema);

    this.borrarArray(this.componenteField, i);
    this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>');
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
