import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-ver-ejecucion-financiera',
  templateUrl: './ver-ejecucion-financiera.component.html',
  styleUrls: ['./ver-ejecucion-financiera.component.scss']
})
export class VerEjecucionFinancieraComponent implements OnInit {

  proyectoId: number;
  dataTable: any[] = [];
  data : any;
  dataTablaFinanciera : any[] = [];
  vContratoPagosRealizados: any;
  total: any;
  listaejecucionPresupuestal: any[] = [];
  dataTableEjpresupuestal: any[] = [];
  dataTableEjfinanciera: any[] = [];

  constructor(
    private financialBalanceService: FinancialBalanceService,
    private route: ActivatedRoute,
  ) {
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.proyectoId;
    });
  }

  ngOnInit(): void {
    this.getBalanceByProyectoId(this.proyectoId);
    this.getContratoByProyectoId();
    this.getEjecucionFinancieraXProyectoId(this.proyectoId);
  }

  getBalanceByProyectoId(proyectoId: number) {
    this.financialBalanceService.getDataByProyectoId(proyectoId)
    .subscribe( getDataByProyectoId => {
        if( getDataByProyectoId.length > 0 ){
            this.data = getDataByProyectoId[0];
        }
    });
  }

  getContratoByProyectoId() {
    this.financialBalanceService.getContratoByProyectoId(this.proyectoId).subscribe(data => {
      data.forEach(element => {
        this.dataTable.push({
          vContratoPagosRealizados: element.contrato.vContratoPagosRealizados,
          tipoSolicitudCodigo: element.tipoSolicitudCodigo
        });
        this.listaejecucionPresupuestal.push({
          componente: element.tipoSolicitudCodigo === '1' ? 'Obra' : element.tipoSolicitudCodigo === '2' ? 'Interventoria' : '',
          totalComprometido: element.contrato.vContratoPagosRealizados.valorSolicitud != null ? element.contrato.vContratoPagosRealizados.valorSolicitud: 0,
          facturadoAntesdeImpuestos: 0,
          saldo: element.contrato.vContratoPagosRealizados.saldoPorPagar != null ? element.contrato.vContratoPagosRealizados.saldoPorPagar: 0,
          porcentajeEjecucionPresupuestal: element.contrato.vContratoPagosRealizados.porcentajeFacturado != null ? element.contrato.vContratoPagosRealizados.porcentajeFacturado : 0,
        });
      });
    });
  }

  getEjecucionFinancieraXProyectoId(proyectoId: number) {
    this.financialBalanceService.getEjecucionFinancieraXProyectoId(proyectoId).subscribe(data => {
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
          saldo: element.saldo,
          totalComprometido: element.totalComprometido
        });
      });
    });
  }

}
