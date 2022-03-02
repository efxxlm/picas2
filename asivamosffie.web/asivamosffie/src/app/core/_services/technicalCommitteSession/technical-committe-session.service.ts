import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Respuesta } from '../common/common.service';
import { environment } from 'src/environments/environment';
import { SolicitudesContractuales, SesionComiteTema, ComiteGrilla, ComiteTecnico, SesionComiteSolicitud, SesionTemaVoto, SesionSolicitudCompromiso, SesionSolicitudObservacionProyecto, SesionParticipante, ProcesoSeleccionMonitoreo } from 'src/app/_interfaces/technicalCommitteSession';
import { Session } from 'protractor';
import { ProyectoGrilla, ContratacionObservacion } from 'src/app/_interfaces/project-contracting';
import { SesionComentario } from 'src/app/_interfaces/compromisos-actas-comite.interfaces';


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

  getListComite( EsFiduciario: string ){
    return this.http.get<ComiteGrilla[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/ListMonitoreo?EsFiduciario=${ EsFiduciario }`);
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

   enviarComiteParaAprobacion( comite: ComiteTecnico ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/EnviarComiteParaAprobacion`, comite );
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

   deleteSesionResponsable( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/deleteSesionResponsable?pSesionResponsableId=${ id }`);
   }

   deleteComiteTecnicoByComiteTecnicoId( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/deleteComiteTecnicoByComiteTecnicoId?pComiteTecnicoId=${ id }`);
   }

   noRequiereVotacionSesionComiteSolicitud( sesionComiteSolicitud: SesionComiteSolicitud ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/noRequiereVotacionSesionComiteSolicitud`, sesionComiteSolicitud);
   }

   noRequiereVotacionSesionComiteTema( sesionComiteTema: SesionComiteTema, pRequiereVotacion: boolean ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/noRequiereVotacionSesionComiteTema?idSesionComiteTema=${ sesionComiteTema.sesionTemaId }&pRequiereVotacion=${ pRequiereVotacion }`, sesionComiteTema );
   }

   getCompromisosByComiteTecnicoId( id: number, esFiduciario: boolean){
    return this.http.get<ComiteTecnico>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getCompromisosByComiteTecnicoId?ComiteTecnicoId=${ id }&pEsFiduciario=${ esFiduciario }`);
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

   getPlantillaByTablaIdRegistroId( pTablaId: string, pRegistroId: number , pComiteTecnicoId: number){
    return this.http.get(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getPlantillaByTablaIdRegistroId?pTablaId=${ pTablaId }&pRegistroId=${ pRegistroId }&pComiteTecnicoId=${ pComiteTecnicoId }`, { responseType: "blob" } );
   }

   getSesionParticipantesByIdComite( id: number ){
    return this.http.get<SesionParticipante[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getSesionParticipantesByIdComite?pComiteId=${ id }`);
   }

   getPlantillaActaBySesionComiteSolicitudId( id: number ){
    return this.http.get(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/GetPlantillaActaIdComite?IdComite=${ id }`, { responseType: "blob" } );
   }

   getPlantillaActaBySesionComiteSolicitudFiduciarioId( id: number ){
    return this.http.get(`${environment.apiUrl}/CommitteeSessionFiduciario/GetPlantillaActaIdComite?IdComite=${ id }`, { responseType: "blob" } );
   }

   getCometariosDelActa( id: number ){
    return this.http.get<SesionComentario[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getCometariosDelActa?pComietTecnicoId=${ id }`);
   }

   getProcesoSeleccionMonitoreo( id: number ){
    return this.http.get<ProcesoSeleccionMonitoreo>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getProcesoSeleccionMonitoreo?pProcesoSeleccionMonitoreoId=${ id }`);
   }

   eliminarCompromisosSolicitud( id ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/EliminarCompromisosSolicitud?pSesionComiteSolicitudId=${ id }`);
   }

   eliminarCompromisosTema( id ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/EliminarCompromisosTema?pSesionTemaId=${ id }`);
   }

   observacionesCompromisos ( pObservacionComentario ) {
     return this.http.post<Respuesta>( `${environment.apiUrl}/RegisterSessionTechnicalCommittee/ObservacionesCompromisos`, pObservacionComentario );
   };

   deleteSesionComiteCompromiso( id ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/DeleteSesionComiteCompromiso?pSesionComiteTemaId=${ id }`);
   }

   deleteTemaCompromiso( id ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/deleteTemaCompromiso?pTemaCompromisoId=${ id }`);
   }

   getValidarcompletosActa( id: number ){
    return this.http.get<boolean>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/GetValidarcompletosActa?IdComite=${ id }` );
   }

}
