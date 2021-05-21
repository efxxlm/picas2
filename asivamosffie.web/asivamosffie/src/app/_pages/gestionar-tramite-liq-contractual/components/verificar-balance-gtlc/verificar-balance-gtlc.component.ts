import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-verificar-balance-gtlc',
  templateUrl: './verificar-balance-gtlc.component.html',
  styleUrls: ['./verificar-balance-gtlc.component.scss']
})
export class VerificarBalanceGtlcComponent implements OnInit {
  estaEditando = false;
  contratacionProyectoId: number;
  balanceFinancieroId: number;//definir
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;
  data : any;
  proyectoId: number;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private financialBalanceService: FinancialBalanceService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
      this.proyectoId = params.proyectoId;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'verificarBalance' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditarBalance' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalleBalance' ) {
          this.esVerDetalle = true;
          return;
      }
    });
  }
  ngOnInit(): void {
    this.getBalanceByProyectoId(this.proyectoId);
  }  

  redirectToParent(): void{
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if(urlSegment.path.includes("Requisitos")){
        this.router.navigate(['/gestionarTramiteLiquidacionContractual/', urlSegment.path, this.contratacionProyectoId ]);
        return;
      }
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
