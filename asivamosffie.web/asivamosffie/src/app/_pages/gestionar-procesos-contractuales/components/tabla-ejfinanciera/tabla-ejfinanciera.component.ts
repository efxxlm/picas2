import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-ejfinanciera',
  templateUrl: './tabla-ejfinanciera.component.html',
  styleUrls: ['./tabla-ejfinanciera.component.scss']
})
export class TablaEjfinancieraComponent implements OnInit {

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
    console.log(this.data);
    this.loadTableData();
  }

  loadTableData() {
    if (this.data.length > 0) {
      this.dataTable = this.data;
      this.total = {
        totalComprometido: 0,
        ordenadoGirarAntesImpuestos: 0,
        saldo: 0,
        porcentajeEjecucionFinanciera: 0
      };
      this.dataTable.forEach(element => {
        this.total.totalComprometido += element.totalComprometido;
        this.total.ordenadoGirarAntesImpuestos =
          this.total.ordenadoGirarAntesImpuestos + element.ordenadoGirarAntesImpuestos;
        this.total.saldo = this.total.saldo + element.saldo;
        this.total.porcentajeEjecucionFinanciera =
          this.total.porcentajeEjecucionFinanciera + element.porcentajeEjecucionFinanciera;
      });
      if (this.total.porcentajeEjecucionFinanciera > 0)
        this.total.porcentajeEjecucionFinanciera = this.total.porcentajeEjecucionFinanciera / 2;
    }
    this.loadDataSource();
  }

  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }

}
