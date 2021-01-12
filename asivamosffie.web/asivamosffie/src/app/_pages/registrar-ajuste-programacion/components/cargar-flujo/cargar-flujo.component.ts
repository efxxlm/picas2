import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogValidacionRegistroComponent } from '../dialog-validacion-registro/dialog-validacion-registro.component'


@Component({
  selector: 'app-cargar-flujo',
  templateUrl: './cargar-flujo.component.html',
  styleUrls: ['./cargar-flujo.component.scss']
})
export class CargarFlujoComponent {

  fileProgramacion: FormControl;
  archivo: string;
  boton: string = "Cargar";
  idProject: string;

  constructor(
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: {
      procesoSeleccionId: number,
    },
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

  onSubmit(): void {
    // this.boton = "Aguarde un momento, estamos procesando el archivo";
    const inputNode: any = document.getElementById('file');
    console.log(inputNode);
    console.log(inputNode.value);
    this.onClose();
    this.openObservaciones();
  }

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
