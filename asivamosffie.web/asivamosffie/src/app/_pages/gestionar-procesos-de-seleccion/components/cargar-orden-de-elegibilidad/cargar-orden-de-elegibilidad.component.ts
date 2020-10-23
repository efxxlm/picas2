import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { RespuestaProyecto } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-cargar-orden-de-elegibilidad',
  templateUrl: './cargar-orden-de-elegibilidad.component.html',
  styleUrls: ['./cargar-orden-de-elegibilidad.component.scss']
})
export class CargarOrdenDeElegibilidadComponent {

  fileElegibilidad: FormControl;
  archivo: string;
  boton:string="Cargar";
  idProject: string;

  constructor(
                private procesoSeleccionService: ProcesoSeleccionService,
                public dialog: MatDialog,
                @Inject(MAT_DIALOG_DATA) public data: { 
                  procesoSeleccionId: number,
                },
             ) 
  {
    this.declararInputFile();
  }

    private declararInputFile() {
      this.fileElegibilidad = new FormControl('', [Validators.required]);
    }

    fileName() {
      const inputNode: any = document.getElementById('file');
      this.archivo = inputNode.files[0].name;
    }

    onSubmit(): void {
      this.boton="Aguarde un momento, estamos procesando el archivo";
      const inputNode: any = document.getElementById('file');    
      this.procesoSeleccionService.setValidateMassiveLoadElegibilidad(inputNode.files[0],this.data.procesoSeleccionId).subscribe(
        response => {
          let respuestaCargue:RespuestaProyecto=response.data;
          let strOpciones ="";
          if( respuestaCargue.cantidadDeRegistrosValidos == respuestaCargue.cantidadDeRegistros ) {
            strOpciones="<br><b>¿Desea realizar el cargue de los proyectos validos?</b><br>";
            this.idProject=respuestaCargue.llaveConsulta;
            this.openDialogSiNo('Validación de registro', 
          "<br>Número de registros en el archivo: <b>"+respuestaCargue.cantidadDeRegistros+"</b><br>"+
          "Número de registros validos: <b>"+respuestaCargue.cantidadDeRegistrosValidos+"</b><br>"+
          "Número de registros inválidos: <b>"+respuestaCargue.cantidadDeRegistrosInvalidos+"</b><br>"+
          strOpciones
          );
          }
          else{
            this.openDialog('Validación de registro', 
            "<br>Número de registros en el archivo:<b>"+respuestaCargue.cantidadDeRegistros+"</b><br>"+
            "Número de registros validos: <b>"+respuestaCargue.cantidadDeRegistrosValidos+"</b><br>"+
            "Número de registros inválidos: <b>"+respuestaCargue.cantidadDeRegistrosInvalidos+"</b><br>"
            );
          }
            
        },
        error => {
          console.log(<any>error);
          let mensaje: string;
            if (error.error.message){
              mensaje = error.error.message;
            }else {
              mensaje = error.message;
            }
            this.openDialog('Error', mensaje);
            
          },
        () =>{
          this.boton="Cargar";
        });
  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
        
          location.reload();
          //this.router.navigate(["/cargarMasivamente"], {});
        
      });
    }
  }

  openDialogSiNo(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '35em',
      data: { modalTitle, modalText,siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.procesoSeleccionService.uploadMassiveLoadElegibilidad(this.idProject,this.data.procesoSeleccionId).subscribe(
          response => {
            this.openDialog('', response.message,response.code=="200");
          },
          error => {
            console.log(<any>error);
            let mensaje: string;
              if (error.error.message){
                mensaje = error.error.message;
              }else {
                mensaje = error.message;
              }
              this.openDialog('Error', mensaje);
              
            },
          () =>{
            this.boton="Cargar";
          }); 
      }           
    });
  }
}
