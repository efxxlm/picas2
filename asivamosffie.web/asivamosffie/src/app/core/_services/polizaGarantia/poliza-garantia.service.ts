import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class PolizaGarantiaService implements OnInit {

  constructor(private http: HttpClient) { }
  ngOnInit(): void {

  }
  CreatePolizaObservacion(polizaObservacion: CreatePolizaObservacion) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateEditPolizaObservacion`, polizaObservacion);
  }
  CreatePolizaGarantia(polizaGarantia: CreatePolizaGarantia) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateEditPolizaGarantia`, polizaGarantia);
  }
  CreateContratoPoliza(contratoPoliza: InsertPoliza) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateContratoPoliza`, contratoPoliza);
  }
  EditarContratoPoliza(contratoPoliza: EditPoliza){
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/EditarContratoPoliza`, contratoPoliza);
  }
  GetListPolizaObservacionByContratoPolizaId(pContratoPolizaId:number){
    return this.http.get<CreatePolizaObservacion>(`${environment.apiUrl}/guaranteePolicy/GetListPolizaObservacionByContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetListPolizaGarantiaByContratoPolizaId(pContratoPolizaId:number){
    return this.http.get<CreatePolizaGarantia>(`${environment.apiUrl}/guaranteePolicy/GetListPolizaGarantiaByContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetContratoPolizaByIdContratoPolizaId(pContratoPolizaId:number){
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetContratoPolizaByIdContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetListVistaContratoGarantiaPoliza(pContratoId:number){
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetListVistaContratoGarantiaPoliza?pContratoId=${pContratoId}`);
  }
  GetListGrillaContratoGarantiaPoliza(){
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetListGrillaContratoGarantiaPoliza`);
  }
  AprobarContratoByIdContrato(pContratoPolizaId:number){
    return this.http.post<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/AprobarContratoByIdContrato?pContratoPolizaId=${pContratoPolizaId}`,pContratoPolizaId);
  }
}

export interface CreatePolizaObservacion {
  polizaObservacionId:number;
  contratoPolizaId: number;
  observacion: string;
  fechaRevision: Date;
  estadoRevisionCodigo: string;
}
export interface CreatePolizaGarantia {
  polizaGarantiaId: number;
  contratoPolizaId: number;
  tipoGarantiaCodigo: any;
  esIncluidaPoliza: boolean;
}
export interface ContratoPoliza {
  contratoPolizaId: number;
  contratoId: number;
  tipoSolicitudCodigo: string;
  tipoModificacionCodigo: string;
  descripcionModificacion: string;
  nombreAseguradora: string;
  numeroPoliza: string;
  numeroCertificado: string;
  fechaExpedicion: Date;
  vigencia: Date;
  vigenciaAmparo: string;
  valorAmparo: number
  observaciones: string;
  cumpleDatosAsegurado: boolean;
  cumpleDatosBeneficiario: boolean;
  cumpleDatosTomador: boolean;
  incluyeReciboPago: boolean;
  incluyeCondicionesGenerales: boolean;
  observacionesRevisionGeneral: string;
  fechaAprobacion: Date;
  responsableAprobacion: string;
  estado: boolean;
  estadoPolizaCodigo: string;
  fechaCreacion: Date;
  usuarioCreacion: string;
  registroCompleo: boolean;
  fechaModificacion: Date;
  usuarioModificacion: string;
  eliminado: boolean;

}


export interface InsertPoliza{
  contratoId:string;
  nombreAseguradora:string;
  numeroPoliza:string;
  numeroCertificado:string;
  fechaExpedicion:Date;
  vigencia:Date;
  vigenciaAmparo:Date;
  valorAmparo:number;
}

export interface EditPoliza{
  contratoId:number;
  nombreAseguradora:string;
  numeroPoliza:string;
  numeroCertificado:string;
  fechaExpedicion:Date;
  vigencia:Date;
  vigenciaAmparo:Date;
  valorAmparo:number;
  estadoPolizaCodigo:string;
  usuarioCreacion:string;
  registroCompleto:boolean;
  fechaModificacion:Date;
  usuarioModificacion:string;
  contratoPolizaId:number;
  polizaGarantia:any;
  polizaObservacion:any;
  cumpleDatosAsegurado:boolean;
  cumpleDatosBeneficiario: boolean;
  cumpleDatosTomador: boolean;
  incluyeReciboPago: boolean;
  incluyeCondicionesGenerales: boolean;
}
