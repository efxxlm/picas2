import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaFaseUnoPreconstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { map } from 'rxjs/operators';
import { Contrato, ContratoModificado } from '../../../_interfaces/faseUnoPreconstruccion.interface';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoPreconstruccionService {

  private urlApi = `${ environment.apiUrl }/RegisterPreContructionPhase1`;

  constructor( private http: HttpClient ) {}

  getListContratacion() {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.urlApi }/GetListContratacion` );
  }

  getContratacionByContratoId( pContratoId: string ) {
    return this.http.get<Contrato>( `${ this.urlApi }/GetContratoByContratoId?pContratoId=${ pContratoId }` );
  }

  createEditContratoPerfil ( pContrato: Contrato ) {
    return this.http.post( `${ this.urlApi }/CreateEditContratoPerfil`, pContrato );
  }

  deleteContratoPerfil ( contratoPerfilId: number ) {
    return ( this.http.delete( `${ this.urlApi }/DeleteContratoPerfil?ContratoPerfilId=${ contratoPerfilId }` ) );
  }

  deleteContratoPerfilNumeroRadicado ( contratoPerfilNumeroRadicadoId: number ) {
    return this.http.post( `${ this.urlApi }/DeleteContratoPerfilNumeroRadicado?ContratoPerfilNumeroRadicadoId=${ contratoPerfilNumeroRadicadoId }`, '' );
  }

}
