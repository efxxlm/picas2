import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-validar-balance',
  templateUrl: './validar-balance.component.html',
  styleUrls: ['./validar-balance.component.scss']
})
export class ValidarBalanceComponent implements OnInit {

  contratacionProyectoId: number;
  proyectoId: number;
  data : any;
  balanceFinancieroId: number;//definir
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;

  constructor(
    private routes: Router,
    private route: ActivatedRoute,
    private financialBalanceService: FinancialBalanceService
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
      this.proyectoId = params.proyectoId;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'validarBalance' ) {
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
        this.routes.navigate(['/registrarSolicitudLiquidacionContractual/', urlSegment.path, this.contratacionProyectoId ]);
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
