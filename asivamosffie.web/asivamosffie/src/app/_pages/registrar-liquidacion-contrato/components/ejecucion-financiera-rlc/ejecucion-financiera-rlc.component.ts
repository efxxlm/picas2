import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-ejecucion-financiera-rlc',
  templateUrl: './ejecucion-financiera-rlc.component.html',
  styleUrls: ['./ejecucion-financiera-rlc.component.scss']
})
export class EjecucionFinancieraRlcComponent implements OnInit {
  proyectoId: number;
  dataTable: any[] = [];
  data : any;
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
