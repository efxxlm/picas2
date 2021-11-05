import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-detalle-balance-financiero-rlc',
  templateUrl: './detalle-balance-financiero-rlc.component.html',
  styleUrls: ['./detalle-balance-financiero-rlc.component.scss']
})
export class DetalleBalanceFinancieroRlcComponent implements OnInit {
  estaEditando = false;
  contratacionId: number;
  balanceFinancieroId: number;//definir
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;
  data : any;
  proyectoId: number;
  cumpleCondicionesTai: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private financialBalanceService: FinancialBalanceService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionId = params.id;
      this.proyectoId = params.proyectoId;
    });
  }
  ngOnInit(): void {
    this.getBalanceByProyectoId(this.proyectoId);
  }

  getBalanceByProyectoId(proyectoId: number) {
    this.financialBalanceService.getDataByProyectoId(proyectoId)
    .subscribe( getDataByProyectoId => {
        if( getDataByProyectoId.length > 0 ){
            this.data = getDataByProyectoId[0];
            if(this.data != null){
              this.cumpleCondicionesTai = this.data.cumpleCondicionesTai;
              if(this.data.balanceFinanciero.length > 0)
                this.balanceFinancieroId = this.data.balanceFinanciero[0].balanceFinancieroId;
            }
        }
    });
  }

  irRecursosComprometidos(){
    this.router.navigate([`${ this.router.url }/recursosComprometidos`]);
  }
  verEjecucionFinanciera(){
    this.router.navigate([`${ this.router.url }/ejecucionFinanciera`]);
  }
  verTrasladoRecursos(){
    this.router.navigate([`${ this.router.url }/trasladoRecursos`]);
  }
  verLiberacionSaldo(){
    this.router.navigate([`${ this.router.url }/liberacionSaldo`]);
  }

}
