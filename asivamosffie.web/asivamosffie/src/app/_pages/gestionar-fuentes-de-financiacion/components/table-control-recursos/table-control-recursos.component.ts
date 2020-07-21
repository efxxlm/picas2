import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface ControlRecursos {
  id: number;
  fechaCreacion: string;
  nombreCuenta: string;
  rp: string;
  vigencia: number;
  fechaConsignacion: string;
  valorConsignacion: number;
}

const ELEMENT_DATA: ControlRecursos[] = [
  {
    id: 0,
    fechaCreacion: '20/05/2020',
    nombreCuenta: 'Recursos corrientes ',
    rp: 'CAE58733398554',
    vigencia: 2021,
    fechaConsignacion: '24/06/2020',
    valorConsignacion: 33000000,
  },
];


@Component({
  selector: 'app-table-control-recursos',
  templateUrl: './table-control-recursos.component.html',
  styleUrls: ['./table-control-recursos.component.scss']
})
export class TableControlRecursosComponent implements OnInit {

  displayedColumns: string[] = [
    'fechaCreacion',
    'nombreCuenta',
    'rp',
    'vigencia',
    'fechaConsignacion',
    'valorConsignacion',
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

  editar(e: number) {
    console.log(e);
  }

  eliminar(e: number) {
    console.log(e);
  }

}
