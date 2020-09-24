import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ComiteGrilla, ComiteTecnico, SolicitudesContractuales } from 'src/app/_interfaces/technicalCommitteSession';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class FiduciaryCommitteeSessionService {

  constructor(
                private http: HttpClient,

  ) 
  { }

  getCommitteeSessionFiduciario(){
    return this.http.get<SolicitudesContractuales[]>(`${environment.apiUrl}/CommitteeSessionFiduciario/getCommitteeSessionFiduciario`);
  }

  createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud`, comite);
  }

  getCommitteeSession(){
    return this.http.get<ComiteGrilla[]>(`${environment.apiUrl}/CommitteeSessionFiduciario/getCommitteeSession`);
  }

  getRequestCommitteeSessionById( id: number){
    return this.http.get<ComiteTecnico>(`${environment.apiUrl}/CommitteeSessionFiduciario/getRequestCommitteeSessionById?comiteTecnicoId=${ id }`);
  }

}
