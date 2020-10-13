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
  EditCargarActaSuscritaContrato(pContratoId: number, pFechaFirmaContratista: Date, pFechaFirmaActaContratistaInterventoria: Date, pFile: File, pUsuarioModificacion: string) {
    const formData = new FormData();
    formData.append('pFile', pFile, pFile.name);
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EditCargarActaSuscritaContrato?pContratoId=${pContratoId}&pFechaFirmaContratista=${pFechaFirmaContratista}
    &pFechaFirmaActaContratistaInterventoria=${pFechaFirmaActaContratistaInterventoria}&pFile=${pFile}&pUsuarioModificacion=${pUsuarioModificacion}`, formData);
  }
  CreatePlazoEjecucionFase2Construccion(pContratoId: number, pPlazoFase2PreMeses: number, pPlazoFase2PreDias: number, pObservacionesConsideracionesEspeciales: string, pUsuarioModificacion: string) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/CreatePlazoEjecucionFase2Construccion?pContratoId=${pContratoId}&pPlazoFase2PreMeses=${pPlazoFase2PreMeses}&pPlazoFase2PreDias=${pPlazoFase2PreDias}&pObservacionesConsideracionesEspeciales=${pObservacionesConsideracionesEspeciales}&pUsuarioModificacion=${pUsuarioModificacion}`, "");
  }
  CreateTieneObservacionesActaInicio(pContratoId:number, pObservacionesActa:string, pUsuarioModificacion: string){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ctBegin/CreateTieneObservacionesActaInicio?pContratoId=${pContratoId}&pObservacionesActa=${pObservacionesActa}&pUsuarioModificacion=${pUsuarioModificacion}`, "");
  }
}
export interface GetVistaGenerarActaInicio {
  cantidadProyectosAsociados: number;
  departamentoYMunicipioLlaveMEN: string;
  fechaActaInicio: any;
  fechaAprobacionGarantiaPoliza: any;
  fechaFirmaContrato: any;
  fechaGeneracionDRP1: any;
  fechaGeneracionDRP2: any;
  fechaPrevistaTerminacion: any;
  institucionEducativaLlaveMEN: string;
  llaveMENContrato: string;
  nombreEntidadContratistaObra: string;
  nombreEntidadContratistaSupervisorInterventoria: string;
  numeroContrato: string;
  numeroDRP1: string;
  numeroDRP2: string;
  observacionOConsideracionesEspeciales: string;
  plazoInicialContratoSupervisor: Date;
  valorActualContrato: string;
  valorFase1Preconstruccion: string;
  valorInicialContrato: string;
  valorfase2ConstruccionObra: string;
  vigenciaContrato: Date;
}
export interface GetListGrillaActaInicio {

}
export interface GetPlantillaActaInicio {

}
