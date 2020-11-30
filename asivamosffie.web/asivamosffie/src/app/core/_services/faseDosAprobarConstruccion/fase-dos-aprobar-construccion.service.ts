import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { Dominio } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class FaseDosAprobarConstruccionService {

  private apiUrl = `${ environment.apiUrl }/TechnicalCheckConstructionPhase2`;

  constructor( private http: HttpClient )
  { }

  getContractsGrid( pTipoContrato: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetContractsGrid?pTipoContrato=${ pTipoContrato }` );
  }

  createEditObservacionDiagnosticoSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionDiagnostico?esSupervisor=true`, contratoConstruccion );
  }

  createEditObservacionPlanesProgramasSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionPlanesProgramas?esSupervisor=true`, contratoConstruccion );
  }

  createEditObservacionManejoAnticipoSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionManejoAnticipo?esSupervisor=true`, contratoConstruccion );
  }

  createEditObservacionProgramacionObraSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionProgramacionObra?esSupervisor=true`, contratoConstruccion );
  }

  createEditObservacionFlujoInversionSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionFlujoInversion?esSupervisor=true`, contratoConstruccion );
  }

  createEditObservacionPerfilSupervisor( contratacionPerfil ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionPerfil?esSupervisor=true`, contratacionPerfil );
  }

  createEditObservacionConstruccionPerfil( pObservacion: any ) {
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/CreateEditObservacionConstruccionPerfil`, pObservacion );
  }

  // Estados AprobarConstruccion
  listaEstadosAprobarConstruccion( tipoContrato: string ) {
    const estadosConstruccion: any = {};
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=53`)
      .pipe(
        map(
          estados => {
            if ( tipoContrato === 'obra' ) {
              estados.forEach( value => {
                if ( value.codigo === '6' ) {
                  estadosConstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value.descripcion
                  };
                }
                if ( value.codigo === '7' ) {
                  estadosConstruccion.enProcesoValidacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '8' ) {
                  estadosConstruccion.conReqTecnicosValidados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '9' ) {
                  estadosConstruccion.conReqTecnicosAprobadosPorSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '10' ) {
                 estadosConstruccion.enviadoAlInterventor = {
                   codigo: value.codigo,
                   nombre: value.nombre
                 };
                }
              } );
              return estadosConstruccion;
            }
            if ( tipoContrato === 'interventoria' ) {
              estados.forEach( value => {
                if ( value.codigo === '6' ) {
                  estadosConstruccion.enviadoAlSupervisor = {
                    codigo: value.codigo,
                    nombre: value.descripcion
                  };
                }
                if ( value.codigo === '7' ) {
                  estadosConstruccion.enProcesoValidacionReqTecnicos = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '8' ) {
                  estadosConstruccion.conReqTecnicosValidados = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '9' ) {
                  estadosConstruccion.conReqTecnicosAprobadosPorSupervisor = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
                if ( value.codigo === '11' ) {
                  estadosConstruccion.enviadoAlApoyo = {
                    codigo: value.codigo,
                    nombre: value.nombre
                  };
                }
              } );
              return estadosConstruccion;
            }
          }
        )
      );
  }

}
