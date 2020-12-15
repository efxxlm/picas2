import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActBeginService {
  loadDataItems= new Subject();
  constructor(private http: HttpClient) { }
  GetVistaGenerarActaInicio(pContratoId: number) {
    return this.http.get<GetVistaGenerarActaInicio>(`${environment.apiUrl}/actBegin/GetVistaGenerarActaInicio?pContratoId=${pContratoId}`);
  }
  GetListGrillaActaInicio(pPerfilId: number) {
    return this.http.get<GetListGrillaActaInicio>(`${environment.apiUrl}/actBegin/GetListGrillaActaInicio?pPerfilId=${pPerfilId}`);
  }
  GetPlantillaActaInicio(pContratoId: number) {
    return this.http.get(`${environment.apiUrl}/actBegin/GetPlantillaActaInicio?pContratoId=${pContratoId}`, { responseType: "blob" });
  }
  EditCargarActaSuscritaContrato(pContratoId: number, pFechaFirmaContratista: string, pFechaFirmaActaContratistaInterventoria: string, pFile: File, pUsuarioModificacion: string) {
    const formData = new FormData();
    formData.append('pFile', pFile, pFile.name);
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EditCargarActaSuscritaContrato?pContratoId=${pContratoId}&pFechaFirmaContratista=${pFechaFirmaContratista}&pFechaFirmaActaContratistaInterventoria=${pFechaFirmaActaContratistaInterventoria}&pUsuarioModificacion=${pUsuarioModificacion}`, formData);
  }
  CreatePlazoEjecucionFase2Construccion(pContratoId: number, pPlazoFase2PreMeses: number, pPlazoFase2PreDias: number, pObservacionesConsideracionesEspeciales: string, pUsuarioModificacion: string, pFechaActaInicioFase1: string, pFechaTerminacionFase2: string, pEsSupervisor:boolean, pEsActa:boolean) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/CreatePlazoEjecucionFase2Construccion?pContratoId=${pContratoId}&pPlazoFase2PreMeses=${pPlazoFase2PreMeses}&pPlazoFase2PreDias=${pPlazoFase2PreDias}&pObservacionesConsideracionesEspeciales=${pObservacionesConsideracionesEspeciales}&pUsuarioModificacion=${pUsuarioModificacion}&pFechaActaInicioFase1=${pFechaActaInicioFase1}&pFechaTerminacionFase2=${pFechaTerminacionFase2}&pEsSupervisor=${pEsSupervisor}&pEsActa=${pEsActa}`, "");
  }
  CreateTieneObservacionesActaInicio(pContratoId:number, pObservacionesActa:string, pUsuarioModificacion: string, pEsSupervisor:boolean, pEsActa:boolean){
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/CreateTieneObservacionesActaInicio?pContratoId=${pContratoId}&pObservacionesActa=${pObservacionesActa}&pUsuarioModificacion=${pUsuarioModificacion}&pEsSupervisor=${pEsSupervisor}&pEsActa=${pEsActa}`, "");
  }

  EditarContratoObservacion(pContratoId: number,  pPlazoFase2PreMeses:number , pPlazoFase2PreDias:number, pObservacion:string, pUsuarioModificacion:string,pFechaActaInicioFase1:string,pFechaTerminacionFase2:string, pEsSupervisor:boolean, pEsActa:boolean){

    let construccionObservacion: ConstruccionObservacion = {
      Observaciones: pObservacion
    }

    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EditarContratoObservacion?pContratoId=${pContratoId}&pPlazoFase2PreMeses=${pPlazoFase2PreMeses}&pPlazoFase2PreDias=${pPlazoFase2PreDias}&pObservacion=${pObservacion}&pUsuarioModificacion=${pUsuarioModificacion}&pFechaActaInicioFase1=${pFechaActaInicioFase1}&pFechaTerminacionFase2=${pFechaTerminacionFase2}&pEsSupervisor=${pEsSupervisor}&pEsActa=${pEsActa}`, construccionObservacion);
  }

  CambiarEstadoActa(pContratoId :number, pNuevoCodigoEstadoActa:string, pUsuarioModifica:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/actBegin/CambiarEstadoActa?pContratoId=${pContratoId}&pNuevoCodigoEstadoActa=${pNuevoCodigoEstadoActa}&pUsuarioModifica=${pUsuarioModifica}`, null);  
  }
  GetContratoObservacionByIdContratoId(pContratoId: number, pEsSupervisor:boolean){
    return this.http.get<GetContratoObservacionByIdContratoId>(`${environment.apiUrl}/actBegin/GetContratoObservacionByIdContratoId?pContratoId=${pContratoId}&pEsSupervisor=${pEsSupervisor}`);
  }
  CambiarEstadoVerificacionActa(pContratoId: number, pNuevoCodigoEstadoVerificacionActa: string,pUsuarioModifica : string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/actBegin/CambiarEstadoActa?pContratoId=${pContratoId}&pNuevoCodigoEstadoVerificacionActa=${pNuevoCodigoEstadoVerificacionActa}&pUsuarioModifica=${pUsuarioModifica}`, null);  
  }
  GetContratoByIdContratoId(pContratoId: number){
    return this.http.get<GetContratoByIdContratoId>(`${environment.apiUrl}/actBegin/GetContratoByIdContratoId?pContratoId=${pContratoId}`);
  }
  CreateEditContratoObservacion(contratoObs: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/CreateEditContratoObservacion`, contratoObs);
  }
  EnviarCorreoSupervisorContratista(pContratoId: number, pPerfilId: number){
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EnviarCorreoSupervisorContratista?pContratoId=${pContratoId}&pPerfilId=${pPerfilId}`,null);

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
  fechaActaInicioDateTime: any;
  fechaPrevistaTerminacionDateTime: any;
  fechaActaInicioFase1DateTime: any;
}
export interface GetListGrillaActaInicio {

}
export interface GetPlantillaActaInicio {

}
export interface GetContratoObservacionByIdContratoId {
  contratoObservacionId: number;
  contratoConstruccionId: number;
  contratoId: number;
  observaciones: string;
  fechaCreacion: Date;
  usuarioCreacion: string;
  esActa: boolean;
  esActaFase2: boolean;
}
export interface GetContratoByIdContratoId{
  conObervacionesActa: boolean;
  observaciones: string;
}
export interface ContratoObservacion{
  contratoObservacionId: number;
  contratoId: number;
  observaciones: string;
  fechaCreacion?: any;
  usuarioCreacion?: any;
  
  //opcionales
  esActa?: boolean;
  fechaModificacion?: any;
  usuarioModificacion?: any;
  esActaFase2?: boolean;
}
export interface ConstruccionObservacion{
//ConstruccionObservacionId: number;
 ContratoConstruccionId?: number;
 TipoObservacionConstruccion?: string;
 Observaciones?: string;
 UsuarioModificacion?: string;
 FechaCreacion?:any;
 UsuarioCreacion?: string;
 EsSupervision?:boolean;
 EsActa?:boolean;
FechaModificacion?:any;
}