import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

import { VerDetalleTablaProcesosComponent } from "../ver-detalle-tabla-procesos/ver-detalle-tabla-procesos.component";
import { ProcesoSeleccionService, ProcesoSeleccion, EstadosProcesoSeleccion, TiposProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-tabla-procesos',
  templateUrl: './tabla-procesos.component.html',
  styleUrls: ['./tabla-procesos.component.scss']
})
export class TablaProcesosComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaCreacion', 'tipo', 'numero', 'etapa', 'estadoDelProceso', 'estadoDelRegistro', 'id'];
  dataSource = new MatTableDataSource();
  listaProceso: ProcesoSeleccion[] = [];
  estadosProcesoSeleccion = EstadosProcesoSeleccion;
  
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
              public dialog: MatDialog,
              private procesoSeleccionService: ProcesoSeleccionService,
              private commonService: CommonService
             )
  { 

  }

  ngOnInit(): void {

    forkJoin([

      this.procesoSeleccionService.listaProcesosSeleccion(),
      this.commonService.listaTipoProcesoSeleccion(),
      this.commonService.listaEstadoProcesoSeleccion(),
      this.commonService.listaEtapaProcesoSeleccion(),

    ]).subscribe( respuesta => {

      this.listaProceso = respuesta[0];

      this.listaProceso.forEach( proceso => {
        let nombreTipo = respuesta[1].find( p => p.codigo == proceso.tipoProcesoCodigo )
        let nombreEstado = respuesta[2].find( p => p.codigo == proceso.estadoProcesoSeleccionCodigo )
        let nombreEtapa = respuesta[3].find( p => p.codigo == proceso.etapaProcesoSeleccionCodigo )
        
        if (nombreTipo)   proceso.tipoProcesoNombre = nombreTipo.nombre;
        if (nombreEstado) proceso.estadoProcesoSeleccionNombre = nombreEstado.nombre;
        if (nombreEtapa)  proceso.etapaProcesoSeleccionNombre = nombreEtapa.nombre;
        
        if(proceso.estadoProcesoSeleccionCodigo==this.estadosProcesoSeleccion.AprobadaAperturaPorComiteFiduciario)
        {
          //valido si esta incompleto si no tiene datos de evaluacion y proponentes seleccionados
          if(proceso.evaluacionDescripcion!="" 
          && proceso.urlSoporteEvaluacion!=""
        
        )
          {
            //si cerrada debe tener contratista
            if( proceso.tipoProcesoCodigo==TiposProcesoSeleccion.Cerrada)
            {
              if(proceso.listaContratistas?.length>0)
              {
                proceso.esCompleto=true;                
              }              
              else
              {
                proceso.esCompleto=false;
              }
            }
            else
            {
              proceso.esCompleto=true;  
            }            
          }
          else{
            proceso.esCompleto=false;
          }
        }
          //this.dataSource = new MatTableDataSource( respuesta[0] );
      })
      this.dataSource = new MatTableDataSource( this.listaProceso );
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';  

    })

    
  }

  verMas(e: number) {
    console.log(this.listaProceso.find( p => p.procesoSeleccionId == e ));
    const dialogRef = this.dialog.open(VerDetalleTablaProcesosComponent, {
      data: this.listaProceso.find( p => p.procesoSeleccionId == e )
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(VerDetalleTablaProcesosComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  // verDetalle(e: number): void {
    
  //   const dialogRef = this.dialog.open(VerDetalleTablaProcesosComponent, {
  //     data: ELEMENT_DATA[e]
  //   });
  // }

}
