import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Respuesta } from '../common/common.service';
import { environment } from 'src/environments/environment';
import { SolicitudesContractuales, SesionComiteTema, ComiteGrilla, ComiteTecnico, SesionComiteSolicitud, SesionTemaVoto, SesionSolicitudCompromiso, SesionSolicitudObservacionProyecto, SesionParticipante } from 'src/app/_interfaces/technicalCommitteSession';
import { Session } from 'protractor';
import { ProyectoGrilla, ContratacionObservacion } from 'src/app/_interfaces/project-contracting';


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

   aplazarSesionComite( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/aplazarSesionComite`, comite );
   }

   createEditActasSesionSolicitudCompromiso( sesionSolicitud: SesionSolicitudCompromiso ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditActasSesionSolicitudCompromiso`, sesionSolicitud );
   }

   createEditTemasCompromiso( tema: SesionComiteTema ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/createEditTemasCompromiso`, tema );
   }

   deleteSesionComiteTema( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/deleteSesionComiteTema?pSesionComiteTemaId=${ id }`);
   }

   deleteSesionInvitado( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/deleteSesionInvitado?pSesionInvitadoId=${ id }`);
   }

   deleteComiteTecnicoByComiteTecnicoId( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/deleteComiteTecnicoByComiteTecnicoId?pComiteTecnicoId=${ id }`);
   }

   noRequiereVotacionSesionComiteSolicitud( sesionComiteSolicitud: SesionComiteSolicitud ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/noRequiereVotacionSesionComiteSolicitud`, sesionComiteSolicitud);
   }

   noRequiereVotacionSesionComiteTema( sesionComiteTema: SesionComiteTema ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/noRequiereVotacionSesionComiteTema?idSesionComiteTema=${ sesionComiteTema.sesionTemaId }`, sesionComiteTema );
   }

   getCompromisosByComiteTecnicoId( id: number){
    return this.http.get<ComiteTecnico>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getCompromisosByComiteTecnicoId?ComiteTecnicoId=${ id }`); 
   }

   verificarTemasCompromisos( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/verificarTemasCompromisos`, comite );
   }

   crearObservacionProyecto( contratacionObservacion: ContratacionObservacion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/crearObservacionProyecto`, contratacionObservacion );
   }

   getSesionSolicitudObservacionProyecto( pSesionComiteSolicitudId: number, pContratacionProyectoId: number ){
    return this.http.get<SesionSolicitudObservacionProyecto[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getSesionSolicitudObservacionProyecto?pSesionComiteSolicitudId=${ pSesionComiteSolicitudId }&pContratacionProyectoId=${ pContratacionProyectoId }`);
   }

   getPlantillaByTablaIdRegistroId( pTablaId: string, pRegistroId: number ){
    return this.http.get(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getPlantillaByTablaIdRegistroId?pTablaId=${ pTablaId }&pRegistroId=${ pRegistroId }`, { responseType: "blob" } );
   }

   getSesionParticipantesByIdComite( id: number ){
    return this.http.get<SesionParticipante[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getSesionParticipantesByIdComite?pComiteId=${ id }`);
   }
  
}
