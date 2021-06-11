import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-ejpresupuestal-gtlc',
  templateUrl: './tabla-ejpresupuestal-gtlc.component.html',
  styleUrls: ['./tabla-ejpresupuestal-gtlc.component.scss']
})
export class TablaEjpresupuestalGtlcComponent implements OnInit {
  @Input() data: any[] = [];

  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'nombre',
    'totalComprometido',
    'facturadoAntesImpuestos',
    'saldo',
    'porcentajeEjecucionPresupuestal'
  ];
  dataTable: any[] = [];
  total: any;

  constructor() {}

  ngOnInit(): void {
    this.loadTableData();
  }

  loadTableData() {
    if (this.data.length > 0) {
      this.dataTable = this.data;
      this.total = {
        totalComprometido: 0,
        facturadoAntesImpuestos: 0,
        saldo: 0,
        porcentajeEjecucionPresupuestal: 0
      };
      this.dataTable.forEach(element => {
        this.total.totalComprometido += element.totalComprometido;
        this.total.facturadoAntesImpuestos = this.total.facturadoAntesImpuestos + element.facturadoAntesImpuestos;
        this.total.saldo = this.total.saldo + element.saldo;
        this.total.porcentajeEjecucionPresupuestal =
          this.total.porcentajeEjecucionPresupuestal + element.porcentajeEjecucionPresupuestal;
      });
      if (this.total.porcentajeEjecucionPresupuestal > 0)
        this.total.porcentajeEjecucionPresupuestal = this.total.porcentajeEjecucionPresupuestal / 2;
    }
    this.loadDataSource();
  }

  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
