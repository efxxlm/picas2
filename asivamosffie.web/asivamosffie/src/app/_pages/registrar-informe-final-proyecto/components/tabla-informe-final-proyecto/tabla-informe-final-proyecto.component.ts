import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface VerificacionDiaria {
  id: string;
  fechaTerminacion: string;
  llaveMEN: string;
  tipoIntervencion: string;
  institucionEducativa: string;
  sede: string;
  estadoInformeFinal: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fechaTerminacion: '21/06/2020',
    llaveMEN: 'LJ776554',
    tipoIntervencion: 'Remodelación',
    institucionEducativa: 'I.E. María Villa Campo',
    sede: 'Única Sede',
    estadoInformeFinal: 'Sin registrar',
    estadoRegistro: 'Incompleto',
  }
];

@Component({
  selector: 'app-tabla-informe-final-proyecto',
  templateUrl: './tabla-informe-final-proyecto.component.html',
  styleUrls: ['./tabla-informe-final-proyecto.component.scss']
})

export class TablaInformeFinalProyectoComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaTerminacion',
    'llaveMEN',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoInformeFinal',
    'estadoRegistro',
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
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      return (page + 1).toString() + ' de ' + length.toString();
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