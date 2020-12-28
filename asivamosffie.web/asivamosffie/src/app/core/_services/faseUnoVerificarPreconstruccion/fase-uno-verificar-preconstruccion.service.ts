import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Contrato, GrillaFaseUnoPreconstruccion, estadosPreconstruccion } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { ObservacionPerfil } from '../../../_interfaces/faseUnoVerificarPreconstruccion.interface';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { Dominio } from '../common/common.service';
import { map } from 'rxjs/operators';

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

  getListContratacionInterventoria () {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.paramUrl( '/VerifyPreConstructionRequirementsPhase1/GetListContratacionInterventoria' ) }` );
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
  //Estados verificar preconstrucción
  listaEstadosVerificacionContrato ( tipoContrato: string ) {
    let estadosPreconstruccion: estadosPreconstruccion = {};
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=53`)
      .pipe(
        map(
          estados => {
            if ( tipoContrato === 'obra' ) {
             estados.forEach( value => {
               if ( value.codigo === '3' ) {
                 estadosPreconstruccion.conReqTecnicosAprobados = {
                   codigo: value.codigo,
                   nombre: value[ 'descripcion' ]
                 };
               };
               if ( value.codigo === '4' ) {
                 estadosPreconstruccion.enProcesoAprobacionReqTecnicos = {
                   codigo: value.codigo,
                   nombre: value.nombre
                 };
               };
               if ( value.codigo === '5' ) {
                 estadosPreconstruccion.conReqTecnicosVerificados = {
                   codigo: value.codigo,
                   nombre: value.nombre
                 };
               };
               if ( value.codigo === '6' ) {
                 estadosPreconstruccion.enviadoAlSupervisor = {
                   codigo: value.codigo,
                   nombre: value.nombre
                 };
               };
             } );
             return estadosPreconstruccion;
            };
            if ( tipoContrato === 'interventoria' ) {
              estados.forEach( value => {
                if ( value.codigo === '1' ) {
                  estadosPreconstruccion.sinAprobacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: 'Sin verificación de requisitos técnicos'
                  };
                };
                if ( value.codigo === '4' ) {
                  estadosPreconstruccion.enProcesoVerificacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '5' ) {
                  estadosPreconstruccion.conReqTecnicosVerificados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '6' ) {
                  estadosPreconstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
              } );
              return estadosPreconstruccion;
            };
          }
        )
      );
  };

};
