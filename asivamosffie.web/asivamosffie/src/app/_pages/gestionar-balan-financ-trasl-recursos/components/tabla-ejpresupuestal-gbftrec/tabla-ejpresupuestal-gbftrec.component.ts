import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-ejpresupuestal-gbftrec',
  templateUrl: './tabla-ejpresupuestal-gbftrec.component.html',
  styleUrls: ['./tabla-ejpresupuestal-gbftrec.component.scss']
})
export class TablaEjpresupuestalGbftrecComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'componente',
    'totalComprometido',
    'facturadoAntesdeImpuestos',
    'saldo',
    'porcentajeEjecucionPresupuestal'
  ];
  dataTable: any[] = [
    {
      componente: 'Obra',
      totalComprometido: '$65.000.000',
      facturadoAntesdeImpuestos: '$40.000.000',
      saldo: '$25.000.000',
      porcentajeEjecucionPresupuestal:'65%'
    },
    {
      componente: 'Interventoría',
      totalComprometido: '$40.000.000',
      facturadoAntesdeImpuestos: '$28.000.000',
      saldo: '$12.000.000',
      porcentajeEjecucionPresupuestal:'70%'
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
