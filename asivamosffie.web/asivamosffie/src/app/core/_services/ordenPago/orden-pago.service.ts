import { Respuesta } from 'src/app/core/_services/common/common.service';
import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrdenPagoService {

  private urlApi = `${ environment.apiUrl }/GenerateSpinOrder`;

  constructor( private http: HttpClient ) { }

  getListOrdenGiro( pMenuId: number ) {
    return this.http.get<any[]>( `${ this.urlApi }/GetListOrdenGiro?pMenuId=${ pMenuId }` );
  }

  getSolicitudPagoBySolicitudPagoId( SolicitudPagoId: number ) {
    return this.http.get( `${ this.urlApi }/GetSolicitudPagoBySolicitudPagoId?SolicitudPagoId=${ SolicitudPagoId }` );
  }

  createEditOrdenGiro( pOrdenGiro ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditOrdenGiro`, pOrdenGiro );
  }

}
