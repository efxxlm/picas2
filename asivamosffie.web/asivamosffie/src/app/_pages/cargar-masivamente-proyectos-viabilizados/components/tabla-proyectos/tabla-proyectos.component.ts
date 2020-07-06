import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface RegistrosCargados {
  id: number;
  fechaCargue: string;
  totalRegistros: number;
  registrosValidos: number;
  registrosInvalidos: number;
}

const ELEMENT_DATA: RegistrosCargados[] = [
  {
    id: 1,
    fechaCargue: '25/03/2020',
    totalRegistros: 5,
    registrosValidos: 3,
    registrosInvalidos: 2
  },
  {
    id: 2,
    fechaCargue: '27/03/2020',
    totalRegistros: 9,
    registrosValidos: 2,
    registrosInvalidos: 1
  },
];

@Component({
  selector: 'app-tabla-proyectos',
  templateUrl: './tabla-proyectos.component.html',
  styleUrls: ['./tabla-proyectos.component.scss']
})

export class TablaProyectosComponent implements OnInit {

  displayedColumns: string[] = ['fechaCargue', 'totalRegistros', 'registrosValidos', 'registrosInvalidos'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  columnas = [
    { titulo: 'Fecha de cargue', name: 'fechaCargue' },
    { titulo: 'Número total de registros ', name: 'totalRegistros' },
    { titulo: 'Número de registros validos ', name: 'registrosValidos' },
    { titulo: 'Número de registros inválidos ', name: 'registrosInvalidos' },
  ];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() {
  }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

}
