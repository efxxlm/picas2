import { Respuesta } from './../autenticacion/autenticacion.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Dominio } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class ObservacionesMultiplesCuService {

  private apiUrl = `${ environment.apiUrl }/PaymentRequierements`;

  constructor( private http: HttpClient ) { }

  getListSolicitudPago( pMenuId: number ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListSolicitudPago?pMenuId=${ pMenuId }` );
  }

  changueStatusSolicitudPago( pSolicitudPago: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/ChangueStatusSolicitudPago`, pSolicitudPago );
  }

  getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( pMenuId: number, pSolicitudPagoId: number, pPadreId: number ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId?pMenuId=${ pMenuId }&pSolicitudPagoId=${ pSolicitudPagoId }&pPadreId=${ pPadreId }` );
  }

  asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( pMenuId: number, pSolicitudPagoId: number, pPadreId: number ) {
    return new Promise<any[]>( resolve => {
      this.http.get<any[]>( `${ this.apiUrl }/GetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId?pMenuId=${ pMenuId }&pSolicitudPagoId=${ pSolicitudPagoId }&pPadreId=${ pPadreId }` )
        .subscribe( response => resolve( response ) );
    } );
  }

  createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateUpdateSolicitudPagoObservacion`, pSolicitudPagoObservacion );
  }

  listaTipoObservacionSolicitudes() {
    /*
      Get lista de tipos de observaciones para los CU => 4.1.8 - 4.1.9 - 4.3.1 - 4.3.2 - 4.3.4 - 4.3.5
    */
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=150`)
      .pipe(
        map( listaTipoObservacion => {
          const tiposObservaciones: any = {
            registrarSolicitudPago: {}
          };
          listaTipoObservacion.forEach( tipoObservacion => {
            if ( tipoObservacion.codigo === '1' ) {
              tiposObservaciones.cargarFormaPagoCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '2' ) {
              tiposObservaciones.registrarSolicitudPago.registrarSolicitudPagoCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '3' ) {
              tiposObservaciones.registrarSolicitudPago.criteriosPagoFacturaCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '4' ) {
              tiposObservaciones.registrarSolicitudPago.criteriosPagoProyectoCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '5' ) {
              tiposObservaciones.registrarSolicitudPago.datosFacturaCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '6' ) {
              tiposObservaciones.registrarSolicitudPago.datosFacturaDescuentoCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '7' ) {
              tiposObservaciones.soporteSolicitudCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '8' ) {
              tiposObservaciones.expensasCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '9' ) {
              tiposObservaciones.otrosCostosCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '10' ) {
              tiposObservaciones.registrarSolicitudPago.amortizacionAnticipoCodigo = tipoObservacion.codigo
            }
            if ( tipoObservacion.codigo === '11' ) {
              tiposObservaciones.listaChequeoCodigo = tipoObservacion.codigo
            }
          } );

          return tiposObservaciones;
        } )
      );
  }

  listaMenu() {
    // Lista menu del proyecto
    return this.http.get<any[]>(`${environment.apiUrl}/Common/GetListMenu`)
      .pipe(
        map( menus => {
          const menusId: any = {};
          menus.forEach( menu => {
            if ( menu.menuId === 70 ) {
              menusId.autorizarSolicitudPagoId = menu.menuId
            }
            if ( menu.menuId === 71 ) {
              menusId.aprobarSolicitudPagoId = menu.menuId
            }
          } );

          return menusId;
        } )
      );
  }

}
