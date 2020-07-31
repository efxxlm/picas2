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
    this.router.navigate(['/seleccion/seccionPrivada', this.data.tipoProcesoCodigo, this.data.procesoSeleccionId ]);
    this.dialogRef.close();
  }
}
