import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';


export interface ProcesosElement {
  id: number;
  fechaCreacion: string;
  tipo: string;
  numero: string;
  etapa: string;
  estadoDelProceso: string;
  estadoDelRegistro: string;
}

@Component({
  selector: 'app-ver-detalle-tabla-procesos',
  templateUrl: './ver-detalle-tabla-procesos.component.html',
  styleUrls: ['./ver-detalle-tabla-procesos.component.scss']
})

export class VerDetalleTablaProcesosComponent {

  constructor(
              public dialogRef: MatDialogRef<VerDetalleTablaProcesosComponent>,
              @Inject(MAT_DIALOG_DATA) public data: ProcesoSeleccion,
              private router: Router
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
}
