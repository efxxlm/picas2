import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Dominio, Respuesta } from '../common/common.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RegisterContractualLiquidationRequestService {
  
  constructor( private http: HttpClient ) { }

  contractual_liquidation = 'RegisterContractualLiquidationRequest';

  getListContractualLiquidationObra(pMenuId: number){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridRegisterContractualLiquidationObra?pMenuId=${ pMenuId }`);
  }

  getListContractualLiquidationInterventoria(pMenuId: number){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridRegisterContractualLiquidationInterventoria?pMenuId=${ pMenuId }`);
  }
  
  getContratoPoliza(pContratoPolizaId: number, pMenuId: number, pContratacionProyectoId: number){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetContratoPoliza?pContratoPolizaId=${ pContratoPolizaId }&pMenuId=${ pMenuId }&pContratacionProyectoId=${ pContratacionProyectoId }`);
  }

  getContratacionProyectoByContratacionProyectoId(pContratacionProyectoId: number){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetContratacionProyectoByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }`);
  }
  
  gridInformeFinal( pContratacionProyectoId: number, pMenuId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridInformeFinal?pContratacionProyectoId=${ pContratacionProyectoId }&pMenuId=${ pMenuId }`);
  }

  getBalanceByContratacionProyectoId( pContratacionProyectoId: number, pMenuId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetBalanceByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }&pMenuId=${ pMenuId }`);
  }

  getInformeFinalByProyectoId( pProyectoId: number, pContratacionProyectoId: number, pMenuId: number){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetInformeFinalByProyectoId?pProyectoId=${ pProyectoId }&pContratacionProyectoId=${ pContratacionProyectoId }&pMenuId=${ pMenuId }`);
  }

  getInformeFinalAnexoByInformeFinalId( pInformeFinalId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetInformeFinalAnexoByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
  }

  getObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId( pMenuId: number, pContratacionProyectoId: number, pPadreId: number, pTipoObservacionCodigo: string ) {
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId?pMenuId=${ pMenuId }&pContratacionProyectoId=${ pContratacionProyectoId }&pPadreId=${ pPadreId }&pTipoObservacionCodigo=${ pTipoObservacionCodigo }` );
  }

  getHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId( pMenuId: number, pContratacionProyectoId: number, pPadreId: number, pTipoObservacionCodigo: string ) {
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetHistoricoObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId?pMenuId=${ pMenuId }&pContratacionProyectoId=${ pContratacionProyectoId }&pPadreId=${ pPadreId }&pTipoObservacionCodigo=${ pTipoObservacionCodigo }` );
  }

  createUpdateLiquidacionContratacionObservacion( pLiquidacionContratacionObservacion: any ) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/${this.contractual_liquidation}/CreateUpdateLiquidacionContratacionObservacion`, pLiquidacionContratacionObservacion);
  }

  changeStatusLiquidacionContratacionProyecto( pContratacionProyecto: any , menuId: number) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/${this.contractual_liquidation}/ChangeStatusLiquidacionContratacionProyecto?menuId=${ menuId }`, pContratacionProyecto);
  }


  listaTipoObservacionLiquidacionContratacion() {
    /*
      Get lista de tipos de observaciones para los CU => 5.1.6, 5.1.7,5.1.8 
    */
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=184`)
      .pipe(
        map( listaTipoObservacion => {
          const tiposObservaciones: any = {};
          listaTipoObservacion.forEach( tipoObservacion => {
            if ( tipoObservacion.codigo === '1' ) {
              tiposObservaciones.actualizacionPoliza = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '2' ) {
              tiposObservaciones.balanceFinanciero = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '3' ) {
              tiposObservaciones.informeFinal = tipoObservacion.codigo
            }
          });
          return tiposObservaciones;
        })
      );
  }

  listaMenu() {
    return this.http.get<any[]>(`${environment.apiUrl}/Common/GetListMenu`)
      .pipe(
        map( menus => {
          const menusId: any = {};
          menus.forEach( menu => {
            if ( menu.menuId === 88 ) {
              menusId.registrarSolicitudLiquidacionContratacion = menu.menuId
            }
            if ( menu.menuId === 89 ) {
              menusId.aprobarSolicitudLiquidacionContratacion = menu.menuId
            }
            if ( menu.menuId === 90 ) {
              menusId.gestionarSolicitudLiquidacionContratacion = menu.menuId
            }
          });
          return menusId;
        })
      );
  }
}
