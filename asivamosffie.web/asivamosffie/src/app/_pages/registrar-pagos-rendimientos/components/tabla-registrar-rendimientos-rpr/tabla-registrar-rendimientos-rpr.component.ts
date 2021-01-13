import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogCargarReportRendRprComponent } from '../dialog-cargar-report-rend-rpr/dialog-cargar-report-rend-rpr.component';

@Component({
  selector: 'app-tabla-registrar-rendimientos-rpr',
  templateUrl: './tabla-registrar-rendimientos-rpr.component.html',
  styleUrls: ['./tabla-registrar-rendimientos-rpr.component.scss']
})
export class TablaRegistrarRendimientosRprComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaCargue',
    'numTotalRegistros',
    'numRegistrosValidos',
    'numRegistrosInvalidos',
    'registrosInconsistentes',
    'estadoCargue',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaCargue: '10/08/2020',
      numTotalRegistros: 5,
      numRegistrosValidos: 5,
      numRegistrosInvalidos: 0,
      registrosInconsistentes: '---',
      estadoCargue: 'Valido',
      gestion: 1
    },
    {
      fechaCargue: '15/08/2020',
      numTotalRegistros: 5,
      numRegistrosValidos: 5,
      numRegistrosInvalidos: 0,
      registrosInconsistentes: 2,
      estadoCargue: 'Valido',
      gestion: 2
    },
    {
      fechaCargue: '15/08/2020',
      numTotalRegistros: 5,
      numRegistrosValidos: 5,
      numRegistrosInvalidos: 0,
      registrosInconsistentes: 0,
      estadoCargue: 'Valido',
      gestion: 3
    }
  ];
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };  

  cargarNuevoReportedeRendimiento(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    const dialogRef = this.dialog.open(DialogCargarReportRendRprComponent, dialogConfig);
  }
}
