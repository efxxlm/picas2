import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../common/common.service'
import { environment } from 'src/environments/environment';
import { pid } from 'process';
import { Contratacion, ContratacionProyecto } from 'src/app/_interfaces/project-contracting';

@Injectable({
  providedIn: 'root'
})
export class GestionarActPreConstrFUnoService {

  constructor(private http: HttpClient) { }

  GetListContrato() {
    return this.http.get<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetListContrato`);
  }
  GetContratoByContratoId(pContratoId: number) {
    return this.http.get<Contrato>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetContratoByContratoId?pContratoId=${pContratoId}`);
  }
  EditContrato(pcontrato: EditContrato) {
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/EditContrato`, pcontrato);
  }
  LoadActa(pcontrato:ContratoParaActa, archivoParaSubir: File,pDirectorioBase:string,pDirectorioActaContrato:string) {
    const formData = new FormData();
    formData.append('idContrato', `${pcontrato.contratoId}`);
    formData.append('fechaFirmaActaContratista', `${pcontrato.fechaFirmaActaContratista}`);
    formData.append('fechaFirmaActaContratistaInterventoria', `${pcontrato.fechaFirmaActaContratistaInterventoria}`);
    formData.append('file', archivoParaSubir, archivoParaSubir.name);
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/LoadActa?pDirectorioBase=${
      pDirectorioBase }&pDirectorioActaContrato=${ pDirectorioActaContrato }`, formData);
  }
  CambiarEstadoActa(pContratoId:number,pEstadoContrato:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/CambiarEstadoActa?pContratoId=${pContratoId}&pEstadoContrato=${pEstadoContrato}`,pContratoId);
  }
  GetActaByIdPerfil(pPerfilId:number, pContratoId:number){
    return this.http.get(`${environment.apiUrl}/ManagePreContructionActPhase1/GetActaByIdPerfil?pPerfilId=${pPerfilId}&pContratoId=${pContratoId}`, { responseType: "blob" } );
  }
}

export interface Contrato {
  contratoId: number;
  contratacionId: number;
  fechaTramite: Date;
  tipoContratoCodigo: string;
  numeroContrato: string;
  estado: boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  fechaActaInicioFase1: Date;
  fechaTerminacion: Date;
  plazoFase1PreMeses:number;
  plazoFase1PreDias:number;
  plazoFase2ConstruccionMeses:number;
  plazoFase2ConstruccionDias:number;
  observaciones: string;
  conObervacionesActa: boolean;
  rutaDocumento: any;
  usuarioCreacion: string;
  valor: any;
  plazo: Date;
  eliminado: boolean;
  estadoVerificacionCodigo: string;
  estadoDocumentoCodigo: string;
  estadoActa: string;
  contratacion: any[];
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}

export interface EditContrato {
  contratoId: number;
  contratacionId: number;
  fechaTramite: Date;
  tipoContratoCodigo: number;
  numeroContrato: string;
  estadoDocumentoCodigo: string;
  estado : boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  fechaActaInicioFase1?: string;
  fechaTerminacion?: string;
  plazoFase1PreMeses:number;
  plazoFase1PreDias:number;
  plazoFase2ConstruccionMeses:number;
  plazoFase2ConstruccionDias:number;
  observaciones: string;
  conObervacionesActa: boolean;
  registroCompleto: boolean;
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}
export interface ContratoParaActa {
  contratoId: number;
  contratacionId: number;
  fechaTramite: Date;
  tipoContratoCodigo: number;
  numeroContrato: string;
  estadoDocumentoCodigo: string;
  estado : boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  fechaActaInicioFase1?: string;
  fechaTerminacion?: string;
  fechaFirmaActaContratista?: Date;
  fechaFirmaActaContratistaInterventoria?: Date;
  plazoFase1PreMeses:number;
  plazoFase1PreDias:number;
  plazoFase2ConstruccionMeses:number;
  plazoFase2ConstruccionDias:number;
  observaciones: string;
  conObervacionesActa: boolean;
  registroCompleto: boolean;
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}