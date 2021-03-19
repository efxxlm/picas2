import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-ejfinanciera-gtlc',
  templateUrl: './tabla-ejfinanciera-gtlc.component.html',
  styleUrls: ['./tabla-ejfinanciera-gtlc.component.scss']
})
export class TablaEjfinancieraGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'componente',
    'totalComprometido',
    'ordenadoGirarAntesdeImpuestos',
    'saldo',
    'porcentajeEjecucionFinanciera'
  ];
  dataTable: any[] = [
    {
      componente: 'Obra',
      totalComprometido: '$65.000.000',
      ordenadoGirarAntesdeImpuestos: '$52.050.000',
      saldo: '$12.950.000',
      porcentajeEjecucionFinanciera:'80%'
    },
    {
      componente: 'Interventoría',
      totalComprometido: '$40.000.000',
      ordenadoGirarAntesdeImpuestos: '$32.050.000',
      saldo: '$7.950.000',
      porcentajeEjecucionFinanciera:'80%'
    }
  ];
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
