import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../common/common.service'
import { environment } from 'src/environments/environment';
import { pid } from 'process';


@Injectable({
  providedIn: 'root'
})
export class RegisterPreContructionPhase1Service {

  constructor(private http: HttpClient) { }
  getListContratacionRPrcF1(){
    return this.http.get<ContratacionList>(`${environment.apiUrl}/RegisterPreContructionPhase1/GetListContratacion`);
  }
  getContratacionByContratoIdRPrcF1(pContratoId:number){
    return this.http.get<ContratoList>(`${environment.apiUrl}/RegisterPreContructionPhase1/GetContratacionByContratoId?pContratoId=${pContratoId}`);
  }
  createEditContratoPerfil(pContrato:ContratoList){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterPreContructionPhase1/CreateEditContratoPerfil`,pContrato);
  }
  deleteContratoPerfil(ContratoPerfilId:number){
    return this.http.delete<ContratoList>(`${environment.apiUrl}/RegisterPreContructionPhase1/DeleteContratoPerfil?ContratoPerfilId=${ContratoPerfilId}`);
  }
}

export interface ContratacionList{

}

export interface ContratoList{

}