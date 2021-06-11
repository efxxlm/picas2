import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-aprobar-balance',
  templateUrl: './aprobar-balance.component.html',
  styleUrls: ['./aprobar-balance.component.scss']
})
export class AprobarBalanceComponent implements OnInit {

  contratacionId: number;
  balanceFinancieroId: number;//definir
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;
  proyectoId: number;
  data : any;

  constructor(
    private routes: Router,
    private route: ActivatedRoute,
    private financialBalanceService: FinancialBalanceService

  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionId = params.id;
      this.proyectoId = params.proyectoId;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'aprobarBalance' ) {
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
        this.routes.navigate(['/aprobarSolicitudLiquidacionContractual/', urlSegment.path, this.contratacionId ]);
        return;
      }
    });
  }

  getBalanceByProyectoId(proyectoId: number) {
    this.financialBalanceService.getDataByProyectoId(proyectoId)
    .subscribe( getDataByProyectoId => {
        if( getDataByProyectoId.length > 0 ){
            this.data = getDataByProyectoId[0];
            if(this.data != null){
              if(this.data.balanceFinanciero.length > 0)
                this.balanceFinancieroId = this.data.balanceFinanciero[0].balanceFinancieroId;
            }
        }
    });
  }

}
