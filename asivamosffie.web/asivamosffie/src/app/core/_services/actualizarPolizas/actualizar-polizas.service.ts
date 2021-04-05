import { Respuesta } from 'src/app/core/_services/common/common.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ActualizarPolizasService {

  private apiUrl = `${ environment.apiUrl }/UpdatePoliciesGuarantees`;

  constructor( private http: HttpClient ) { }

  getContratoByNumeroContrato( pNumeroContrato: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetContratoByNumeroContrato?pNumeroContrato=${ pNumeroContrato }` );
  }

  getContratoPoliza( pContratoPolizaId: number ) {
    return this.http.get<any>( `${ this.apiUrl }/GetContratoPoliza?pContratoPolizaId=${ pContratoPolizaId }` )
  }

  getListVActualizacionPolizaYGarantias() {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListVActualizacionPolizaYGarantias` );
  }

  createorUpdateCofinancing( pContratoPolizaActualizacion: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateorUpdateCofinancing`, pContratoPolizaActualizacion )
  }

}
