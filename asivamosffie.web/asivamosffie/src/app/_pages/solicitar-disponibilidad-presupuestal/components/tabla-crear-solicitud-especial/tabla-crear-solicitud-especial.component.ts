import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface Solicitudes {
  id: number;
  fecha: string;
  numero: string;
  valorSolicitado: number;
  estadoSolicitud: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: Solicitudes[] = [
  {
    id: 1,
    fecha: '08/07/2020',
    numero: 'DE_001',
    valorSolicitado: 7500000,
    estadoSolicitud: 'Sin registrar',
    estadoRegistro: 'Completo'
  },
];

@Component({
  selector: 'app-tabla-crear-solicitud-especial',
  templateUrl: './tabla-crear-solicitud-especial.component.html',
  styleUrls: ['./tabla-crear-solicitud-especial.component.scss']
})
export class TablaCrearSolicitudEspecialComponent implements OnInit {

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'valorSolicitado',
    'estadoSolicitud',
    'estadoRegistro',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

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
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) { return '0 de ' + length; }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  enviarSolicitud(e: number) {
    console.log(e);
  }

  editar(e: number) {
    console.log(e);
  }

  eliminar(e: number) {
    console.log(e);
  }

}
