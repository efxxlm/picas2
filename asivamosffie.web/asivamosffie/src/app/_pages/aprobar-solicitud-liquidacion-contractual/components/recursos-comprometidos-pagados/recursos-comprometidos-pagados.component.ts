import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-recursos-comprometidos-pagados',
  templateUrl: './recursos-comprometidos-pagados.component.html',
  styleUrls: ['./recursos-comprometidos-pagados.component.scss']
})
export class RecursosComprometidosPagadosComponent implements OnInit {
  
  proyectoId: number;
  contratoObra: any;
  contratoInterventoria: any;
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
    this.getContratoByProyectoId();
    this.getBalanceByProyectoId(this.proyectoId);
  }
  

  getContratoByProyectoId() {
    this.financialBalanceService.getContratoByProyectoId(this.proyectoId).subscribe(data => {
      data.forEach(element => {
        if(element.tipoSolicitudCodigo === '1'){
          this.contratoObra = element.contrato;
        }
        if(element.tipoSolicitudCodigo === '2'){
          this.contratoInterventoria = element.contrato;
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
