import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface VerificacionDiaria {
  id: string;
  llaveMEN: string;
  institucionEducativa: string;
  sede: string;
  tipoInterventor: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    llaveMEN: 'LJ776554',
    institucionEducativa: 'I.E. María Villa Campo',
    sede: 'Única sede',
    tipoInterventor: 'Remodelación',
  },
  {
    id: '2',
    llaveMEN: 'LJ776554',
    institucionEducativa: 'María Inmaculada',
    sede: 'Sede 2',
    tipoInterventor: 'Remodelación',
  },
  {
    id: '3',
    llaveMEN: 'LJ776554',
    institucionEducativa: 'I.E. Primera de Mayo',
    sede: 'Única sede',
    tipoInterventor: 'Remodelación',
  }
];

@Component({
  selector: 'app-tabla-proyectos-registrar-novedad',
  templateUrl: './tabla-proyectos-registrar-novedad.component.html',
  styleUrls: ['./tabla-proyectos-registrar-novedad.component.scss']
})
export class TablaProyectosRegistrarNovedadComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'llaveMEN',
    'institucionEducativa',
    'sede',
    'tipoInterventor',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor() { }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

}
