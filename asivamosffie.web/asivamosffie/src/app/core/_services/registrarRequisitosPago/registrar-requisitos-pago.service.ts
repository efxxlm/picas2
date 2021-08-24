import { Dominio } from 'src/app/core/_services/common/common.service';
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

  getContratoByContratoId( pContratoId: number, pSolicitudPago: number ) {
    return this.http.get( `${ this.apiUrl }/GetContratoByContratoId?pContratoId=${ pContratoId }&pSolicitudPago=${ pSolicitudPago }` );
  }

  returnSolicitudPago( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/ReturnSolicitudPago`, pSolicitudPago );
  }

  getSolicitudPago( pSolicitudPagoId: number ) {
    return this.http.get( `${ this.apiUrl }/GetSolicitudPago?pSolicitudPagoId=${ pSolicitudPagoId }` );
  }

  getProyectosByIdContrato( pContratoId: number ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetProyectosByIdContrato?pContratoId=${ pContratoId }` );
  }

  getFormaPagoCodigoByFase( pEsPreconstruccion: string, pContratoId: number) {
    return this.http.get<Dominio[]>( `${ this.apiUrl }/GetFormaPagoCodigoByFase?pEsPreconstruccion=${ pEsPreconstruccion }&pContratoId=${ pContratoId }` );
  }

  getMontoMaximo( solicitudPagoId: number, esPreConstruccion: string ) {
    return this.http.get<any>( `${ this.apiUrl }/GetMontoMaximo?SolicitudPagoId=${ solicitudPagoId }&EsPreConstruccion=${ esPreConstruccion }` )
  }

  getMontoMaximoMontoPendiente( SolicitudPagoId: number, strFormaPago: string, EsPreConstruccion: string, contratacionProyectoId: number, pCriterioCodigo: string, pConceptoCodigo: string ) {
    return this.http.get<{ montoMaximo: number, valorPendientePorPagar: number }>( `${ this.apiUrl }/GetMontoMaximoMontoPendiente?SolicitudPagoId=${ SolicitudPagoId }&strFormaPago=${ strFormaPago }&EsPreConstruccion=${ EsPreConstruccion }&pContratacionProyectoId=${ contratacionProyectoId }&pCriterioCodigo=${ pCriterioCodigo }&pConceptoCodigo=${ pConceptoCodigo }` );
  }

  getCriterioByFormaPagoCodigo( pFormaPagoCodigo: string ) {
    return this.http.get<{ codigo: string, nombre: string, porcentaje: number }[]>( `${ this.apiUrl }/GetCriterioByFormaPagoCodigo?pFormaPagoCodigo=${ pFormaPagoCodigo }` );
  }

  getValidateSolicitudPagoId( pSolicitudPagoId: number ) {
    return this.http.get( `${ this.apiUrl }/GetValidateSolicitudPagoId?pSolicitudPagoId=${ pSolicitudPagoId }` );
  }

  createEditNewPayment( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditNewPayment`, pSolicitudPago );
  }

  createEditExpensas( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditExpensas`, pSolicitudPago );
  }

  createEditOtrosCostosServicios( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditOtrosCostosServicios`, pSolicitudPago );
  }

  getListSolicitudPago() {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListSolicitudPago` );
  }

  getTipoPagoByCriterioCodigo( pCriterioCodigo: string ) {
    return new Promise<any[]>( resolve => {
      this.http.get<any[]>( `${ this.apiUrl }/GetTipoPagoByCriterioCodigo?pCriterioCodigo=${ pCriterioCodigo }` )
        .subscribe( response => resolve( response ) );
    } );
  }

  getConceptoPagoCriterioCodigoByTipoPagoCodigo( tipoPagoCodigo: string ) {
    return new Promise<any[]>( resolve => {
      this.http.get<any[]>( `${ this.apiUrl }/GetConceptoPagoCriterioCodigoByTipoPagoCodigo?TipoPagoCodigo=${ tipoPagoCodigo }` )
        .subscribe( response => resolve( response ) );
    } );
  }

  getUsoByConceptoPagoCriterioCodigo( pConceptoPagoCodigo: string, pContratoId: number ) {
    return new Promise<any[]>( resolve => {
      this.http.get( `${ this.apiUrl }/GetUsoByConceptoPagoCriterioCodigo?pConceptoPagoCodigo=${ pConceptoPagoCodigo }&pContratoId=${ pContratoId }` )
        .subscribe( ( response: any[] ) => resolve( response ) );
    } );
  }

  getMontoMaximoProyecto( pContrato: number, pContratacionProyectoId: number, esPreConstruccion: string ) {
    return new Promise<{ valorMaximoProyecto: number, valorPendienteProyecto:number }>( resolve => {
      this.http.get<{ valorMaximoProyecto: number, valorPendienteProyecto:number }>( `${ this.apiUrl }/GetMontoMaximoProyecto?pContrato=${ pContrato }&pContratacionProyectoId=${ pContratacionProyectoId }&EsPreConstruccion=${ esPreConstruccion }` )
        .subscribe( response => resolve( response ) );
    } );
  }

  getListProyectosByLlaveMen( pLlaveMen: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListProyectosByLlaveMen?pLlaveMen=${ pLlaveMen }` );
  }
  // Eliminar criterio del contrato
  deleteSolicitudPagoFaseCriterio( pSolicitudPagoFaseCriterioId: number ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/DeleteSolicitudPagoFaseCriterio?pSolicitudPagoFaseCriterioId=${ pSolicitudPagoFaseCriterioId }`, '' );
  }
  // Eliminar criterio del proyecto
  deleteSolicitudPagoFaseCriterioProyecto( SolicitudPagoFaseCriterioProyectoId: number ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/DeleteSolicitudPagoFaseCriterioProyecto?SolicitudPagoFaseCriterioProyectoId=${ SolicitudPagoFaseCriterioProyectoId }`, '' );
  }
  // Eliminar llave del proyecto
  deleteSolicitudLlaveCriterioProyecto( pContratacionProyectoId: number ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/DeleteSolicitudLlaveCriterioProyecto?pContratacionProyectoId=${ pContratacionProyectoId }`, '' );
  }
  // Eliminar solicitud de pago
  deleteSolicitudPago( pSolicitudPagoId: number ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/DeleteSolicitudPago?pSolicitudPagoId=${ pSolicitudPagoId }`, '' );
  }
  // Eliminar descuentos
  deleteSolicitudPagoFaseFacturaDescuento( pSolicitudPagoFaseFacturaDescuentoId: number ) {
    return this.http.post( `${ this.apiUrl }/DeleteSolicitudPagoFaseFacturaDescuento?pSolicitudPagoFaseFacturaDescuentoId=${ pSolicitudPagoFaseFacturaDescuentoId }`, '' );
  }

}
