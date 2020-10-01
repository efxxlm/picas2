import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-programacion',
  templateUrl: './dialog-cargar-programacion.component.html',
  styleUrls: ['./dialog-cargar-programacion.component.scss']
})
export class DialogCargarProgramacionComponent implements OnInit {

  archivo: string;
  boton: string= 'Cargar';
  formCargarProgramacion: FormGroup;

  constructor ( private fb: FormBuilder,
                private dialog: MatDialog ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
  };

  crearFormulario () {
    this.formCargarProgramacion = this.fb.group({
      fileCargarProgramacion: [ null, Validators.required ]
    });
  };

  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  };

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText }
    });
  };

  openDialogConfirmar (modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText, siNoBoton:true }
    });

    confirmarDialog.afterClosed().subscribe( console.log );
  };

  guardar () {
    console.log( this.formCargarProgramacion );
    //this.openDialog(
    //  'Validación de registro',
    //  ` <br>Número de registros en el archivo:<b>${ 5 }</b><br>
    //    Número de registros validos: <b>${ 3 }</b><br>
    //    Número de registros inválidos: <b>${ 2 }</b><br><br>
    //    <b>No se permite el cargue, ya que el archivo tiene registros inválidos. Ajuste el archivo y cargue de nuevo</b>
    //  `
    //);
    this.openDialogConfirmar(
      'Validación de registro',
      ` <br>Número de registros en el archivo:<b>${ 5 }</b><br>
      Número de registros validos: <b>${ 5 }</b><br>
      Número de registros inválidos: <b>${ 0 }</b><br><br>
      <b>¿Desea realizar el cargue de la programación de obra?</b>
      `
    )
  };

};