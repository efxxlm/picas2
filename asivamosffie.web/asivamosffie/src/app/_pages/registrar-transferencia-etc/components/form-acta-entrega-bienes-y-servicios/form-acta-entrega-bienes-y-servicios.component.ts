import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-form-acta-entrega-bienes-y-servicios',
  templateUrl: './form-acta-entrega-bienes-y-servicios.component.html',
  styleUrls: ['./form-acta-entrega-bienes-y-servicios.component.scss']
})
export class FormActaEntregaBienesYServiciosComponent implements OnInit {

  addressForm = this.fb.group({
    fechaFirma: [null, Validators.required],
    urlActa: [null, Validators.required]
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
