import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogValidacionRegistroComponent } from '../dialog-validacion-registro/dialog-validacion-registro.component'
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';

@Component({
  selector: 'app-cargar-programacion',
  templateUrl: './cargar-programacion.component.html',
  styleUrls: ['./cargar-programacion.component.scss']
})
export class CargarProgramacionComponent {

  fileProgramacion: FormControl;
  archivo: string;
  boton: string = "Cargar";
  idProject: string;

  constructor(
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: {
      ajusteProgramacionInfo: any
    },
    private dialogRef: MatDialogRef<CargarProgramacionComponent>,
    private reprogramacionSvc: ReprogrammingService,

  ) {
    this.declararInputFile();
  }

  private declararInputFile() {
    this.fileProgramacion = new FormControl('', [Validators.required]);
  }

  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  }

  openObservaciones() {
    const dialogCargarProgramacion = this.dialog.open(DialogValidacionRegistroComponent, {
      width: '50em',
      // data: { }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

  onClose(): void {
    this.dialog.closeAll();
  }

  openDialogResponse (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
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
           this.reprogramacionSvc.transferMassiveLoadAdjustmentProgramming( this.idProject,
                                                                                this.data.ajusteProgramacionInfo.proyectoId,
                                                                                this.data.ajusteProgramacionInfo.contratoId )
             .subscribe(
               response => {
                 this.openDialogResponse( '', response.message );
                 this.dialogRef.close( { terminoCarga: true } );
               },
               err => this.openDialogResponse( '', err.message )
             )
         }
     });
  }

  onSubmit(): void {
      const inputNode: any = document.getElementById('file');
      if ( inputNode.files[0] === undefined ) {
        return;
      };
      console.log( inputNode.files[0], this.data );

        this.reprogramacionSvc.uploadFileToValidateAdjustmentProgramming( this.data.ajusteProgramacionInfo.ajusteProgramacionId,
                                                                               this.data.ajusteProgramacionInfo.contratacionProyectoId,
                                                                               this.data.ajusteProgramacionInfo.novedadContractualId,
                                                                               this.data.ajusteProgramacionInfo.contratoId,
                                                                               this.data.ajusteProgramacionInfo.proyectoId,
                                                                               inputNode.files[0]
                                                                              )
        .subscribe(
          ( response: any ) => {
            console.log( response );
            if ( response.data.cargaValida === true && response.data.cantidadDeRegistros === response.data.cantidadDeRegistrosValidos ) {
              this.idProject = response.data.llaveConsulta;
              this.openDialogConfirmar(
                'Validación de registro',
                ` <br>Número de registros en el archivo: <b>${ response.data.cantidadDeRegistros }</b><br>
                Número de registros válidos: <b>${ response.data.cantidadDeRegistrosValidos }</b><br>
                Número de registros inválidos: <b>${ response.data.cantidadDeRegistrosInvalidos }</b><br><br>
                <b>¿Desea realizar el cargue del flujo de inversión?</b>
                `
              )
            } else if (response.data.cargaValida === true) {
              this.openDialog(
                'Validación de registro',
                ` <br>Número de registros en el archivo: <b>${ response.data.cantidadDeRegistros }</b><br>
                  Número de registros válidos: <b>${ response.data.cantidadDeRegistrosValidos }</b><br>
                  Número de registros inválidos: <b>${ response.data.cantidadDeRegistrosInvalidos }</b><br><br>
                  <b>No se permite el cargue, ya que el archivo tiene registros inválidos. Ajuste el archivo y cargue de nuevo</b>
                `
              );
              this.dialogRef.close( { terminoCarga: true } );
            }else if (response.data.cargaValida === false) {
              this.openDialog(
                'Validación de registro',
                ` <br><b>${ response.data.mensaje }</b><br>`
              );
              this.dialogRef.close( { terminoCarga: true } );
            };
          }
        )
    };


  openDialog(modalTitle: string, modalText: string, redirect?: boolean) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {

        location.reload();
        //this.router.navigate(["/cargarMasivamente"], {});

      });
    }
  }

  openDialogSiNo(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '35em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}
