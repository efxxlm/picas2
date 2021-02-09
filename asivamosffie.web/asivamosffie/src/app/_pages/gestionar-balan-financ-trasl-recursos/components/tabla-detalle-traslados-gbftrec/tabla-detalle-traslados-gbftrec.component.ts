import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-detalle-traslados-gbftrec',
  templateUrl: './tabla-detalle-traslados-gbftrec.component.html',
  styleUrls: ['./tabla-detalle-traslados-gbftrec.component.scss']
})
export class TablaDetalleTrasladosGbftrecComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaTraslado',
    'numTraslado',
    'numContrato',
    'numOrdenGiro',
    'valorTraslado',
    'estadoTraslado',
    'gestion'
  ];
  dataTable: any[] = [{
    fechaTraslado: '09/08/2021',
    numTraslado: 'Tras_001',
    numContrato: 'N000000',
    numOrdenGiro: 'ODG_obra_222',
    valorTraslado: '$5.000.000',
    estadoTraslado: 'Con registro',
    id: 1
  }];
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadTable();
  }

  loadTable(){
    this.dataSource = new MatTableDataSource(this.dataTable);
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
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  verDetalle(id){
    
  }
}
