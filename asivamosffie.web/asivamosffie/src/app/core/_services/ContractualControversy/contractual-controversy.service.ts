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
  CreateEditNuevaActualizacionTramite(controversiaActuacion: any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ContractualControversy/CreateEditNuevaActualizacionTramite`, controversiaActuacion);
  }
  GetListGrillaTipoSolicitudControversiaContractual(){
    return this.http.get<GetListGrillaTipoSolicitudControversiaContractual>(`${environment.apiUrl}/ContractualControversy/GetListGrillaTipoSolicitudControversiaContractual`);
  }
  GetVistaContratoContratista(pContratoId: number){
    return this.http.get<GetVistaContratoContratista>(`${environment.apiUrl}/ContractualControversy/GetVistaContratoContratista?pContratoId=${pContratoId}`);
  }
  CambiarEstadoControversiaContractual(pControversiaContractualId:number, pNuevoCodigoEstado:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ContractualControversy/CambiarEstadoControversiaContractual?pControversiaContractualId=${pControversiaContractualId}&pNuevoCodigoEstado=${pNuevoCodigoEstado}`, null);  
  }
  EliminarControversiaContractual(pControversiaContractualId:number){
    return this.http.get<EliminarControversiaContractual>(`${environment.apiUrl}/ContractualControversy/EliminarControversiaContractual?pControversiaContractualId=${pControversiaContractualId}`);
  }
  EliminarControversiaActuacion(pControversiaActuacionId:number){
    return this.http.get<EliminarControversiaContractual>(`${environment.apiUrl}/ContractualControversy/EliminarControversiaActuacion?pControversiaActuacionId=${pControversiaActuacionId}`);
  }
  CambiarEstadoControversiaActuacion(pControversiaActuacionId:number, pNuevoCodigoEstadoAvance:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ContractualControversy/CambiarEstadoControversiaActuacion?pControversiaActuacionId=${pControversiaActuacionId}&pNuevoCodigoEstadoAvance=${pNuevoCodigoEstadoAvance}`, null);
  }
}

export interface GetListGrillaTipoSolicitudControversiaContractual{

}

export interface GetVistaContratoContratista{

}

export interface EliminarControversiaContractual{

}