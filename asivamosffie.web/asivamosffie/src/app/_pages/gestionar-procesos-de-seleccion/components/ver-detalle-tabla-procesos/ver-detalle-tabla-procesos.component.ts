import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ProcesoSeleccion, ProcesoSeleccionService, EstadosProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { pid } from 'process';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';


@Component({
  selector: 'app-ver-detalle-tabla-procesos',
  templateUrl: './ver-detalle-tabla-procesos.component.html',
  styleUrls: ['./ver-detalle-tabla-procesos.component.scss']
})

export class VerDetalleTablaProcesosComponent {

  estadosProcesoSeleccion = EstadosProcesoSeleccion;

  constructor(
              public dialogRef: MatDialogRef<VerDetalleTablaProcesosComponent>,
              @Inject(MAT_DIALOG_DATA) public data: ProcesoSeleccion,
              private router: Router,
              private procesoseleccionService: ProcesoSeleccionService,
              public dialog: MatDialog,
             ) 
  {

  }

  onEditar(){
console.log(this.data.tipoProcesoCodigo)
    switch (this.data.tipoProcesoCodigo)
    {
      case "1": { this.router.navigate(['/seleccion/seccionPrivada', this.data.procesoSeleccionId ]); } break;
      case "2": { this.router.navigate(['/seleccion/invitacionCerrada', this.data.procesoSeleccionId ]); } break;
      case "3": { this.router.navigate(['/seleccion/invitacionAbierta', this.data.procesoSeleccionId ]); } break;
    }

    
    this.dialogRef.close();
  }

  onNoClick(){
    
  }

  onMonitorearCronograma(){
    this.router.navigate(['/seleccion/monitorearCronograma', this.data.procesoSeleccionId ]);
    this.dialogRef.close();
  }

  onEliminar( pId: number ){

    this.openDialogSiNo('','¿Está seguro de eliminar este registro?', pId)
  }

  eliminarRegistro( pId: number ){
    this.procesoseleccionService.deleteProcesoSeleccion( pId ).subscribe( respuesta => {
      let r = respuesta as Respuesta;
       if ( r.code == "200" )
       {
         this.openDialog("Proceso Seleccion", "La información se ha eliminado correctamente,");
         this.router.navigate(['/seleccion']);
         this.dialogRef.close();
       }else
        this.openDialog("Proceso Seleccion", r.message);
    })
  }

  onCerrarProceso( pId: number ){
    let proceso: ProcesoSeleccion = {
      procesoSeleccionId: pId,
      //estadoProcesoSeleccionCodigo: this.estadosProcesoSeleccion.Creado
    }

    this.openDialog("Proceso Seleccion", "falta integrar, falta el estado 'cerrado'.");

    // this.procesoseleccionService.changeStateProcesoSeleccion( proceso ).subscribe( respuesta => {
    //   this.openDialog("Proceso Seleccion", respuesta.message);
    //   if ( respuesta.code == "200" )
    //      this.dialogRef.close();
    // })
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result)
      {
        this.eliminarRegistro(e);
      }           
    });
  }
}
