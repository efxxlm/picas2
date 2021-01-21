import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component'

export interface VerificacionDiaria {
  id: string;
  fechaRevision: string;
  numeroSolicitud: string;
  fechaValidacion: string;
  estadoValidacion: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fechaRevision: '15/10/2020',
    numeroSolicitud: 'NOV-001',
    fechaValidacion: 'Modificación de Condiciones Contractuales',
    estadoValidacion: 'En proceso de registro',
  }
];

@Component({
  selector: 'app-tabla-programacion-de-obra',
  templateUrl: './tabla-programacion-de-obra.component.html',
  styleUrls: ['./tabla-programacion-de-obra.component.scss']
})
export class TablaProgramacionDeObraComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaRevision',
    'numeroSolicitud',
    'fechaValidacion',
    'estadoValidacion',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    public dialog: MatDialog
  ) { }

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

  openObservaciones(id: string) {
    const dialogCargarProgramacion = this.dialog.open(DialogObservacionesComponent, {
      width: '75em',
      // data: { }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

}