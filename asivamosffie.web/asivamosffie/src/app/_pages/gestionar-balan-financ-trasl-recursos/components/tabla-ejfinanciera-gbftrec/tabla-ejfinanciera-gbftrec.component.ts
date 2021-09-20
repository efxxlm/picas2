import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-ejfinanciera-gbftrec',
  templateUrl: './tabla-ejfinanciera-gbftrec.component.html',
  styleUrls: ['./tabla-ejfinanciera-gbftrec.component.scss']
})
export class TablaEjfinancieraGbftrecComponent implements OnInit {
  @Input() data: any[] = [];

  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'nombre',
    'totalComprometido',
    'ordenadoGirarAntesImpuestos',
    'saldo',
    'porcentajeEjecucionFinanciera'
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

      // console.log(this.dataTable);

      let tablaEjecucionFinanciera = [
        {
          nombre: 'Obra',
          ordenadoGirarAntesImpuestos: 0,
          porcentajeEjecucionFinanciera: 0,
          descuento: 0,
          saldo: 0,
          totalComprometido: 0
        },
        {
          nombre: 'Interventoria',
          ordenadoGirarAntesImpuestos: 0,
          porcentajeEjecucionFinanciera: 0,
          descuento: 0,
          saldo: 0,
          totalComprometido: 0
        }
      ];

      let concepto;

      this.dataTable.forEach(element => {
        if (element.nombre === 'Obra') {
          concepto = 0;
        } else if (element.nombre === 'Interventoria') {
          concepto = 1;
        }
          if (element.totalComprometido) tablaEjecucionFinanciera[concepto].totalComprometido = element.totalComprometido;
          if (element.descuento) tablaEjecucionFinanciera[concepto].descuento = element.descuento;
          tablaEjecucionFinanciera[concepto].ordenadoGirarAntesImpuestos += element.ordenadoGirarAntesImpuestos;
      });
      for (let i = 0; i <= 1; i++) {
        tablaEjecucionFinanciera[i].ordenadoGirarAntesImpuestos -= tablaEjecucionFinanciera[i].descuento;
        tablaEjecucionFinanciera[i].saldo = tablaEjecucionFinanciera[i].totalComprometido - tablaEjecucionFinanciera[i].ordenadoGirarAntesImpuestos;
        tablaEjecucionFinanciera[i].porcentajeEjecucionFinanciera = (tablaEjecucionFinanciera[i].ordenadoGirarAntesImpuestos * 100) / tablaEjecucionFinanciera[i].totalComprometido;
      }

      this.total = {
        totalComprometido: 0,
        ordenadoGirarAntesImpuestos: 0,
        saldo: 0,
        porcentajeEjecucionFinanciera: 0
      };
      tablaEjecucionFinanciera.forEach(element => {
        this.total.totalComprometido += element.totalComprometido;
        this.total.ordenadoGirarAntesImpuestos =
          this.total.ordenadoGirarAntesImpuestos + element.ordenadoGirarAntesImpuestos;
        this.total.saldo = this.total.saldo + element.saldo;
        this.total.porcentajeEjecucionFinanciera =
          this.total.porcentajeEjecucionFinanciera + element.porcentajeEjecucionFinanciera;
      });
      if (this.total.porcentajeEjecucionFinanciera > 0)
        this.total.porcentajeEjecucionFinanciera = this.total.porcentajeEjecucionFinanciera / 2;
      this.loadDataSource(tablaEjecucionFinanciera);
    }
  }

  loadDataSource(tablaEjecucionFinanciera) {
    this.dataSource = new MatTableDataSource(tablaEjecucionFinanciera);
    this.dataSource.sort = this.sort;
  }
}
