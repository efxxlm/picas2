import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-acta-firmada-air',
  templateUrl: './dialog-cargar-acta-firmada-air.component.html',
  styleUrls: ['./dialog-cargar-acta-firmada-air.component.scss']
})
export class DialogCargarActaFirmadaAirComponent implements OnInit {

  addressForm = new FormGroup({
    documentoFile: new FormControl()
  });
  boton: string = "Cargar";
  archivo: string;
  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, public matDialogRef: MatDialogRef<DialogCargarActaFirmadaAirComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText }
    });
  }
  openDialogNoConfirmar (modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText}
    });
  };
  openDialogConfirmar (modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText, siNoBoton:true }
    });
  };
  fileName(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.addressForm.patchValue({
        documentoFile: file
      });
    }
  }
  onSubmit() {
    const pContrato = new FormData();
    let pFile = this.addressForm.get('documentoFile').value;
    pFile = pFile.name.split('.');
    pFile = pFile[pFile.length - 1];
    if (pFile === 'pdf') {
      console.log("pasó")
    } else {
      this.openDialog('', '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .pdf</b>');
      return;
    }
  }
  close() {
    this.matDialogRef.close('aceptado');
  }
}
