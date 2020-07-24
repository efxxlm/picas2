import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface SolicitudesElement {
  id: number;
  fecha: string;
  numero: string;
  opcionPorContratar: string;
  estadoSolicitud: string;
  estadoDelIngreso: string;
}

const ELEMENT_DATA: SolicitudesElement[] = [
  {
    id: 0,
    fecha: '9/06/2020',
    numero: 'P.I-0001',
    opcionPorContratar: 'Obra',
    estadoSolicitud: '',
    estadoDelIngreso: 'Incompleto'
  }
];

@Component({
  selector: 'app-table-solicitud-contratacion',
  templateUrl: './table-solicitud-contratacion.component.html',
  styleUrls: ['./table-solicitud-contratacion.component.scss']
})
export class TableSolicitudContratacionComponent implements OnInit {

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'opcionPorContratar',
    'estadoSolicitud',
    'estadoDelIngreso',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  detallarSolicitud(id: number) {
    console.log(id);
  }
  eliminarSolicitud(id: number) {
    console.log(id);
  }

}
