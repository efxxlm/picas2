import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FichaContratoService {
  private urlApi = `${ environment.apiUrl }`;

  constructor( private http: HttpClient ) { }

  ficha_contrato = 'FichaContrato';
   
  getContratosByNumeroContrato( pNumeroContrato: string ){
    return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_contrato}/GetContratosByNumeroContrato?pNumeroContrato=${ pNumeroContrato }`);
  }

}
