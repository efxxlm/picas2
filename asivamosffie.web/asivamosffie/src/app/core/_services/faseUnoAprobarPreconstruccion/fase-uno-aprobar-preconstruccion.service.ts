import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { ObservacionPerfil } from 'src/app/_interfaces/faseUnoVerificarPreconstruccion.interface';
import { estadosPreconstruccion } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Dominio } from '../common/common.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoAprobarPreconstruccionService {

  private api_url: string = `${ environment.apiUrl }`;
  private paramUrl ( parametro: string ) {
    return `${ this.api_url }${ parametro }`;
  };

  constructor ( private http: HttpClient ) { }

  aprobarCrearContratoPerfilObservacion ( pContratoPerfilObservacion: ObservacionPerfil ) {
    return this.http.post<Respuesta>( `${ this.paramUrl( '/ApprovePreConstructionPhase1/CrearContratoPerfilObservacion' ) }`, pContratoPerfilObservacion );
  };

  //Estados verificar preconstrucción
  listaEstadosAprobarContrato ( tipoContrato: string ) {
    let estadosPreconstruccion: estadosPreconstruccion = {};
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=53`)
      .pipe(
        map(
          estados => {
            if ( tipoContrato === 'obra' ) {
              estados.forEach( value => {
                if ( value.codigo === '6' ) {
                  estadosPreconstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value[ 'descripcion' ]
                  };
                };
                if ( value.codigo === '7' ) {
                  estadosPreconstruccion.enProcesoValidacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '8' ) {
                  estadosPreconstruccion.conReqTecnicosValidados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '9' ) {
                  estadosPreconstruccion.conReqTecnicosAprobadosPorSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '10' ) {
                 estadosPreconstruccion.enviadoAlInterventor = {
                   codigo: value.codigo,
                   nombre: value.nombre
                 };
                };
              } );
              return estadosPreconstruccion;
            };
            if ( tipoContrato === 'interventoria' ) {
              estados.forEach( value => {
                if ( value.codigo === '6' ) {
                  estadosPreconstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value[ 'descripcion' ]
                  };
                };
                if ( value.codigo === '7' ) {
                  estadosPreconstruccion.enProcesoValidacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '8' ) {
                  estadosPreconstruccion.conReqTecnicosValidados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '9' ) {
                  estadosPreconstruccion.conReqTecnicosAprobadosPorSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                };
                if ( value.codigo === '11' ) {
                  estadosPreconstruccion.enviadoAlApoyo = {
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

}
