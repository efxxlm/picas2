import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { DialogVerDetalleComponent } from '../dialog-ver-detalle/dialog-ver-detalle.component';

export interface OrdenDelDia {
  id: number;
  tarea: string;
  responsable: string;
  fechaCumplimiento: string;
  fechaReporte: string;
  estadoReportado: string;
  gestionRealizada: number;
}

const ELEMENT_DATA: OrdenDelDia[] = [
  {
    id: 0,
    tarea: 'Realizar seguimiento semanal del cronograma',
    responsable: 'Julian Guillermo García Pedreros',
    fechaCumplimiento: '30/06/2020',
    fechaReporte: '02/07/2020',
    estadoReportado: 'En proceso',
    gestionRealizada: 0
  }
];

@Component({
  selector: 'app-tabla-verificar-cumplimiento',
  templateUrl: './tabla-verificar-cumplimiento.component.html',
  styleUrls: ['./tabla-verificar-cumplimiento.component.scss']
})
export class TablaVerificarCumplimientoComponent implements OnInit {

  displayedColumns: string[] = [
    'tarea',
    'responsable',
    'fechaCumplimiento',
    'fechaReporte',
    'estadoReportado',
    'gestionRealizada',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  openVerDetalle(id: number) {
    this.dialog.open(DialogVerDetalleComponent, {
      width: '70em'
    });
  }

}
