import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaFaseUnoPreconstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { map } from 'rxjs/operators';
import { Contrato, ContratoModificado } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoPreconstruccionService {

  private url_api: string = `${ environment.apiUrl }/RegisterPreContructionPhase1`;

  constructor ( private http: HttpClient ) {};

  getListContratacion () {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.url_api }/GetListContratacion` );
  };

  getContratacionByContratoId ( pContratoId: string ) {
    return this.http.get<Contrato>( `${ this.url_api }/GetContratoByContratoId?pContratoId=${ pContratoId }` );
  };

  createEditContratoPerfil ( pContrato: Contrato ) {
    return this.http.post<Respuesta>( `${ this.url_api }/CreateEditContratoPerfil`, pContrato );
  };

  deleteContratoPerfil ( contratoPerfilId: number ) {
    return this.http.delete<Respuesta>( `${ this.url_api }/DeleteContratoPerfil?ContratoPerfilId=${ contratoPerfilId }` );
  };

  deleteContratoPerfilNumeroRadicado ( contratoPerfilNumeroRadicadoId: number ) {
    return this.http.post<Respuesta>( `${ this.url_api }/DeleteContratoPerfilNumeroRadicado?ContratoPerfilNumeroRadicadoId=${ contratoPerfilNumeroRadicadoId }`, '' )
  };

};