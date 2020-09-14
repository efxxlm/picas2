import { Component, OnInit, ViewChild } from '@angular/core';
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
    tipoInterventor: 'Ampliación',
    llaveMEN: 'MY567444',
    region: 'Pacífico',
    departamento: 'Jamundí',
    municipio: 'Valle del Cauca',
    institucionEducativa: 'I.E Alfredo Bonilla Montaño',
    sede: 'Única sede',
  },
  {
    id: 1,
    tipoInterventor: 'Reconstrucción',
    llaveMEN: 'L809KLJ3',
    region: 'Caribe',
    departamento: 'Manatí',
    municipio: 'Atlántico',
    institucionEducativa: 'I.E Antonio Nariño',
    sede: 'Única sede',
  }
];

@Component({
  selector: 'app-table-fuentes-y-usos',
  templateUrl: './table-fuentes-y-usos.component.html',
  styleUrls: ['./table-fuentes-y-usos.component.scss']
})
export class TableFuentesYUsosComponent implements OnInit {

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

  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor() { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
  }

}
