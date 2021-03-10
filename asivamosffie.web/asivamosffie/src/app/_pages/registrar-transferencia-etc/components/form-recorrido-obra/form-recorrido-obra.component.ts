import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-recorrido-obra',
  templateUrl: './form-recorrido-obra.component.html',
  styleUrls: ['./form-recorrido-obra.component.scss']
})
export class FormRecorridoObraComponent implements OnInit {
  addressForm = this.fb.group({
    fechaRecorrido: [null, Validators.required],
    cuantosRepresentantes: [null, Validators.compose([Validators.required, Validators.maxLength(2)])],
    fechaFirma: [null, Validators.required],
    urlActa: [null, Validators.required]
  });

  estaEditando = false;

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  constructor(private fb: FormBuilder, public dialog: MatDialog) {}

  ngOnInit(): void {}

  arrayOne(n: number): any[] {
    return Array(n);
  }

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
