import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaFaseUnoPreconstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoPreconstruccionService {

  private url_api: string = `${ environment.apiUrl }/RegisterPreContructionPhase1`;

  constructor ( private http: HttpClient ) {};

  getListContratacion () {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.url_api }/GetListContratacion` );
  };

  getContratacionByContratoId ( pContratoId: number ) {
    return this.http.get( `${ this.url_api }/GetContratacionByContratoId?pContratoId=${ pContratoId }` );
  }

  createEditContratoPerfil ( pContrato: any ) {
    return this.http.post( `${ this.url_api }/CreateEditContratoPerfil`, pContrato );
  };

  deleteContratoPerfil ( contratoPerfilId: number ) {
    return ( this.http.delete( `${ this.url_api }/DeleteContratoPerfil?ContratoPerfilId=${ contratoPerfilId }` ) );
  }

};