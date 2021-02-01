import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-detalle-reg-sol-pago-validfsp',
  templateUrl: './detalle-reg-sol-pago-validfsp.component.html',
  styleUrls: ['./detalle-reg-sol-pago-validfsp.component.scss']
})
export class DetalleRegSolPagoValidfspComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'faseContrato',
    'pagosRealizados',
    'valorFacturado',
    'porcentajeFacturado',
    'saldoPorPagar',
    'porcentajePorPagar'
  ];
  dataTable: any[] = [
    {
      faseContrato: 'Fase 1 - Preconstrucción',
      pagosRealizados: '0',
      valorFacturado: '0',
      porcentajeFacturado: '0',
      saldoPorPagar: '$30.000.000',
      porcentajePorPagar: '100%',
    },
    {
      faseContrato: 'Fase 2 - Construcción',
      pagosRealizados: '0',
      valorFacturado: '0',
      porcentajeFacturado: '0',
      saldoPorPagar: '$75.000.000',
      porcentajePorPagar: '100%',
    }
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

}
