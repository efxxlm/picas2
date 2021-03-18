import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    id: '1',
    fechaEnvio: '29/11/2020',
    fechaAprobacion: '29/11/2020',
    llaveMEN: 'LL457326',
    tipoIntervencion: 'Remodelación',
    institucionEducativa: 'I.E Nuestra Señora Del Carmen',
    sede: 'Única sede',
    estadoValidacion: 'Sin validación'
  }
];

@Component({
  selector: 'app-tabla-informe-final',
  templateUrl: './tabla-informe-final.component.html',
  styleUrls: ['./tabla-informe-final.component.scss']
})
export class TablaInformeFinalComponent implements OnInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'fechaEnvio',
    'fechaAprobacion',
    'llaveMEN',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoValidacion',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

  // ngAfterViewInit() {
  //   this.dataSource.sort = this.sort;
  //   this.dataSource.paginator = this.paginator;
  //   this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  //   this.paginator._intl.nextPageLabel = 'Siguiente';
  //   this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
  //     if (length === 0 || pageSize === 0) {
  //       return '0 de ' + length;
  //     }
  //     length = Math.max(length, 0);
  //     const startIndex = page * pageSize;
  //     // If the start index exceeds the list length, do not try and fix the end index to the end.
  //     const endIndex = startIndex < length ?
  //       Math.min(startIndex + pageSize, length) :
  //       startIndex + pageSize;
  //     return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
  //   };
  //   this.paginator._intl.previousPageLabel = 'Anterior';
  // }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}