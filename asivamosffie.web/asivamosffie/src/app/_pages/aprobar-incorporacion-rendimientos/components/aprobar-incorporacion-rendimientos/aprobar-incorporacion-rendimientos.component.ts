import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogCargarActaFirmadaAirComponent } from '../dialog-cargar-acta-firmada-air/dialog-cargar-acta-firmada-air.component';


@Component({
  selector: 'app-aprobar-incorporacion-rendimientos',
  templateUrl: './aprobar-incorporacion-rendimientos.component.html',
  styleUrls: ['./aprobar-incorporacion-rendimientos.component.scss']
})
export class AprobarIncorporacionRendimientosComponent implements OnInit {
  verAyuda = false;
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaCargue',
    'numTotalRegistrosIncorporados',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaCargue: '10/08/2020',
      numTotalRegistrosIncorporados: 5,
      gestion: 1
    }
  ]
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

  
  cargarActaFirmada(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    const dialogRef = this.dialog.open(DialogCargarActaFirmadaAirComponent, dialogConfig);
  }
}
