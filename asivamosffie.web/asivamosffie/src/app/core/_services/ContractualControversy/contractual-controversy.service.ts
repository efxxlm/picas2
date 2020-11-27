import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class ContractualControversyService implements OnInit{

  constructor(private http: HttpClient) { }
  ngOnInit(): void {

  }
  CreateEditarControversiaTAI(controversiaContractual: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/CreateEditarControversiaTAI`, controversiaContractual);
  }
  CreateEditarControversiaMotivo(controversiaMotivo: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/CreateEditarControversiaMotivo`, controversiaMotivo);
  }
  CreateEditControversiaOtros(controversiaContractual: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/CreateEditControversiaOtros`, controversiaContractual);
  }
  GetMotivosSolicitudByControversiaId(id: any){
    return this.http.get<any[]>(`${environment.apiUrl}/ContractualControversy/GetMotivosSolicitudByControversiaId?id=${id}`);
  }
  CreateEditarActuacionSeguimiento(actuacionSeguimiento: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/CreateEditarActuacionSeguimiento`, actuacionSeguimiento);
  }
  CreateEditNuevaActualizacionTramite(controversiaActuacion: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/CreateEditNuevaActualizacionTramite`, controversiaActuacion);
  }
  GetListGrillaTipoSolicitudControversiaContractual(){
    return this.http.get<GetListGrillaTipoSolicitudControversiaContractual>(`${environment.apiUrl}/ContractualControversy/GetListGrillaTipoSolicitudControversiaContractual`);
  }
  GetListGrillaControversiaActuacion(pControversiaContractualId:number){
    return this.http.get<any[]>(`${environment.apiUrl}/ContractualControversy/GetListGrillaControversiaActuacion?pControversiaContractualId=${pControversiaContractualId}`);
  }
  GetVistaContratoContratista(pContratoId: number){
    return this.http.get<GetVistaContratoContratista>(`${environment.apiUrl}/ContractualControversy/GetVistaContratoContratista?pContratoId=${pContratoId}`);
  }
  CambiarEstadoControversiaContractual(pControversiaContractualId:number, pNuevoCodigoEstado:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ContractualControversy/CambiarEstadoControversiaContractual?pControversiaContractualId=${pControversiaContractualId}&pNuevoCodigoEstado=${pNuevoCodigoEstado}`, null);  
  }
  EliminarControversiaContractual(pControversiaContractualId:number){
    return this.http.post<EliminarControversiaContractual>(`${environment.apiUrl}/ContractualControversy/EliminarControversiaContractual?pControversiaContractualId=${pControversiaContractualId}`,null);
  }
  GetControversiaContractualById(pControversiaContractualId: number){
    return this.http.get<GetControversiaContractualById>(`${environment.apiUrl}/ContractualControversy/GetControversiaContractualById?pControversiaContractualId=${pControversiaContractualId}`);
  }
  GetControversiaActuacionById(pControversiaActuacionId: number){
    return this.http.get<GetControversiaContractualById>(`${environment.apiUrl}/ContractualControversy/GetControversiaActuacionById?pControversiaActuacionId=${pControversiaActuacionId}`);
  }
  GetListContratos(){
    return this.http.get<GetListContratos>(`${environment.apiUrl}/ContractualControversy/GetListContratos`);
  }
  EliminarControversiaActuacion(pControversiaActuacionId:number){
    return this.http.get<EliminarControversiaActuacion>(`${environment.apiUrl}/ContractualControversy/EliminarControversiaActuacion?pControversiaActuacionId=${pControversiaActuacionId}`);
  }
  ActualizarRutaSoporteControversiaContractual(pControversiaContractualId:number, pRutaSoporte:string){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/ActualizarRutaSoporteControversiaContractual?pControversiaContractualId=${pControversiaContractualId}&pRutaSoporte=${pRutaSoporte}`, null);
  }
  ActualizarRutaSoporteControversiaActuacion(pControversiaActuacionId:number, pRutaSoporte:string){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/ActualizarRutaSoporteControversiaActuacion?pControversiaActuacionId=${pControversiaActuacionId}&pRutaSoporte=${pRutaSoporte}`, null);
  }
  CambiarEstadoActuacionSeguimiento(pActuacionSeguimientoId:number, pEstadoReclamacionCodigo:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ContractualControversy/CambiarEstadoActuacionSeguimiento?pActuacionSeguimientoId=${pActuacionSeguimientoId}&pEstadoReclamacionCodigo=${pEstadoReclamacionCodigo}`,null);
  }
  CambiarEstadoControversiaActuacion(pControversiaActuacionId:number, pNuevoCodigoEstadoAvance:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ContractualControversy/CambiarEstadoControversiaActuacion?pControversiaActuacionId=${pControversiaActuacionId}&pNuevoCodigoEstadoAvance=${pNuevoCodigoEstadoAvance}`, null);
  }
}

export interface GetListGrillaTipoSolicitudControversiaContractual{

}

export interface GetVistaContratoContratista{
  fechaFinContrato: any;
  fechaInicioContrato: any;
  idContratista: number;
  nombreContratista: any;
  numeroContrato: any;
  plazoFormat: any;
}

export interface EliminarControversiaContractual{

}

export interface EliminarControversiaActuacion{

}

export interface GetListGrillaControversiaActuacion{

}

export interface GetControversiaContractualById{
  contratoId: number;
}

export interface GetControversiaActuacionById{

}

export interface GetListContratos{

}