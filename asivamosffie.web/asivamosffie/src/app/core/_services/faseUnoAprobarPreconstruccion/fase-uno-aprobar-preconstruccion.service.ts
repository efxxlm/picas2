import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { ObservacionPerfil } from 'src/app/_interfaces/faseUnoVerificarPreconstruccion.interface';
import { estadosPreconstruccion, GrillaFaseUnoPreconstruccion } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Dominio } from '../common/common.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoAprobarPreconstruccionService {

  private apiUrl = `${ environment.apiUrl }`;
  private paramUrl( parametro: string ) {
    return `${ this.apiUrl }${ parametro }`;
  }

  constructor( private http: HttpClient ) { }

  getListContratacion() {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.paramUrl( '/ApprovePreConstructionPhase1/GetListContratacion' ) }` );
  }

  aprobarCrearContratoPerfilObservacion( pContratoPerfilObservacion: ObservacionPerfil ) {
    return this.http.post<Respuesta>(
      `${ this.paramUrl( '/ApprovePreConstructionPhase1/CrearContratoPerfilObservacion' ) }`, pContratoPerfilObservacion
    );
  }

  // Estados verificar preconstrucción
  listaEstadosAprobarContrato( tipoContrato: string ) {
    const estadoPreconstruccion: estadosPreconstruccion = {};
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=53`)
      .pipe(
        map(
          estados => {
            if ( tipoContrato === 'obra' ) {
              estados.forEach( value => {
                if ( value.codigo === '6' ) {
                  estadoPreconstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value[ 'descripcion' ]
                  };
                }
                if ( value.codigo === '7' ) {
                  estadoPreconstruccion.enProcesoValidacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '8' ) {
                  estadoPreconstruccion.conReqTecnicosValidados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '9' ) {
                  estadoPreconstruccion.conReqTecnicosAprobadosPorSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '10' ) {
                 estadoPreconstruccion.enviadoAlInterventor = {
                   codigo: value.codigo,
                   nombre: value.nombre
                 };
                }
              } );
              return estadoPreconstruccion;
            }
            if ( tipoContrato === 'interventoria' ) {
              estados.forEach( value => {
                if ( value.codigo === '6' ) {
                  estadoPreconstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value[ 'descripcion' ]
                  };
                }
                if ( value.codigo === '7' ) {
                  estadoPreconstruccion.enProcesoValidacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '8' ) {
                  estadoPreconstruccion.conReqTecnicosValidados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '9' ) {
                  estadoPreconstruccion.conReqTecnicosAprobadosPorSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '11' ) {
                  estadoPreconstruccion.enviadoAlApoyo = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
              } );
              return estadoPreconstruccion;
            }
          }
        )
      );
  }

}
