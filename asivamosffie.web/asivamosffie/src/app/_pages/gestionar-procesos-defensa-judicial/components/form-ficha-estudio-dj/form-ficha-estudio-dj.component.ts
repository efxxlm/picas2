import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-ficha-estudio-dj',
  templateUrl: './form-ficha-estudio-dj.component.html',
  styleUrls: ['./form-ficha-estudio-dj.component.scss']
})
export class FormFichaEstudioDjComponent implements OnInit {


 
  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  constructor(  private fb: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }
}
