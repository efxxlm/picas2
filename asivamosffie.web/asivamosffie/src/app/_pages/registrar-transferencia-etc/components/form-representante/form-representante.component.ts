import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-representante',
  templateUrl: './form-representante.component.html',
  styleUrls: ['./form-representante.component.scss']
})
export class FormRepresentanteComponent implements OnInit {

  addressForm = this.fb.group({
    nombreApellidos: [null, Validators.required],
    cargo: [null, Validators.required],
    dependenciaETC: [null, Validators.required]
  });

  estaEditando = false;

  constructor(private fb: FormBuilder, public dialog: MatDialog) {}

  ngOnInit(): void {}

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
