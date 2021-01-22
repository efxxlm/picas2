import { Respuesta } from './../common/common.service';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RegistrarRequisitosPagoService {

  private apiUrl = `${ environment.apiUrl }/RegisterValidatePaymentRequierements`;

  constructor( private http: HttpClient ) { }

  getContratos( pTipoSolicitud: string, pModalidadContrato: string, pNumeroContrato: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato?pTipoSolicitud=${ pTipoSolicitud }&pModalidadContrato=${ pModalidadContrato }&pNumeroContrato=${ pNumeroContrato }` );
  }

  getContratoByContratoId( pContratoId: number ) {
    return this.http.get( `${ this.apiUrl }/GetContratoByContratoId?pContratoId=${ pContratoId }` );
  }

  getProyectosByIdContrato( pContratoId: number ) {
    return this.http.get( `${ this.apiUrl }/GetProyectosByIdContrato?pContratoId=${ pContratoId }` );
  }

  getCriterioByFormaPagoCodigo( pFormaPagoCodigo: string ) {
    return this.http.get<{ codigo: string, nombre: string }[]>( `${ this.apiUrl }/GetCriterioByFormaPagoCodigo?pFormaPagoCodigo=${ pFormaPagoCodigo }` );
  }

  createEditNewPayment( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditNewPayment`, pSolicitudPago );
  }

  getListSolicitudPago() {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListSolicitudPago` );
  }

  getTipoPagoByCriterioCodigo( pCriterioCodigo: string ) {
    return new Promise<any[]>( resolve => {
      this.http.get<any[]>( `${ this.apiUrl }/GetTipoPagoByCriterioCodigo?pCriterioCodigo=${ pCriterioCodigo }` )
        .subscribe(
          response => {
            resolve( response );
          }
        );
    } );
  }

  getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPagoCodigo: string ) {
    return new Promise<any[]>( resolve => {
      this.http.get<any[]>( `${ this.apiUrl }/GetConceptoPagoCriterioCodigoByTipoPagoCodigo?TipoPagoCodigo=${ tipoPagoCodigo }` )
        .subscribe(
          response => {
            resolve( response );
          }
        );
    } );
  }

  deleteSolicitudPagoFaseCriterio( pSolicitudPagoFaseCriterioId: number ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/DeleteSolicitudPagoFaseCriterio?pSolicitudPagoFaseCriterioId=${ pSolicitudPagoFaseCriterioId }`, '' );
  }

  deleteSolicitudPagoFaseCriterioProyecto( SolicitudPagoFaseCriterioProyectoId: number ) {
    return this.http.post( `${ this.apiUrl }/DeleteSolicitudPagoFaseCriterioProyecto?SolicitudPagoFaseCriterioProyectoId=${ SolicitudPagoFaseCriterioProyectoId }`, '' );
  }

}
