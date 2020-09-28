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

  GetListContrato(){
    return this.http.get<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetListContrato`);
  }
  GetContratoByContratoId(pContratoId: number){
    return this.http.get<Contrato>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetContratoByContratoId?pContratoId=${pContratoId}`);
  }
  EditContrato(pcontrato: Contrato){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/EditContrato`,pcontrato);
  }
  LoadActa(pcontrato: Contrato,archivoParaSubir: File){
    const formData = new FormData(); 
    formData.append('file', archivoParaSubir, archivoParaSubir.name);
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/EditContrato`,formData);
  }
}

export interface Contrato{
  contratoId: number;
  contratacionId: number;
  tipoContratoCodigo: string;
  estado: boolean;
  valor: any;
  plazo: Date;
  eliminado: boolean;
  estadoVerificacionCodigo: string;
  estadoActa: string;
  contratacion: Contratacion;
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}
