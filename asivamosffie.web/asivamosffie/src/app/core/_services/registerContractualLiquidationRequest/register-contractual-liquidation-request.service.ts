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

  getListContractualLiquidationObra(){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridRegisterContractualLiquidationObra`);
  }

  getListContractualLiquidationInterventoria(){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridRegisterContractualLiquidationInterventoria`);
  }

  gridInformeFinal( pContratacionProyectoId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridInformeFinal?pContratacionProyectoId=${ pContratacionProyectoId }`);
  }

  getInformeFinalByProyectoId( pProyectoId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetInformeFinalByProyectoId?pProyectoId=${ pProyectoId }`);
  }

  getInformeFinalAnexoByInformeFinalId( pInformeFinalId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetInformeFinalAnexoByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
  }

  getObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId( pMenuId: number, pContratacionProyectoId: number, pPadreId: number, pTipoObservacionCodigo: string ) {
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/getObservacionLiquidacionContratacionByMenuIdAndContratacionProyectoId?pMenuId=${ pMenuId }&pContratacionProyectoId=${ pContratacionProyectoId }&pPadreId=${ pPadreId }&pTipoObservacionCodigo=${ pTipoObservacionCodigo }` );
  }

  createUpdateLiquidacionContratacionObservacion( pLiquidacionContratacionObservacion: any ) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/${this.contractual_liquidation}/CreateUpdateLiquidacionContratacionObservacion`, pLiquidacionContratacionObservacion);
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
