import { Component, Input, OnInit } from '@angular/core';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-recursos-compro-pagados',
  templateUrl: './recursos-compro-pagados.component.html',
  styleUrls: ['./recursos-compro-pagados.component.scss']
})
export class RecursosComproPagadosComponent implements OnInit {
  @Input() proyectoId: number;
  contratoObra: any;
  contratoInterventoria: any;
  tablaOrdenGiroValorTotalObra: any;
  tablaOrdenGiroValorTotalInterventoria: any;
  data : any;

  constructor(
    private financialBalanceService: FinancialBalanceService,
  ) {
  }

  ngOnInit(): void {
    this.getContratoByProyectoId();
    console.log(this.proyectoId);
    this.getBalanceByProyectoId(this.proyectoId);
  }

  getContratoByProyectoId() {
    this.financialBalanceService.getContratoByProyectoId(this.proyectoId).subscribe(data => {
      data.forEach(element => {
        if(element.tipoSolicitudCodigo === '1'){
          this.contratoObra = element.contrato;
          this.tablaOrdenGiroValorTotalObra = element.tablaOrdenGiroValorTotal;
        }
        if(element.tipoSolicitudCodigo === '2'){
          this.contratoInterventoria = element.contrato;
          this.tablaOrdenGiroValorTotalInterventoria = element.tablaOrdenGiroValorTotal;
        }
      });
    });
  }

  getBalanceByProyectoId(proyectoId: number) {
    this.financialBalanceService.getDataByProyectoId(proyectoId)
    .subscribe( getDataByProyectoId => {
        if( getDataByProyectoId.length > 0 ){
            this.data = getDataByProyectoId[0];
        }
    });
  }
}
