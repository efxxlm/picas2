import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FaseDosAprobarConstruccionService {

  private apiUrl: string = `${ environment.apiUrl }/TechnicalCheckConstructionPhase2`;

  constructor ( private http: HttpClient )
  { };

  getContractsGrid ( pTipoContrato: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetContractsGrid?pTipoContrato=${ pTipoContrato }` );
  };

};