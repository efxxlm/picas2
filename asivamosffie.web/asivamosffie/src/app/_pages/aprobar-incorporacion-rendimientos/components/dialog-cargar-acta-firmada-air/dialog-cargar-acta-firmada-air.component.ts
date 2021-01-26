import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-acta-firmada-air',
  templateUrl: './dialog-cargar-acta-firmada-air.component.html',
  styleUrls: ['./dialog-cargar-acta-firmada-air.component.scss']
})
export class DialogCargarActaFirmadaAirComponent implements OnInit {

  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  boton: string = "Cargar";
  archivo: string;
  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, public matDialogRef: MatDialogRef<DialogCargarActaFirmadaAirComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

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
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
  close() {
    this.matDialogRef.close('aceptado');
  }
}
