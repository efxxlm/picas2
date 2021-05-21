import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-ejecucion-financiera-gtlc',
  templateUrl: './ejecucion-financiera-gtlc.component.html',
  styleUrls: ['./ejecucion-financiera-gtlc.component.scss']
})
export class EjecucionFinancieraGtlcComponent implements OnInit {

  proyectoId: number;
  dataTable: any[] = [];
  data : any;

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

}
