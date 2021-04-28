import { Component, Input, OnInit } from '@angular/core';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-ejecucion-financiera-gbftrec',
  templateUrl: './ejecucion-financiera-gbftrec.component.html',
  styleUrls: ['./ejecucion-financiera-gbftrec.component.scss']
})
export class EjecucionFinancieraGbftrecComponent implements OnInit {

  @Input() id: number;
  dataTable: any[] = [];

  vContratoPagosRealizados: any;
  constructor(
    private financialBalanceService: FinancialBalanceService,
  ) { }

  ngOnInit(): void {
    this.getContratoByProyectoId();
  }
  
  getContratoByProyectoId() {
    this.financialBalanceService.getContratoByProyectoId(this.id).subscribe(data => {
      data.forEach(element => {
        this.dataTable.push({
          vContratoPagosRealizados: element.contrato.vContratoPagosRealizados,
          tipoSolicitudCodigo: element.tipoSolicitudCodigo
        });
      });  
    });
  }

}
