import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-verificar-balance-gtlc',
  templateUrl: './verificar-balance-gtlc.component.html',
  styleUrls: ['./verificar-balance-gtlc.component.scss']
})
export class VerificarBalanceGtlcComponent implements OnInit {
  estaEditando = false;
  contratacionProyectoId: number;
  balanceFinancieroId: number;//definir
  listaTipoObservacionLiquidacionContratacion: any;
  listaMenu: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
    });
  }
  ngOnInit(): void {
    this.registerContractualLiquidationRequestService.listaTipoObservacionLiquidacionContratacion()
    .subscribe( response => {
        this.listaTipoObservacionLiquidacionContratacion = response;
    });
    this.registerContractualLiquidationRequestService.listaMenu()
    .subscribe( response => {
        this.listaMenu = response;
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
