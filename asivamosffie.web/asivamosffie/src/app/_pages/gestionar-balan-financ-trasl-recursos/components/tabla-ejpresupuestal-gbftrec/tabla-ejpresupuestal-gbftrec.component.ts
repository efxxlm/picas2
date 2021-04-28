import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-ejpresupuestal-gbftrec',
  templateUrl: './tabla-ejpresupuestal-gbftrec.component.html',
  styleUrls: ['./tabla-ejpresupuestal-gbftrec.component.scss']
})
export class TablaEjpresupuestalGbftrecComponent implements OnInit {
  @Input() data : any[] = [];

  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'componente',
    'totalComprometido',
    'facturadoAntesdeImpuestos',
    'saldo',
    'porcentajeEjecucionPresupuestal'
  ];
  dataTable: any[] = [];
  total: any;



  constructor() { }

  ngOnInit(): void {
    if(this.data.length > 0){
      console.log(this.data);
      this.data.forEach(element => {
        if(element.vContratoPagosRealizados.length > 0){
          this.dataTable.push({
            componente: element.tipoSolicitudCodigo === '1' ? 'Obra' : element.tipoSolicitudCodigo === '2' ? 'Interventoria' : '',
            totalComprometido: element.vContratoPagosRealizados[0].valorSolicitud != null ? element.vContratoPagosRealizados[0].valorSolicitud: 0,
            facturadoAntesdeImpuestos: 0,
            saldo: element.vContratoPagosRealizados[0].saldoPorPagar != null ? element.vContratoPagosRealizados[0].saldoPorPagar: 0,
            porcentajeEjecucionPresupuestal: element.vContratoPagosRealizados[0].porcentajeFacturado != null ? element.vContratoPagosRealizados[0].porcentajeFacturado : 0,
          });
        }
      });
      this.total = {
        totalComprometido: 0,
        facturadoAntesdeImpuestos: 0,
        saldo: 0,
        porcentajeEjecucionPresupuestal: 0       
      }
      this.dataTable.forEach(element => {
        this.total.totalComprometido += element.totalComprometido;
        this.total.facturadoAntesdeImpuestos = this.total.facturadoAntesdeImpuestos + element.facturadoAntesdeImpuestos;
        this.total.saldo = this.total.saldo + element.saldo;
        this.total.porcentajeEjecucionPresupuestal = this.total.porcentajeEjecucionPresupuestal + element.porcentajeEjecucionPresupuestal;
      });
    }
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }

}
