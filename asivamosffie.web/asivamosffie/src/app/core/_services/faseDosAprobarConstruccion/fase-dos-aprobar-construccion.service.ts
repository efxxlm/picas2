import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class FaseDosAprobarConstruccionService {

  private apiUrl: string = `${ environment.apiUrl }/TechnicalCheckConstructionPhase2`;

  constructor ( private http: HttpClient )
  { };

  getContractsGrid ( pTipoContrato: string ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetContractsGrid?pTipoContrato=${ pTipoContrato }` );
  };

  createEditObservacionDiagnosticoSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionDiagnostico?esSupervisor=true`, contratoConstruccion );
  };

  createEditObservacionPlanesProgramasSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionPlanesProgramas?esSupervisor=true`, contratoConstruccion );
  };

  createEditObservacionManejoAnticipoSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionManejoAnticipo?esSupervisor=true`, contratoConstruccion );
  };

  createEditObservacionProgramacionObraSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionProgramacionObra?esSupervisor=true`, contratoConstruccion );
  };

  createEditObservacionFlujoInversionSupervisor( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase/createEditObservacionFlujoInversion?esSupervisor=true`, contratoConstruccion );
  };

};