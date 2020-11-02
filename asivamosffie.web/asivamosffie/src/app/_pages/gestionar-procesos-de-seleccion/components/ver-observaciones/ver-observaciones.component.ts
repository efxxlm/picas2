import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ProcesoSeleccion, ProcesoSeleccionService, EstadosProcesoSeleccion, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { pid } from 'process';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';


@Component({
  selector: 'app-ver-observaciones',
  templateUrl: './ver-observaciones.component.html',
  styleUrls: ['./ver-observaciones.component.scss']
})


export class VerObservacionesComponent implements OnInit{

  estadosProcesoSeleccion = EstadosProcesoSeleccion;
  tiposProcesoSeleccion = TiposProcesoSeleccion; 
  observaciones: any[];
  
  constructor(
              public dialogRef: MatDialogRef<VerObservacionesComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private router: Router,
              private procesoseleccionService: ProcesoSeleccionService,
              public dialog: MatDialog,
             ) 
  {

  }
  ngOnInit(): void {
    //this.activarBotones();
    console.log(this.data);
    this.procesoseleccionService.getObservacionesByID(this.data.id).subscribe(result=>
      {
        this.observaciones=result;
      });
  }

  

}
