import { Respuesta } from 'src/app/core/_services/common/common.service';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GestionarParametricasService {

  private urlApi = `${ environment.apiUrl }/Parametric`;

  constructor( private http: HttpClient ) { }

  getParametricas() {
    return this.http.get<any[]>( `${ this.urlApi }/GetParametricas` );
  }

  dominioByIdDominio( pIdDominio: number ) {
    return this.http.get<any[]>( `${ this.urlApi }/GetDominioByTipoDominioId?TipoDominioId=${ pIdDominio }` );
  }

  createDominio( pTipoDominio: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateDominio`, pTipoDominio );
  }

}
