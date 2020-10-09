import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { CommonService } from '../../../../core/_services/common/common.service';

export interface PeriodicElement {
  id: number;
  fecha: string;
  numContrato: string;
  proyAsociados: number;
  proyConRequisitosAprovados: number;
  proyConRequisitosPendientes: number;
  estado: string;
}

@Component({
  selector: 'app-tabla-registrar-requisitos',
  templateUrl: './tabla-registrar-requisitos.component.html',
  styleUrls: ['./tabla-registrar-requisitos.component.scss']
})
export class TablaRegistrarRequisitosComponent implements OnInit {

  verAyuda = false;
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor ( private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
                private commonSvc: CommonService ) 
  {
    commonSvc.listaEstadosVerificacionContrato()
      .subscribe( console.log )
    this.faseUnoPreconstruccionSvc.getListContratacion()
      .subscribe( listas => {
        console.log( listas );
        this.dataSource = new MatTableDataSource( listas );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
          if (length === 0 || pageSize === 0) {
            return '0 de ' + length;
          }
          length = Math.max(length, 0);
          const startIndex = page * pageSize;
          // If the start index exceeds the list length, do not try and fix the end index to the end.
          const endIndex = startIndex < length ?
            Math.min(startIndex + pageSize, length) :
            startIndex + pageSize;
          return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
        };
      } );
  }

  ngOnInit(): void {
  }

  aprobarInicio () {
    console.log( 'Aprobando Inicio' );
  }

}
