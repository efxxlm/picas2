import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-detalle-balance-financiero-rlc',
  templateUrl: './detalle-balance-financiero-rlc.component.html',
  styleUrls: ['./detalle-balance-financiero-rlc.component.scss']
})
export class DetalleBalanceFinancieroRlcComponent implements OnInit {
  idBalance: any;
  constructor(private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      console.log(param.id);
      this.idBalance = param.id;
    });
  }
  irRecursosComprometidos(id){
    this.router.navigate(['/registrarLiquidacionContrato/recursosComprometidos', id]);
  }
  verEjecucionFinanciera(id){
    this.router.navigate(['/registrarLiquidacionContrato/ejecucionFinanciera', id]);
  }
  verTrasladoRecursos(id){
    this.router.navigate(['/registrarLiquidacionContrato/trasladoRecursos', id]);
  }
}
