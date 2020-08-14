import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface Solicitudes {
  fecha: string;
  numero: string;
  tipo: string;
}

const ELEMENT_DATA: Solicitudes[] = [
  {fecha: '23/06/2020', numero: 'SA0006', tipo: 'Apertura de proceso de selección'},
  {fecha: '22/06/2020', numero: 'SC0005', tipo: 'Evaluación de proceso de selección'},
  {fecha: '22/06/2020', numero: 'PI0004', tipo: 'Contratación'},
];

@Component({
  selector: 'app-tabla-solicitudes-contractuales',
  templateUrl: './tabla-solicitudes-contractuales.component.html',
  styleUrls: ['./tabla-solicitudes-contractuales.component.scss']
})
export class TablaSolicitudesContractualesComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'tipo'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor() { }

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

}
