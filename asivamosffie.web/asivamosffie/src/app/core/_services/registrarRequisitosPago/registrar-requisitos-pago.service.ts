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
    return this.http.get( `${ this.apiUrl }/GetCriterioByFormaPagoCodigo?pFormaPagoCodigo=${ pFormaPagoCodigo }` );
  }

  createEditNewPayment( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditNewPayment`, pSolicitudPago );
  }

  getListSolicitudPago() {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListSolicitudPago` );
  }

}
