import { Component, Input, OnInit } from '@angular/core';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-ejecucion-financiera-gbftrec',
  templateUrl: './ejecucion-financiera-gbftrec.component.html',
  styleUrls: ['./ejecucion-financiera-gbftrec.component.scss']
})
export class EjecucionFinancieraGbftrecComponent implements OnInit {

  @Input() id: number;
  dataTableEjpresupuestal: any[] = [];
  dataTableEjfinanciera: any[] = [];

  vContratoPagosRealizados: any;
  constructor(
    private financialBalanceService: FinancialBalanceService,
  ) { }

  ngOnInit(): void {
    this.getEjecucionFinancieraXProyectoId();
  }
  
  getEjecucionFinancieraXProyectoId() {
    this.financialBalanceService.getEjecucionFinancieraXProyectoId(this.id).subscribe(data => {
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
    });
  }

}
