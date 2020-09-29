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
  tipoContratoCodigo: string;
  numeroContrato: string;
  estado: boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  observaciones: string;
  conObervacionesActa: boolean;
  rutaDocumento: any;
  usuarioCreacion: string;
  valor: any;
  plazo: Date;
  eliminado: boolean;
  estadoVerificacionCodigo: string;
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
  estado: boolean;
  "fechaEnvioFirma": "2020-09-25T05:00:00",
  "fechaFirmaContratista": "2020-09-25T05:00:00",
  "fechaFirmaFiduciaria": "2020-09-25T05:00:00",
  "fechaFirmaContrato": "2020-09-25T05:00:00",
  "observaciones": "preuba",
  "conObervacionesActa": false,
  "contratoConstruccion": [],
  "contratoObservacion": [],
  "contratoPerfil": [],
  "contratoPoliza": []
}