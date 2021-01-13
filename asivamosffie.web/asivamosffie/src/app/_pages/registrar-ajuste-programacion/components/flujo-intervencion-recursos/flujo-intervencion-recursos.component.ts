import { Component, OnInit, Input, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CargarProgramacionComponent } from '../cargar-programacion/cargar-programacion.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component'

export interface VerificacionDiaria {
  id: string;
  fechaCargue: string;
  numeroToalRegistros: string;
  numeroRegistrosValidos: string;
  numeroRegistrosInalidos: string;
  estadoCargue: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fechaCargue: '10/08/2020',
    numeroToalRegistros: '5',
    numeroRegistrosValidos: '3',
    numeroRegistrosInalidos: '2',
    estadoCargue: 'Fallido',
  }
];
@Component({
  selector: 'app-flujo-intervencion-recursos',
  templateUrl: './flujo-intervencion-recursos.component.html',
  styleUrls: ['./flujo-intervencion-recursos.component.scss']
})
export class FlujoIntervencionRecursosComponent implements AfterViewInit, OnInit  {

  displayedColumns: string[] = [
    'fechaCargue',
    'numeroToalRegistros',
    'numeroRegistrosValidos',
    'numeroRegistrosInalidos',
    'estadoCargue',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
  }

  openCargarProgramacion() {
    const dialogRef = this.dialog.open(CargarProgramacionComponent, {
      width: '75em',
      // data: { }
    });
    dialogRef.afterClosed()
    .subscribe(response => {
      if (response) {
        console.log(response);
      };
    })
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
