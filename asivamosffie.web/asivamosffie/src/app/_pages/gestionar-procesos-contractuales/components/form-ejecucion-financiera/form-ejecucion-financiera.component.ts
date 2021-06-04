import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-form-ejecucion-financiera',
  templateUrl: './form-ejecucion-financiera.component.html',
  styleUrls: ['./form-ejecucion-financiera.component.scss']
})
export class FormEjecucionFinancieraComponent implements OnInit {

  @Input() ejecucionPresupuestal: any[] = [];

  dataSourcePresupuestal = new MatTableDataSource();
  dataSourceFinanciera = new MatTableDataSource();

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumnsFinanciera: string[] = [
    'nombre',
    'totalComprometido',
    'ordenadoGirarAntesImpuestos',
    'saldo',
    'porcentajeEjecucionFinanciera'
  ];

  displayedColumnsPresupuestal: string[] = [
    'nombre',
    'totalComprometido',
    'facturadoAntesImpuestos',
    'saldo',
    'porcentajeEjecucionPresupuestal'
  ];

  dataTableFinanciera: any[] = [];
  totalFinanciera: any;

  dataTablePresupuestal: any[] = [];
  totalPresupuestal: any;

  constructor() { }

  ngOnInit(): void {
    console.log(this.ejecucionPresupuestal);
    this.totalPresupuestal = {
      totalComprometido: 0,
      facturadoAntesImpuestos: 0,
      saldo: 0,
      porcentajeEjecucionPresupuestal: 0
    };
    this.totalFinanciera = {
      totalComprometido: 0,
      facturadoAntesImpuestos: 0,
      saldo: 0,
      porcentajeEjecucionPresupuestal: 0
    };
    this.loadTableData();
  }

  loadTableData() {
    if (this.ejecucionPresupuestal.length > 0) {
      this.dataTablePresupuestal = this.ejecucionPresupuestal[0].dataTableEjpresupuestal;
      this.dataTablePresupuestal.forEach(element => {
        this.totalPresupuestal.totalComprometido += element.totalComprometido;
        this.totalPresupuestal.facturadoAntesImpuestos = this.totalPresupuestal.facturadoAntesImpuestos + element.facturadoAntesImpuestos;
        this.totalPresupuestal.saldo = this.totalPresupuestal.saldo + element.saldo;
        this.totalPresupuestal.porcentajeEjecucionPresupuestal =
        this.totalPresupuestal.porcentajeEjecucionPresupuestal + element.porcentajeEjecucionPresupuestal;
      });
      if (this.totalPresupuestal.porcentajeEjecucionPresupuestal > 0)
        this.totalPresupuestal.porcentajeEjecucionPresupuestal = this.totalPresupuestal.porcentajeEjecucionPresupuestal / 2;
      //financiera
      this.dataTableFinanciera = this.ejecucionPresupuestal[0].dataTableEjfinanciera;
      this.dataTableFinanciera.forEach(element => {
        this.totalFinanciera.totalComprometido += element.totalComprometido;
        this.totalFinanciera.facturadoAntesImpuestos = this.totalFinanciera.facturadoAntesImpuestos + element.facturadoAntesImpuestos;
        this.totalFinanciera.saldo = this.totalFinanciera.saldo + element.saldo;
        this.totalFinanciera.porcentajeEjecucionPresupuestal =
        this.totalFinanciera.porcentajeEjecucionPresupuestal + element.porcentajeEjecucionPresupuestal;
      });
      if (this.totalFinanciera.porcentajeEjecucionPresupuestal > 0)
        this.totalFinanciera.porcentajeEjecucionPresupuestal = this.totalFinanciera.porcentajeEjecucionPresupuestal / 2;

      }
    this.loadDataSource();
  }

  loadDataSource() {
    this.dataSourcePresupuestal = new MatTableDataSource(this.dataTablePresupuestal);
    this.dataSourcePresupuestal.sort = this.sort;

    this.dataSourceFinanciera = new MatTableDataSource(this.dataTableFinanciera);
    this.dataSourceFinanciera.sort = this.sort;
  }

}
