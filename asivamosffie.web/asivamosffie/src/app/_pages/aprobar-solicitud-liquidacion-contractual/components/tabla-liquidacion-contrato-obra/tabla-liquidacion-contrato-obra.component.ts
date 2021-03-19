import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    id: '1',
    fechaValidacion: '15/11/2020',
    fechaActualizacion: '10/11/2020',
    numeroSolicitud: 'SL00001',
    numeroContrato: 'N801801',
    valor: '$ 105.000.000',
    cantidadProyectosAsociados: '1',
    estadoAprobacion: 'Sin aprobación'
  }
];

@Component({
  selector: 'app-tabla-liquidacion-contrato-obra',
  templateUrl: './tabla-liquidacion-contrato-obra.component.html',
  styleUrls: ['./tabla-liquidacion-contrato-obra.component.scss']
})
export class TablaLiquidacionContratoObraComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'fechaValidacion',
    'fechaActualizacion',
    'numeroSolicitud',
    'numeroContrato',
    'valor',
    'cantidadProyectosAsociados',
    'estadoAprobacion',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
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