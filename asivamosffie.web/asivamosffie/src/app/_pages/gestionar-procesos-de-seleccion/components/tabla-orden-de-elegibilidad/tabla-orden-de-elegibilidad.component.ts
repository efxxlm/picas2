import { Component, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface ProcesosElement {
  id: number;
  fechaCargue: string;
  numeroRegistros: number;
  numeroRegistrosValidos: number;
  numeroRegistrosInalidos: number;
}

const ELEMENT_DATA: ProcesosElement[] = [
  {
    id: 0,
    fechaCargue: '25/03/2020',
    numeroRegistros: 1,
    numeroRegistrosValidos: 1,
    numeroRegistrosInalidos: 0
  }
];

@Component({
  selector: 'app-tabla-orden-de-elegibilidad',
  templateUrl: './tabla-orden-de-elegibilidad.component.html',
  styleUrls: ['./tabla-orden-de-elegibilidad.component.scss']
})
export class TablaOrdenDeElegibilidadComponent implements OnInit {

  displayedColumns: string[] = ['fechaCargue', 'numeroRegistros', 'numeroRegistrosValidos', 'numeroRegistrosInalidos', 'id'];
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
  }

  verDetalle(e: number) {
    console.log(e);
  }

}
