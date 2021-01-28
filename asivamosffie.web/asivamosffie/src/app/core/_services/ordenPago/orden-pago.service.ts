import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrdenPagoService {

  private urlApi = `${ environment.apiUrl }/GenerateSpinOrder`;

  constructor( private http: HttpClient ) { }

  getListSolicitudPago() {
    return this.http.get<any[]>( `${ this.urlApi }/GetListSolicitudPago` );
  }

  getSolicitudPagoBySolicitudPagoId( SolicitudPagoId: number ) {
    return this.http.get( `${ this.urlApi }/GetSolicitudPagoBySolicitudPagoId?SolicitudPagoId=${ SolicitudPagoId }` );
  }

}
