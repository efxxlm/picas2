import { Component, Input, OnInit } from '@angular/core';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-recursos-comprom-pagados-gbftrec',
  templateUrl: './recursos-comprom-pagados-gbftrec.component.html',
  styleUrls: ['./recursos-comprom-pagados-gbftrec.component.scss']
})
export class RecursosCompromPagadosGbftrecComponent implements OnInit {

  @Input() id: number;
  @Input() tieneOrdenGiro: boolean;
  contratoObra: any;
  contratoInterventoria: any;

  constructor(
    private financialBalanceService: FinancialBalanceService,
  ) { }

  ngOnInit(): void {
    this.getContratoByProyectoId();
  }
  
  getContratoByProyectoId() {
    this.financialBalanceService.getContratoByProyectoId(this.id).subscribe(data => {
      data.forEach(element => {
        if(element.tipoSolicitudCodigo === '1'){
          this.contratoObra = element.contrato
          this.contratoObra.tablaOrdenGiroValorTotal = element.tablaOrdenGiroValorTotal
        }
        if(element.tipoSolicitudCodigo === '2'){
          this.contratoInterventoria = element.contrato
          this.contratoInterventoria.tablaOrdenGiroValorTotal = element.tablaOrdenGiroValorTotal
        }
      });  
    });
  }
}

