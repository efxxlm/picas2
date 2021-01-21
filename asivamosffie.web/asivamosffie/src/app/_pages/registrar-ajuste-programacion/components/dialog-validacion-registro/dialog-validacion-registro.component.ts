import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-validacion-registro',
  templateUrl: './dialog-validacion-registro.component.html',
  styleUrls: ['./dialog-validacion-registro.component.scss']
})
export class DialogValidacionRegistroComponent implements OnInit {

  constructor(
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
  }

  openDialogGuardar(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onClose(): void {
    this.dialog.closeAll();
  }

  guardar() {
    this.onClose();
    this.openDialogGuardar('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}
