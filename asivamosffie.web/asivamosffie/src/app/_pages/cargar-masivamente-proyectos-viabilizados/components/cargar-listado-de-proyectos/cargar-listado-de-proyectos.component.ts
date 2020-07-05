import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ProjectService, RespuestaProyecto } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-cargar-listado-de-proyectos',
  templateUrl: './cargar-listado-de-proyectos.component.html',
  styleUrls: ['./cargar-listado-de-proyectos.component.scss']
})
export class CargarListadoDeProyectosComponent implements OnInit {

  fileListaProyectos: FormControl;
  archivo: string;
  boton:string="Cargar";

  constructor( private projectService: ProjectService,public dialog: MatDialog,) {
    this.declararInputFile();
  }

  ngOnInit(): void {
  }

  private declararInputFile() {
    this.fileListaProyectos = new FormControl('', [Validators.required]);
  }

  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }
  si()
  {
    console.log("Si");
  }

  enviarListaProyectos() {
    if (this.fileListaProyectos.valid) {
      this.boton="Aguarde un momento, estamos procesando el archivo";
      const inputNode: any = document.getElementById('file');    
      this.projectService.uploadFileProject(inputNode.files[0]).subscribe(
        response => {
          console.log(response);        
          let respuestaCargue:RespuestaProyecto=response.data;
          let strOpciones ="";
          if(respuestaCargue.cantidadDeRegistrosValidos>0)
          {
            strOpciones="<br><b>¿Desea realizar el cargue de los proyectos validos?</b><br>"+
            "<button type='button' (click)='Si()'>Si</button>   <button type='button'>No</button>";
          }
          this.openDialog('Validación de registro', 
          "<br>Número de registros en el archivo:<b>"+respuestaCargue.cantidadDeRegistros+"</b><br>"+
          "Número de registros validos: <b>"+respuestaCargue.cantidadDeRegistrosValidos+"</b><br>"+
          "Número de registros inválidos: <b>"+respuestaCargue.cantidadDeRegistrosInvalidos+"</b><br>"
          );  
         
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
      console.log(this.fileListaProyectos.value);
    }
  }
}
