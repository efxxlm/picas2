import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Contrato, GrillaFaseUnoPreconstruccion } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { ObservacionPerfil } from '../../../_interfaces/faseUnoVerificarPreconstruccion.interface';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoVerificarPreconstruccionService {

  private api_url: string = `${ environment.apiUrl }`;
  private paramUrl ( parametro: string ) {
    return `${ this.api_url }${ parametro }`;
  };

  constructor( private http: HttpClient ) { };

  getListContratacion () {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.paramUrl( '/VerifyPreConstructionRequirementsPhase1/GetListContratacion' ) }` );
  };

  getContratacionByContratoId ( pContratoId: number ) {
    return this.http.get<Contrato>( `${ this.paramUrl( '/VerifyPreConstructionRequirementsPhase1/GetContratoByContratoId' ) }?pContratoId=${ pContratoId }` );
  };

  createEditContratoPerfil ( pContrato: any ) {
    return this.http.post( `${ this.paramUrl( '/RegisterPreContructionPhase1/CreateEditContratoPerfil' ) }`, pContrato );
  };

  deleteContratoPerfil ( contratoPerfilId: number ) {
    return this.http.delete( `${ this.paramUrl( '/RegisterPreContructionPhase1/DeleteContratoPerfil' ) }?ContratoPerfilId=${ contratoPerfilId }` );
  };

  crearContratoPerfilObservacion ( pContratoPerfilObservacion: ObservacionPerfil ) {
    return this.http.post<Respuesta>( `${ this.paramUrl( '/VerifyPreConstructionRequirementsPhase1/CrearContratoPerfilObservacion' ) }`, pContratoPerfilObservacion );
  }

};
