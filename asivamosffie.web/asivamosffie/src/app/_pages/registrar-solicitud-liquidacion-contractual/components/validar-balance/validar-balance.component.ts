import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';

@Component({
  selector: 'app-validar-balance',
  templateUrl: './validar-balance.component.html',
  styleUrls: ['./validar-balance.component.scss']
})
export class ValidarBalanceComponent implements OnInit {

  contratacionProyectoId: number;
  balanceFinancieroId: number;//definir
  listaTipoObservacionLiquidacionContratacion: any;
  listaMenu: any;

  constructor(
    private routes: Router,
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
}
