import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaFaseUnoPreconstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { map } from 'rxjs/operators';
import { Contrato, ContratoModificado, estadosPreconstruccion } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { Dominio } from '../common/common.service';

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

  createEditContratoPerfil( pContrato: Contrato ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditContratoPerfil`, pContrato );
  }

  deleteContratoPerfil( contratoPerfilId: number ) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/DeleteContratoPerfil?ContratoPerfilId=${ contratoPerfilId }` );
  }

  deleteContratoPerfilNumeroRadicado( contratoPerfilNumeroRadicadoId: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/DeleteContratoPerfilNumeroRadicado?ContratoPerfilNumeroRadicadoId=${ contratoPerfilNumeroRadicadoId }`, '' );
  }
  changeStateContrato( pContratoId: number, pEstadoVerificacionContratoCodigo: string ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/ChangeStateContrato?pContratoId=${ pContratoId }&pEstadoVerificacionContratoCodigo=${ pEstadoVerificacionContratoCodigo }`, '' );
  }
  // Estados de preconstrucción
  listaEstadosVerificacionContrato() {
    const estadoPreconstruccion: estadosPreconstruccion = {};
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=53`)
      .pipe(
        map(
          estados => {
            estados.forEach( value => {
              if ( value.codigo === '1' ) {
                estadoPreconstruccion.sinAprobacionReqTecnicos = {
                  codigo: value.codigo,
                  nombre: value.nombre
                };
              }
              if ( value.codigo === '2' ) {
                estadoPreconstruccion.enProcesoAprobacionReqTecnicos = {
                  codigo: value.codigo,
                  nombre: value.nombre
                };
              }
              if ( value.codigo === '3' ) {
                estadoPreconstruccion.conReqTecnicosAprobados = {
                  codigo: value.codigo,
                  nombre: value.nombre
                };
              }
              if ( value.codigo === '10' ) {
                estadoPreconstruccion.enviadoAlInterventor = {
                  codigo: value.codigo,
                  nombre: value[ 'descripcion' ]
                };
              }
            } );
            return estadoPreconstruccion;
          }
        )
      );
  }

}
