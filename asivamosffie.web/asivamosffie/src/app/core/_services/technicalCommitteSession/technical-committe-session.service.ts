import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Respuesta } from '../common/common.service';
import { environment } from 'src/environments/environment';
import { SolicitudesContractuales, SesionComiteTema, ComiteGrilla, ComiteTecnico, SesionComiteSolicitud, SesionTemaVoto } from 'src/app/_interfaces/technicalCommitteSession';
import { Session } from 'protractor';


@Injectable({
  providedIn: 'root'
})
export class TechnicalCommitteSessionService {

  constructor(
              private http: HttpClient,

             ) 
  { }

  getListSolicitudesContractuales( fechaComite ){
    return this.http.get<SolicitudesContractuales[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/GetListSesionComiteSolicitudByFechaOrdenDelDia?pFechaComite=${ fechaComite }`);
  }

  createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud( comite: ComiteTecnico ){
     return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditComiteTecnicoAndSesionComiteTemaAndSesionComiteSolicitud`, comite );
   }

  getListComiteGrilla(){
     return this.http.get<ComiteGrilla[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getListComiteGrilla`);
   }

  cambiarEstadoComiteTecnico( comite: ComiteTecnico ){
     return this.http.put<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/CambiarEstadoComiteTecnico`, comite);
   }

  getComiteTecnicoByComiteTecnicoId( id: number ){
     return this.http.get<ComiteTecnico>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/GetComiteTecnicoByComiteTecnicoId?pComiteTecnicoId=${ id }`);
   }

   createEditSesionComiteTema( lista: SesionComiteTema[] ){
     return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditSesionComiteTema`, lista );
   }

   createEditSesionInvitadoAndParticipante( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditSesionInvitadoAndParticipante`, comite );
   }

   createEditSesionSolicitudVoto( sesionComiteSolicitud: SesionComiteSolicitud ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditSesionSolicitudVoto`, sesionComiteSolicitud );
   }

   convocarComiteTecnico( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/convocarComiteTecnico`, comite );
   }

   createEditSesionTemaVoto( tema: SesionTemaVoto ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditSesionTemaVoto`, tema );
   }

}
