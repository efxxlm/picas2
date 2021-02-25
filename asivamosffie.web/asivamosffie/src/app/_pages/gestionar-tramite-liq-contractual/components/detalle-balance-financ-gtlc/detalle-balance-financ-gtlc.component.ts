import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-detalle-balance-financ-gtlc',
  templateUrl: './detalle-balance-financ-gtlc.component.html',
  styleUrls: ['./detalle-balance-financ-gtlc.component.scss']
})
export class DetalleBalanceFinancGtlcComponent implements OnInit {
  idBalance: any;
  constructor(private router: Router,private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
       this.activatedRoute.params.subscribe(param => {
      console.log(param.id);
      this.idBalance = param.id;
    });
  }
  irRecursosComprometidos(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/recursosComprometidos', id]);
  }
  verEjecucionFinanciera(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/ejecucionFinanciera', id]);
  }
  verTrasladoRecursos(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/trasladoRecursos', id]);
  }
}
