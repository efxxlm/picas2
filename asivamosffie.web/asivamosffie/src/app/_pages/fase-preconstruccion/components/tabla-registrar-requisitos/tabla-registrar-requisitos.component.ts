import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';

export interface PeriodicElement {
  id: number;
  fecha: string;
  numContrato: string;
  proyAsociados: number;
  proyConRequisitosAprovados: number;
  proyConRequisitosPendientes: number;
  estado: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {
    id: 0,
    fecha: '21/06/2020',
    numContrato: 'C223456789',
    proyAsociados: 2,
    proyConRequisitosAprovados: 0,
    proyConRequisitosPendientes: 2,
    estado: 'Sin aprobación de requisitos técnicos'
  },
  {
    id: 1,
    fecha: '20/06/2020',
    numContrato: 'A886675445',
    proyAsociados: 1,
    proyConRequisitosAprovados: 0,
    proyConRequisitosPendientes: 1,
    estado: 'Sin aprobación de requisitos técnicos'
  },
  {
    id: 2,
    fecha: '11/05/2020',
    numContrato: 'C333344786',
    proyAsociados: 1,
    proyConRequisitosAprovados: 0,
    proyConRequisitosPendientes: 1,
    estado: 'Sin aprobación de requisitos técnicos'
  },
];

@Component({
  selector: 'app-tabla-registrar-requisitos',
  templateUrl: './tabla-registrar-requisitos.component.html',
  styleUrls: ['./tabla-registrar-requisitos.component.scss']
})
export class TablaRegistrarRequisitosComponent implements OnInit {

  verAyuda = false;
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaAprobacionPoliza',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'proyectosCompletos',
    'proyectosNoCompletos',
    'estadoVerificacionNombre',
    'gestion'
  ];

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor ( private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) {
    this.faseUnoPreconstruccionSvc.getListContratacion()
      .subscribe( listas => {
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

}
