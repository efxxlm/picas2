import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-gestionar-fuentes',
  templateUrl: './form-gestionar-fuentes.component.html',
  styleUrls: ['./form-gestionar-fuentes.component.scss']
})
export class FormGestionarFuentesComponent implements OnInit {
  addressForm = this.fb.group({
    fuentes: this.fb.array([
      this.fb.group({
        fuente: [null, Validators.required],
        saldoActual: [null, Validators.required],
        valorSolicitado: [null, Validators.compose([
          Validators.required, Validators.minLength(1), Validators.maxLength(20)])
        ],
        nuevoSaldo: [null, Validators.required]
      })
    ])
  });

  fuentesArray = [
    { name: 'Alabama', value: 'AL' },
    { name: 'Alaska', value: 'AK' },
    { name: 'American Samoa', value: 'AS' },
    { name: 'Arizona', value: 'AZ' },
    { name: 'Arkansas', value: 'AR' },
    { name: 'California', value: 'CA' }
  ];

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void { }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get fuentes() {
    return this.addressForm.get('fuentes') as FormArray;
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

  agregaFuente() {
    this.fuentes.push(this.crearFuente());
  }

  crearFuente() {
    return this.fb.group({
      fuente: [null, Validators.required],
      saldoActual: [null, Validators.required],
      valorSolicitado: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(20)])
      ],
      nuevoSaldo: [null, Validators.required],
    });
  }

  onSubmit() {
    alert('Thanks!');
  }
}
