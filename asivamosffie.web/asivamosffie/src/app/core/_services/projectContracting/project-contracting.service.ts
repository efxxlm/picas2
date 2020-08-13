import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Contratacion, ContratacionProyecto } from 'src/app/_interfaces/project-contracting';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectContractingService {

  constructor(
                private http: HttpClient,
                
             ) 
  { }

  createContratacionProyecto( contratacion: Contratacion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ProjectContracting/CreateContratacionProyecto`, contratacion);
  }
  getListContratacionProyectoByContratacionId( contratacionId: number ){
    return this.http.get<ContratacionProyecto[]>(`${environment.apiUrl}/ProjectContracting/GetListContratacionProyectoByContratacionId?idContratacion=${ contratacionId }`);
  }

  getListContratacion(){
    return this.http.get<Contratacion[]>(`${environment.apiUrl}/ProjectContracting/getListContratacion`);
  }

}
