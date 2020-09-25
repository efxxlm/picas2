import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-cargar-acta-suscrita-acta-ini-f-i-prc',
  templateUrl: './cargar-acta-suscrita-acta-ini-f-i-prc.component.html',
  styleUrls: ['./cargar-acta-suscrita-acta-ini-f-i-prc.component.scss']
})
export class CargarActaSuscritaActaIniFIPreconstruccionComponent implements OnInit {
  boton:string="Cargar";
  archivo: string;
  fileListaProyectos: FormControl;
  maxDate: Date;
  maxDate2: Date;

  constructor(private router: Router,public dialog: MatDialog, public matDialogRef: MatDialogRef<CargarActaSuscritaActaIniFIPreconstruccionComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { 
    this.declararInputFile();
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }

  ngOnInit(): void {
  }
  private declararInputFile() {
    this.fileListaProyectos = new FormControl('', [Validators.required]);
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  }
  cargarActa(){
    this.openDialog('La información ha sido guardada exitosamente.', "");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
    this.close();
  }
  close(){
    this.matDialogRef.close('cancel');
}
}
