import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-recibo-a-satisfaccion',
  templateUrl: './form-recibo-a-satisfaccion.component.html',
  styleUrls: ['./form-recibo-a-satisfaccion.component.scss'],
})
export class FormReciboASatisfaccionComponent implements OnInit {

  estaEditando = false;

  addressForm = this.fb.group({
    URLActaRecibo: [null, Validators.required],
    fechaSuscripcion: [null, Validators.required],
  });

  constructor(private fb: FormBuilder, public dialog: MatDialog) {}

  ngOnInit(): void {}

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText },
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}
