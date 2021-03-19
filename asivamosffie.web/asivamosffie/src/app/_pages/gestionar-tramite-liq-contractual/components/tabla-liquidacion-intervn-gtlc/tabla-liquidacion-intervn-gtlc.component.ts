import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-liquidacion-intervn-gtlc',
  templateUrl: './tabla-liquidacion-intervn-gtlc.component.html',
  styleUrls: ['./tabla-liquidacion-intervn-gtlc.component.scss']
})
export class TablaLiquidacionIntervnGtlcComponent implements OnInit {
  displayedColumns: string[] = ['fechaAprobacion', 'fechaActualizacionPoliza', 'numeroSolicitud', 'numeroContrato', 'valor', 'cantProyectosAsociados', 'estadoVerifiacion', 'gestion'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataTable: any[] = [
    {
      fechaAprobacion : '16/11/2020',
      fechaActualizacionPoliza : '10/11/2020',
      numeroSolicitud: 'SL00001',
      numeroContrato: 'N801801',
      valor: '$ 105.000.000',
      cantProyectosAsociados: '1',
      estadoVerifiacion: 'Sin verificación',
      id: 1
    },
    {
      fechaAprobacion : '19/11/2020',
      fechaActualizacionPoliza : '14/11/2020',
      numeroSolicitud: 'SL00002',
      numeroContrato: 'A8897098765',
      valor: '$ 85.000.000',
      cantProyectosAsociados: '1',
      estadoVerifiacion: 'Sin verificación',
      id: 2
    },
    {
      fechaAprobacion : '23/11/2020',
      fechaActualizacionPoliza : '20/11/2020',
      numeroSolicitud: 'SL00003',
      numeroContrato: 'CC909765442',
      valor: '$ 340.000.000',
      cantProyectosAsociados: '4',
      estadoVerifiacion: 'Sin verificación',
      id: 2
    }
  ];
  constructor() { }

  ngOnInit(): void {
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
  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
}
