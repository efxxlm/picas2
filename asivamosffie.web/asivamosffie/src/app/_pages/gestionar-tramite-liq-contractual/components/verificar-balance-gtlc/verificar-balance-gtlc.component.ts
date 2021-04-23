import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
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


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
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
  }  

  redirectToParent(): void{
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if(urlSegment.path.includes("Requisitos")){
        this.router.navigate(['/gestionarTramiteLiquidacionContractual/', urlSegment.path, this.contratacionProyectoId ]);
        return;
      }
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
