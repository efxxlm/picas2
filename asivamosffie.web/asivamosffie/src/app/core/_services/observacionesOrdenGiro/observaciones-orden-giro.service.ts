import { Respuesta } from 'src/app/core/_services/common/common.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ObservacionesOrdenGiroService {

  private apiUrl = `${ environment.apiUrl }/RegisterValidateSpinOrder`;

  constructor( private http: HttpClient ) { }

  createEditSpinOrderObservations( pOrdenGiroObservacion: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditSpinOrderObservations`, pOrdenGiroObservacion );
  }

  getObservacionOrdenGiroByMenuIdAndSolicitudPagoId( pMenuId: number, pOrdenGiroId: number, pPadreId: number, pTipoObservacionCodigo: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId?pMenuId=${ pMenuId }&pOrdenGiroId=${ pOrdenGiroId }&pPadreId=${ pPadreId }&pTipoObservacionCodigo=${ pTipoObservacionCodigo }` ).toPromise();

    // return new Promise<any[]>( resolve => {
    //   this.http.get<any[]>( `${ this.apiUrl }/GetObservacionOrdenGiroByMenuIdAndSolicitudPagoId?pMenuId=${ pMenuId }&pOrdenGiroId=${ pOrdenGiroId }&pPadreId=${ pPadreId }&pTipoObservacionCodigo=${ pTipoObservacionCodigo }` )
    //     .subscribe( response => resolve( response ) )
    // } )
  }

}
