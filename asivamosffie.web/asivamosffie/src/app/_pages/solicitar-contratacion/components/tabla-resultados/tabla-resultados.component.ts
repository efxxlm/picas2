import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

import { DialogTableProyectosSeleccionadosComponent } from '../dialog-table-proyectos-seleccionados/dialog-table-proyectos-seleccionados.component';
import { AsociadaComponent } from '../asociada/asociada.component';

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

  elementosSelecciondos: any[] = [];

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  addElement(event: any, elemento: any) {
    console.log(event);
    this.elementosSelecciondos.push(elemento);
  }

  verSeleccionados() {
    console.log(this.elementosSelecciondos);
    const dialogRef = this.dialog.open(DialogTableProyectosSeleccionadosComponent, {
      data: this.elementosSelecciondos
    });
  }

  openPopup() {
    this.dialog.open(AsociadaComponent);
  }
}
