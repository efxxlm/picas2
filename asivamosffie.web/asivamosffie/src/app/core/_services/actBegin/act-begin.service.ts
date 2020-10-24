import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class ActBeginService {

  constructor(private http: HttpClient) { }
  GetVistaGenerarActaInicio(pContratoId: number) {
    return this.http.get<GetVistaGenerarActaInicio>(`${environment.apiUrl}/actBegin/GetVistaGenerarActaInicio?pContratoId=${pContratoId}`);
  }
  GetListGrillaActaInicio() {
    return this.http.get<GetListGrillaActaInicio>(`${environment.apiUrl}/actBegin/GetListGrillaActaInicio`);
  }
  GetPlantillaActaInicio(pContratoId: number) {
    return this.http.get(`${environment.apiUrl}/actBegin/GetPlantillaActaInicio?pContratoId=${pContratoId}`, { responseType: "blob" });
  }
  EditCargarActaSuscritaContrato(pContratoId: number, pFechaFirmaContratista: string, pFechaFirmaActaContratistaInterventoria: string, pFile: File, pUsuarioModificacion: string) {
    const formData = new FormData();
    formData.append('pFile', pFile, pFile.name);
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EditCargarActaSuscritaContrato?pContratoId=${pContratoId}&pFechaFirmaContratista=${pFechaFirmaContratista}&pFechaFirmaActaContratistaInterventoria=${pFechaFirmaActaContratistaInterventoria}&pUsuarioModificacion=${pUsuarioModificacion}`, formData);
  }
  CreatePlazoEjecucionFase2Construccion(pContratoId: number, pPlazoFase2PreMeses: number, pPlazoFase2PreDias: number, pObservacionesConsideracionesEspeciales: string, pUsuarioModificacion: string, pFechaActaInicioFase1: string, pFechaTerminacionFase2: string) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/CreatePlazoEjecucionFase2Construccion?pContratoId=${pContratoId}&pPlazoFase2PreMeses=${pPlazoFase2PreMeses}&pPlazoFase2PreDias=${pPlazoFase2PreDias}&pObservacionesConsideracionesEspeciales=${pObservacionesConsideracionesEspeciales}&pUsuarioModificacion=${pUsuarioModificacion}&pFechaActaInicioFase1=${pFechaActaInicioFase1}&pFechaTerminacionFase2=${pFechaTerminacionFase2}`, "");
  }
  CreateTieneObservacionesActaInicio(pContratoId:number, pObservacionesActa:string, pUsuarioModificacion: string){
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/CreateTieneObservacionesActaInicio?pContratoId=${pContratoId}&pObservacionesActa=${pObservacionesActa}&pUsuarioModificacion=${pUsuarioModificacion}`, "");
  }
  EditarContratoObservacion(pContratoId: number,  pPlazoFase2PreMeses:number , pPlazoFase2PreDias:number, pObservacion:string, pUsuarioModificacion:string,pFechaActaInicioFase1:string,pFechaTerminacionFase2:string){
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EditarContratoObservacion?pContratoId=${pContratoId}&pPlazoFase2PreMeses=${pPlazoFase2PreMeses}&pPlazoFase2PreDias=${pPlazoFase2PreDias}&pObservacion=${pObservacion}&pUsuarioModificacion=${pUsuarioModificacion}&pFechaActaInicioFase1=${pFechaActaInicioFase1}&pFechaTerminacionFase2=${pFechaTerminacionFase2}`, "");
  }
  CambiarEstadoActa(pContratoId :number, pNuevoCodigoEstadoActa:string, pUsuarioModifica:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/actBegin/CambiarEstadoActa?pContratoId=${pContratoId}&pNuevoCodigoEstadoActa=${pNuevoCodigoEstadoActa}&pUsuarioModifica=${pUsuarioModifica}`, null);  
  }
}
export interface GetVistaGenerarActaInicio {
  cantidadProyectosAsociados: number;
  departamentoYMunicipioLlaveMEN: string;
  fechaActaInicio: Date;
  fechaAprobacionGarantiaPoliza: Date;
  fechaFirmaContrato: Date;
  fechaGeneracionDRP1: Date;
  fechaGeneracionDRP2: Date;
  fechaPrevistaTerminacion: Date;
  institucionEducativaLlaveMEN: string;
  llaveMENContrato: string;
  nombreEntidadContratistaObra: string;
  nombreEntidadContratistaSupervisorInterventoria: string;
  numeroContrato: string;
  numeroDRP1: string;
  numeroDRP2: string;
  objeto: string;
  observacionOConsideracionesEspeciales: string;
  plazoInicialContratoSupervisor: Date;
  valorActualContrato: string;
  valorFase1Preconstruccion: string;
  valorInicialContrato: string;
  valorfase2ConstruccionObra: string;
  vigenciaContrato: Date;
  plazoFase2ConstruccionDias: number;
  plazoFase2ConstruccionMeses: number;
  plazoFase1PreDias: number;
  plazoFase1PreMeses: number;
}
export interface GetListGrillaActaInicio {

}
export interface GetPlantillaActaInicio {

}
