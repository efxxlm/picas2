import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-soporte-solicitud',
  templateUrl: './form-soporte-solicitud.component.html',
  styleUrls: ['./form-soporte-solicitud.component.scss']
})
export class FormSoporteSolicitudComponent {

  estaEditando = false;

  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}
