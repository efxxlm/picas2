import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-dialog-cargar-programacion',
  templateUrl: './dialog-cargar-programacion.component.html',
  styleUrls: ['./dialog-cargar-programacion.component.scss']
})
export class DialogCargarProgramacionComponent implements OnInit {

  archivo: string;
  boton: string= 'Cargar';
  formCargarProgramacion: FormGroup;
  idProject: string;
  esFlujoInversion: boolean;

  constructor ( private fb: FormBuilder,
                private dialog: MatDialog,
                @Inject(MAT_DIALOG_DATA) public data,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService )
  {
    this.crearFormulario();
    this.esFlujoInversion = data.esFlujoInversion;
    console.log( this.esFlujoInversion );
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

  openDialogResponse (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
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

    confirmarDialog.afterClosed()
    .subscribe( response => {
      if ( response === true ) {
        if ( this.esFlujoInversion ) {
          this.faseUnoConstruccionSvc.transferMassiveLoadInvestmentFlow( this.idProject )
            .subscribe(
              response => this.openDialogResponse( '', response.message ),
              err => this.openDialogResponse( '', err.message )
            )
        } else {
          this.faseUnoConstruccionSvc.transferMassiveLoadProgramming( this.idProject )
          .subscribe(
            response => this.openDialogResponse( '', response.message ),
            err => this.openDialogResponse( '', err.message )
          )
        }
      }
    } );
  };

  guardar () {
    //console.log( this.formCargarProgramacion );
    const inputNode: any = document.getElementById('file');
    if ( inputNode.files[0] === undefined ) {
      return;
    };
    console.log( inputNode.files[0] );
    if ( this.esFlujoInversion ) {
      this.faseUnoConstruccionSvc.uploadFileToValidateInvestmentFlow( 28, inputNode.files[0] )
      .subscribe( 
        ( response: any ) => {
          console.log( response );
          if ( response.data.cantidadDeRegistros === response.data.cantidadDeRegistrosValidos ) {
            this.idProject = response.data.llaveConsulta;
            this.openDialogConfirmar(
              'Validación de registro',
              ` <br>Número de registros en el archivo: <b>${ response.data.cantidadDeRegistros }</b><br>
              Número de registros validos: <b>${ response.data.cantidadDeRegistrosValidos }</b><br>
              Número de registros inválidos: <b>${ response.data.cantidadDeRegistrosInvalidos }</b><br><br>
              <b>¿Desea realizar el cargue de la programación de obra?</b>
              `
            )
          } else {
            this.openDialog(
              'Validación de registro',
              ` <br>Número de registros en el archivo: <b>${ response.data.cantidadDeRegistros }</b><br>
                Número de registros validos: <b>${ response.data.cantidadDeRegistrosValidos }</b><br>
                Número de registros inválidos: <b>${ response.data.cantidadDeRegistrosInvalidos }</b><br><br>
                <b>No se permite el cargue, ya que el archivo tiene registros inválidos. Ajuste el archivo y cargue de nuevo</b>
              `
            );
          }
        }
      )
    } else {
      this.faseUnoConstruccionSvc.uploadFileToValidateProgramming( 28, inputNode.files[0] )
      .subscribe( 
        ( response: any ) => {
          console.log( response );
          if ( response.data.cantidadDeRegistros === response.data.cantidadDeRegistrosValidos ) {
            this.idProject = response.data.llaveConsulta;
            this.openDialogConfirmar(
              'Validación de registro',
              ` <br>Número de registros en el archivo: <b>${ response.data.cantidadDeRegistros }</b><br>
              Número de registros validos: <b>${ response.data.cantidadDeRegistrosValidos }</b><br>
              Número de registros inválidos: <b>${ response.data.cantidadDeRegistrosInvalidos }</b><br><br>
              <b>¿Desea realizar el cargue de la programación de obra?</b>
              `
            )
          } else {
            this.openDialog(
              'Validación de registro',
              ` <br>Número de registros en el archivo: <b>${ response.data.cantidadDeRegistros }</b><br>
                Número de registros validos: <b>${ response.data.cantidadDeRegistrosValidos }</b><br>
                Número de registros inválidos: <b>${ response.data.cantidadDeRegistrosInvalidos }</b><br><br>
                <b>No se permite el cargue, ya que el archivo tiene registros inválidos. Ajuste el archivo y cargue de nuevo</b>
              `
            );
          }
        }
      )
    }
  };

};