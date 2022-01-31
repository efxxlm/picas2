import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-seguimiento-financiero',
  templateUrl: './seguimiento-financiero.component.html',
  styleUrls: ['./seguimiento-financiero.component.scss']
})
export class SeguimientoFinancieroComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  proyectoId: number;
  dataSeguimiento: any = null;
  infoProyecto: any = null;
  openAcordeon = false;
  displayedColumnsEjPresupuestal: string[] = [ 'nombre', 'totalComprometido', 'facturadoAntesImpuestos', 'saldo', 'porcentajeEjecucionPresupuestal' ];
  displayedColumnsEjFinanciera: string[] = [ 'nombre', 'totalComprometido', 'ordenadoGirarAntesImpuestos', 'saldo', 'porcentajeEjecucionFinanciera' ];
  dataTableEjpresupuestal: any[] = [];
  dataTableEjfinanciera: any[] = [];
  dataSourceEjPresupuestal = new MatTableDataSource();
  dataSourceEjFinanciera = new MatTableDataSource();

  tablaEjecucionFinanciera = [
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
  totalEjFinanciera = {
    totalComprometido: 0,
    ordenadoGirarAntesImpuestos: 0,
    saldo: 0,
    porcentajeEjecucionFinanciera: 0
  };
  totalEjPresupuestal = {
    totalComprometido: 0,
    facturadoAntesImpuestos: 0,
    saldo: 0,
    porcentajeEjecucionPresupuestal: 0
  };

  constructor(
    private route: ActivatedRoute,
    private fichaProyectoService: FichaProyectoService,
    private financialBalanceService: FinancialBalanceService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.id;
    });
  }

  ngOnInit(): void {
    this.fichaProyectoService.getFlujoProyectoByProyectoId(this.proyectoId)
    .subscribe(response => {
      this.infoProyecto = response;
    });
      this.financialBalanceService.getEjecucionFinancieraXProyectoId(this.proyectoId).subscribe(data => {
        data[0].forEach(element => {
          this.dataTableEjpresupuestal.push({
            facturadoAntesImpuestos: element.facturadoAntesImpuestos,
            nombre: element.nombre,
            porcentajeEjecucionPresupuestal: element.porcentajeEjecucionPresupuestal,
            proyectoId: element.proyectoId,
            saldo: element.saldo,
            tipoSolicitudCodigo: element.tipoSolicitudCodigo,
            totalComprometido: element.totalComprometido
          });
        });
        data[1].forEach(element => {
          this.dataTableEjfinanciera.push({
            nombre: element.nombre,
            ordenadoGirarAntesImpuestos: element.ordenadoGirarAntesImpuestos,
            porcentajeEjecucionFinanciera: element.porcentajeEjecucionFinanciera,
            proyectoId: element.proyectoId,
            descuento: element.descuento,
            saldo: element.saldo,
            totalComprometido: element.totalComprometido
          });
        });
        if(this.dataTableEjfinanciera.length > 0){
          let concepto;
          this.dataTableEjfinanciera.forEach(element => {
            if (element.nombre === 'Obra') concepto = 0;
            else if (element.nombre === 'Interventoria') concepto = 1;

            if (element.totalComprometido) this.tablaEjecucionFinanciera[concepto].totalComprometido = element.totalComprometido;
            if (element.descuento) this.tablaEjecucionFinanciera[concepto].descuento = element.descuento;
            if (element.saldo) this.tablaEjecucionFinanciera[concepto].saldo = element.saldo;
            this.tablaEjecucionFinanciera[concepto].ordenadoGirarAntesImpuestos += element.ordenadoGirarAntesImpuestos;
            this.tablaEjecucionFinanciera[concepto].porcentajeEjecucionFinanciera = (this.tablaEjecucionFinanciera[concepto].ordenadoGirarAntesImpuestos * 100) / this.tablaEjecucionFinanciera[concepto].totalComprometido;
            this.tablaEjecucionFinanciera[concepto].porcentajeEjecucionFinanciera = this.tablaEjecucionFinanciera[concepto].porcentajeEjecucionFinanciera > 100 ? 100 : this.tablaEjecucionFinanciera[concepto].porcentajeEjecucionFinanciera;

            this.totalEjFinanciera.totalComprometido += this.tablaEjecucionFinanciera[concepto].totalComprometido;
            this.totalEjFinanciera.ordenadoGirarAntesImpuestos = this.totalEjFinanciera.ordenadoGirarAntesImpuestos + this.tablaEjecucionFinanciera[concepto].ordenadoGirarAntesImpuestos;
            this.totalEjFinanciera.saldo = this.totalEjFinanciera.saldo + this.tablaEjecucionFinanciera[concepto].saldo;
            this.totalEjFinanciera.porcentajeEjecucionFinanciera = this.totalEjFinanciera.porcentajeEjecucionFinanciera + this.tablaEjecucionFinanciera[concepto].porcentajeEjecucionFinanciera;

          });
          if (this.totalEjFinanciera.porcentajeEjecucionFinanciera > 0) {
            this.totalEjFinanciera.porcentajeEjecucionFinanciera = this.totalEjFinanciera.porcentajeEjecucionFinanciera / 2;
          }
          this.dataSourceEjFinanciera = new MatTableDataSource(this.tablaEjecucionFinanciera);
          this.dataSourceEjFinanciera.sort = this.sort;

          if(this.dataTableEjpresupuestal.length > 0){
            this.dataTableEjpresupuestal.forEach(element => {
              this.totalEjPresupuestal.totalComprometido += element.totalComprometido;
              this.totalEjPresupuestal.facturadoAntesImpuestos = this.totalEjPresupuestal.facturadoAntesImpuestos + element.facturadoAntesImpuestos;
              this.totalEjPresupuestal.saldo = this.totalEjPresupuestal.saldo + element.saldo;
              this.totalEjPresupuestal.porcentajeEjecucionPresupuestal =
                this.totalEjPresupuestal.porcentajeEjecucionPresupuestal + element.porcentajeEjecucionPresupuestal;
            });
            if (this.totalEjPresupuestal.porcentajeEjecucionPresupuestal > 0)
              this.totalEjPresupuestal.porcentajeEjecucionPresupuestal = this.totalEjPresupuestal.porcentajeEjecucionPresupuestal / 2;
          }

          this.dataSourceEjPresupuestal = new MatTableDataSource(this.dataTableEjpresupuestal);
          this.dataSourceEjPresupuestal.sort = this.sort;
        }
      });
  }

  downloadPDF() {
    this.openAcordeon = true;
    setTimeout(() => {
      document.title='Seguimiento Financiero '+this.dataSeguimiento?.infoProyecto?.llaveMen;
      window.print();
    }, 300);
    window.onafterprint = function(){
      window.location.reload();
    }
  }
}
