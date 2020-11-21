import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PolizaGarantiaService implements OnInit {
  loadDataItems= new Subject();
  loadTableObservaciones = new Subject();
  constructor(private http: HttpClient) { }
  ngOnInit(): void {

  }
  CreatePolizaObservacion(polizaObservacion: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateEditPolizaObservacion`, polizaObservacion);
  }
  CreatePolizaGarantia(polizaGarantia: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateEditPolizaGarantia`, polizaGarantia);
  }
  CreateContratoPoliza(contratoPoliza: InsertPoliza) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateContratoPoliza`, contratoPoliza);
  }
  EditarContratoPoliza(contratoPoliza: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/EditarContratoPoliza`, contratoPoliza);
  }
  GetListPolizaObservacionByContratoPolizaId(pContratoPolizaId: number) {
    return this.http.get<any[]>(`${environment.apiUrl}/guaranteePolicy/GetListPolizaObservacionByContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetListPolizaGarantiaByContratoPolizaId(pContratoPolizaId: number) {
    return this.http.get<any[]>(`${environment.apiUrl}/guaranteePolicy/GetListPolizaGarantiaByContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetContratoPolizaByIdContratoPolizaId(pContratoPolizaId: number) {
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetContratoPolizaByIdContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetListVistaContratoGarantiaPoliza(pContratoId: number) {
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetListVistaContratoGarantiaPoliza?pContratoId=${pContratoId}`);
  }
  GetListGrillaContratoGarantiaPoliza() {
    return this.http.get<any[]>(`${environment.apiUrl}/guaranteePolicy/GetListGrillaContratoGarantiaPoliza`);
  }
  GetContratoPolizaByIdContratoId(pContratoId: number) {
    return this.http.get<GetContratoPolizaByIdContratoId>(`${environment.apiUrl}/guaranteePolicy/GetContratoPolizaByIdContratoId?pContratoId=${pContratoId}`);
  }
  AprobarContratoByIdContrato(pIdContrato: number) {
    return this.http.post<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/AprobarContratoByIdContrato?pIdContrato=${pIdContrato}`, null);
  }
  CambiarEstadoPoliza(pContratoPolizaId: number, pCodigoNuevoEstadoPoliza: string) {
    return this.http.put<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CambiarEstadoPoliza?pContratoPolizaId=${pContratoPolizaId}&pCodigoNuevoEstadoPoliza=${pCodigoNuevoEstadoPoliza}`, null);
  }
  CambiarEstadoPolizaByContratoId(pCodigoNuevoEstadoPoliza:string, pContratoId:number){
    return this.http.put<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CambiarEstadoPolizaByContratoId?pCodigoNuevoEstadoPoliza=${pCodigoNuevoEstadoPoliza}&pContratoId=${pContratoId}`, null);
  }
  GetNotificacionContratoPolizaByIdContratoId(pContratoId: number){
    return this.http.get<GetNotificacionContratoPolizaByIdContratoId>(`${environment.apiUrl}/guaranteePolicy/GetNotificacionContratoPolizaByIdContratoId?pContratoId=${pContratoId}`);
  }
}

export interface CreatePolizaObservacion {
  polizaObservacionId: number;
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


export interface InsertPoliza {
  contratoId: number;
  TipoSolicitudCodigo: any;
  TipoModificacionCodigo: any;
  DescripcionModificacion: any;
  NombreAseguradora: any;
  NumeroPoliza: any;
  NumeroCertificado: any;
  Observaciones: any;
  ObservacionesRevisionGeneral: any;
  ResponsableAprobacion: any;
  EstadoPolizaCodigo: any;
  UsuarioCreacion: any;
  UsuarioModificacion: any;
  FechaExpedicion: any;
  Vigencia: any;
  VigenciaAmparo: any;
  ValorAmparo: any;
  CumpleDatosAsegurado: boolean;
  CumpleDatosBeneficiario: boolean;
  CumpleDatosTomador: boolean;
  IncluyeReciboPago: boolean;
  IncluyeCondicionesGenerales: boolean;
  FechaAprobacion: any;
  Estado: boolean;
  FechaCreacion: any;
  RegistroCompleto: boolean;
  FechaModificacion: any;
  Eliminado: boolean;
}

export interface EditPoliza {
  contratoId: number;
  nombreAseguradora: string;
  numeroPoliza: string;
  numeroCertificado: string;
  fechaExpedicion: Date;
  vigencia: Date;
  vigenciaAmparo: Date;
  valorAmparo: number;
  estadoPolizaCodigo: string;
  usuarioCreacion: string;
  registroCompleto: boolean;
  fechaModificacion: Date;
  usuarioModificacion: string;
  contratoPolizaId: number;
  polizaGarantia: any;
  polizaObservacion: any;
  cumpleDatosAsegurado: boolean;
  cumpleDatosBeneficiario: boolean;
  cumpleDatosTomador: boolean;
  incluyeReciboPago: boolean;
  incluyeCondicionesGenerales: boolean;
}

export interface GetContratoPolizaByIdContratoId {
  contratoId: number;
  contratoPolizaId: number;
  cumpleDatosAsegurado: boolean;
  cumpleDatosBeneficiario: boolean;
  cumpleDatosTomador: boolean;
  descripcionModificacion: any;
  estado: boolean;
  estadoPolizaCodigo: any;
  fechaAprobacion: any;
  fechaExpedicion: any;
  incluyeCondicionesGenerales: boolean;
  incluyeReciboPago: boolean;
  nombreAseguradora: any;
  numeroCertificado:  any;
  numeroPoliza:  any;
  observaciones:  any;
  observacionesRevisionGeneral:  any;
  polizaGarantia:  any[];
  polizaObservacion:  any[];
  responsableAprobacion: any;
  tipoModificacionCodigo:  any;
  tipoSolicitudCodigo: any;
  valorAmparo: number;
  vigencia:  any;
  vigenciaAmparo:  any;
}

export interface GetNotificacionContratoPolizaByIdContratoId{
  nombreAseguradora: any;
  numeroPoliza: any;
  fechaRevision: any;
  fechaRevisionDateTime: any;
  fechaAprobacion: any;
  estadoRevision: any;
  observaciones: any;
}