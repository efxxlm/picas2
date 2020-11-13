import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DatePipe } from '@angular/common';

export interface SeguimientoDiario {
  id: string;
  llaveMEN: string;
  tipoInterventor: string;
  institucionEducativa: string;
  Sede: string;
  fechaUltimoReporte: string;
}

const ELEMENT_DATA: SeguimientoDiario[] = [
  {
    id: '1',
    llaveMEN: 'LJ776554',
    tipoInterventor: 'Remodelación',
    institucionEducativa: 'I.E. María Villa Campo',
    Sede: 'Única sede',
    fechaUltimoReporte: 'Sin registro'
  }
];

@Component({
  selector: 'app-tabla-registro-seguimiento-diario',
  templateUrl: './tabla-registro-seguimiento-diario.component.html',
  styleUrls: ['./tabla-registro-seguimiento-diario.component.scss']
})
export class TablaRegistroSeguimientoDiarioComponent implements AfterViewInit {
  displayedColumns: string[] = [
    'llaveMEN',
    'tipoInterventor',
    'institucionEducativa',
    'sede',
    'fechaUltimoReporte',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor() { }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = function (page, pageSize, length) {
      return (page + 1).toString() + " de " + length.toString();
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
