import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  tipo: string;
}

const ELEMENT_DATA: OrdenDelDia[] = [
  { id: 0, fecha: '23/06/2020', numero: 'SA0006', tipo: 'Sin Apertura de proceso de selección' }
];

@Component({
  selector: 'app-tabla-validacion-solicitudes-contractuales',
  templateUrl: './tabla-validacion-solicitudes-contractuales.component.html',
  styleUrls: ['./tabla-validacion-solicitudes-contractuales.component.scss']
})
export class TablaValidacionSolicitudesContractualesComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'id'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

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
