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
  LoadActa(pcontrato: Contrato, archivoParaSubir: File) {
    const formData = new FormData();
    formData.append('file', archivoParaSubir, archivoParaSubir.name);
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/EditContrato`, formData);
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