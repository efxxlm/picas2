import { Component, Inject, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-devolver-solicitud',
  templateUrl: './dialog-devolver-solicitud.component.html',
  styleUrls: ['./dialog-devolver-solicitud.component.scss']
})
export class DialogDevolverSolicitudComponent implements OnInit {

    addressForm = this.fb.group({
      fechaRadicacionSAC: [null, Validators.required],
      numeroRadicacionSAC: [null, Validators.required]
    });

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private matDialogRef: MatDialogRef<DialogDevolverSolicitudComponent>,
        @Inject(MAT_DIALOG_DATA) public registro: any )
    { }

    ngOnInit(): void {
        console.log( this.registro );
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
      console.log(this.addressForm.value);
      // this.openDialog('', 'La información ha sido guardada exitosamente.');
    }

}
