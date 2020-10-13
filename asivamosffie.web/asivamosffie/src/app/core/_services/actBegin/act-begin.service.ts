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
  GetVistaGenerarActaInicio(pContratoId:number){
    return this.http.get<GetVistaGenerarActaInicio>(`${environment.apiUrl}/actBegin/GetVistaGenerarActaInicio?pContratoId=${pContratoId}`);
  }
  GetListGrillaActaInicio(){
    return this.http.get<GetListGrillaActaInicio>(`${environment.apiUrl}/actBegin/GetListGrillaActaInicio`);
  }
  GetPlantillaActaInicio(pContratoId:number){
    return this.http.get<GetPlantillaActaInicio>(`${environment.apiUrl}/actBegin/GetPlantillaActaInicio?pContratoId=${pContratoId}`);
  }
}
export interface GetVistaGenerarActaInicio{

}
export interface GetListGrillaActaInicio{

}
export interface GetPlantillaActaInicio{

}