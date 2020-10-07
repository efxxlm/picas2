import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoVerificarPreconstruccionService {

  private api_url: string = `${ environment.apiUrl }/RegisterPreContructionPhase1`

  constructor( private http: HttpClient ) { };

  getListContratacion () {
    return this.http.get( `${ this.api_url }/GetListContratacion` );
  };

  getContratacionByContratoId ( pContratoId: number ) {
    return this.http.get( `${ this.api_url }/GetContratacionByContratoId?pContratoId=${ pContratoId }` );
  };

  createEditContratoPerfil ( pContrato: any ) {
    return this.http.post( `${ this.api_url }/CreateEditContratoPerfil`, pContrato );
  };

  deleteContratoPerfil ( contratoPerfilId: number ) {
    return this.http.delete( `${ this.api_url }/DeleteContratoPerfil?ContratoPerfilId=${ contratoPerfilId }` );
  };

};
