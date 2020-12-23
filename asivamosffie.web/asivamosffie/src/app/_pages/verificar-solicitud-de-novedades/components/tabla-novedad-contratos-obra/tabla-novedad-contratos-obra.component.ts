import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface VerificacionDiaria {
  id: string;
  fecha: string;
  numeroSolicitud: string;
  numeroContrato: string;
  tipoNovedad: string;
  estadoNovedad: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fecha: '15/10/2020',
    numeroSolicitud: 'NOV-001',
    numeroContrato: 'A887654344',
    tipoNovedad: 'Modificación de Condiciones Contractuales',
    estadoNovedad: 'Sin verificar',
    estadoRegistro: 'Incompleto',
  },
  {
    id: '2',
    fecha: '17/10/2020',
    numeroSolicitud: 'NOV-006',
    numeroContrato: 'A887654344',
    tipoNovedad: 'Suspensión',
    estadoNovedad: 'Sin verificar',
    estadoRegistro: 'Incompleto',
  },
];

@Component({
  selector: 'app-tabla-novedad-contratos-obra',
  templateUrl: './tabla-novedad-contratos-obra.component.html',
  styleUrls: ['./tabla-novedad-contratos-obra.component.scss']
})
export class TablaNovedadContratosObraComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fecha',
    'numeroSolicitud',
    'numeroContrato',
    'tipoNovedad',
    'estadoNovedad',
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
