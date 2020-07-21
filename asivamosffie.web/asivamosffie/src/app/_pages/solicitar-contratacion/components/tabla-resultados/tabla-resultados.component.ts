import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface TableElement {
  id: number;
  tipoInterventor: string;
  llaveMEN: string;
  region: string;
  departamento: string;
  municipio: string;
  institucionEducativa: string;
  sede: string;
}

const ELEMENT_DATA: TableElement[] = [
  {
    id: 0,
    tipoInterventor: 'Reconstrucción',
    llaveMEN: 'MY567890',
    region: 'Caribe',
    departamento: 'Baranoa',
    municipio: 'Atlántico',
    institucionEducativa: 'Sede 2 - María Inmaculada',
    sede: 'Sede 2',
  },
  {
    id: 1,
    tipoInterventor: 'Reconstrucción',
    llaveMEN: 'LJ867890',
    region: 'Caribe',
    departamento: 'Galapa',
    municipio: 'Atlántico',
    institucionEducativa: 'I.E María Auxiliadora',
    sede: 'Única sede',
  }
];

@Component({
  selector: 'app-tabla-resultados',
  templateUrl: './tabla-resultados.component.html',
  styleUrls: ['./tabla-resultados.component.scss']
})


export class TablaResultadosComponent implements OnInit {

  displayedColumns: string[] = [
    'tipoInterventor',
    'llaveMEN',
    'region',
    'departamento',
    'institucionEducativa',
    'sede',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor() { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

}
