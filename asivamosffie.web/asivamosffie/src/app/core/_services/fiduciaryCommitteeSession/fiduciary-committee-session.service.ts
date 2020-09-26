import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ComiteGrilla, ComiteTecnico, SesionComiteSolicitud, SesionComiteTema, SesionParticipante, SolicitudesContractuales } from 'src/app/_interfaces/technicalCommitteSession';
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

  convocarComiteTecnico( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/convocarComiteTecnico`, comite);    
  }

  cambiarEstadoComiteTecnico( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/cambiarEstadoComiteTecnico`, comite);    
  }

  getComiteTecnicoByComiteTecnicoId( id: number){
    return this.http.get<ComiteTecnico>(`${environment.apiUrl}/CommitteeSessionFiduciario/getComiteTecnicoByComiteTecnicoId?pComiteTecnicoId=${ id }`);
  }

  getPlantillaByTablaIdRegistroId(pTablaId: string, pRegistroId: number)
  {
    return this.http.get(`${environment.apiUrl}/CommitteeSessionFiduciario/getPlantillaByTablaIdRegistroId?pTablaId=${ pTablaId }&pRegistroId=${ pRegistroId }`, { responseType: "blob" });
  }

  createEditSesionComiteTema(lista: SesionComiteTema[]){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/createEditSesionComiteTema`, lista);    
  }

  aplazarSesionComite( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/aplazarSesionComite`, comite );
   }

  getSesionParticipantesByIdComite( id: number ){
    return this.http.get<SesionParticipante[]>(`${environment.apiUrl}/CommitteeSessionFiduciario/getSesionParticipantesByIdComite?pComiteId=${ id }`);
   }

   deleteSesionInvitado( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/deleteSesionInvitado?pSesionInvitadoId=${ id }`);
   }

   createEditSesionInvitadoAndParticipante( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/createEditSesionInvitadoAndParticipante`, comite );
   }
   
   noRequiereVotacionSesionComiteSolicitud( sesionComiteSolicitud: SesionComiteSolicitud ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/noRequiereVotacionSesionComiteSolicitud`, sesionComiteSolicitud);
   }

   noRequiereVotacionSesionComiteTema( sesionComiteTema: SesionComiteTema, pRequiereVotacion: boolean ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/noRequiereVotacionSesionComiteTema?idSesionComiteTema=${ sesionComiteTema.sesionTemaId }&pRequiereVotacion=${ pRequiereVotacion }`, sesionComiteTema );
   }

   createEditSesionSolicitudVoto( sesionComiteSolicitud: SesionComiteSolicitud ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/CommitteeSessionFiduciario/createEditSesionSolicitudVoto`, sesionComiteSolicitud );
   }

}
