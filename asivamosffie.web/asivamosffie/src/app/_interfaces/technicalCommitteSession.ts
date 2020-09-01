import { Usuario } from '../core/_services/autenticacion/autenticacion.service';
import { Contratacion } from './project-contracting';
import { Proyecto } from '../core/_services/project/project.service';

export interface SolicitudesContractuales{
    id?: number,
    fechaSolicitud?: Date,
    tipoSolicitud?: string,
    tipoSolicitudNumeroTabla?: string,
    numeroSolicitud?: string,
    sesionComiteSolicitudId?: number

}

export interface ComiteTecnico{
    comiteTecnicoId?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    eliminado?: boolean,
    esCompleto?: boolean,
    requiereVotacion?: boolean,
    justificacion?: string,
    esAprobado?: boolean,
    fechaAplazamiento?: Date,
    observaciones?: string,
    rutaSoporteVotacion?: string,
    tieneCompromisos?: boolean,
    cantCompromisos?: number,
    rutaActaSesion?: string,
    fechaOrdenDia?: Date,
    numeroComite?: string,
    estadoComiteCodigo?: string,

    sesionComiteTema?: SesionComiteTema[],
    sesionComiteSolicitud?: SesionComiteSolicitud[],
    sesionParticipante?: SesionParticipante[],
    sesionInvitado?: SesionInvitado[],

}

export interface SesionComiteTema{
    sesionTemaId?: number,
    tema?: string,
    responsableCodigo?: string,
    tiempoIntervencion?: number,
    rutaSoporte?: string,
    observaciones?: string,
    esAprobado?: boolean,
    observacionesDecision?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    eliminado?: boolean,
    comiteTecnicoId?: number,
    esProposicionesVarios?: boolean,
    requiereVotacion?: boolean,

    sesionTemaVoto?: SesionTemaVoto[]

}

export interface SesionComiteSolicitud {
    sesionComiteSolicitudId?: number,
    tipoSolicitudCodigo?: string,
    solicitudId?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    comiteTecnicoId?: number,
    estadoCodigo?: string,
    observaciones?: string,
    rutaSoporteVotacion?: string,
    generaCompromiso?: boolean,
    cantCompromisos?: number,
    eliminado?: boolean,
    requiereVotacion?: boolean,

    tipoSolicitud?: string,
    numeroSolicitud?: string,
    fechaSolicitud?: Date, 

    sesionSolicitudVoto?: SesionSolicitudVoto[],
    sesionSolicitudObservacionProyecto?: SesionSolicitudObservacionProyecto[],
    contratacion?: Contratacion,


}

export interface SesionParticipante{
    sesionParticipanteId?: number,
    comiteTecnicoId?: number,
    usuarioId?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,

    usuario?: Usuario,
    sesionSolicitudVoto?: SesionSolicitudVoto[],
    sesionTemaVoto?: SesionTemaVoto[],
    sesionSolicitudObservacionProyecto?: SesionSolicitudObservacionProyecto[],

}

export interface SesionInvitado{
    sesionInvitadoId?: number,
    nombre?: string,
    cargo?: string,
    entidad?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    eliminado?: boolean,
    comiteTecnicoId?: number,

}

export interface SesionSolicitudVoto{
    sesionSolicitudVotoId?: number,
    sesionComiteSolicitudId?: number,
    sesionParticipanteId?: number,
    esAprobado?: boolean,
    observacion?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,
    usuarioModificacion?: string,
    fechaModificacion?: Date,

    nombreParticipante?: string,

    sesionComiteSolicitud?: SesionComiteSolicitud

}

export interface SesionTemaVoto{
    sesionTemaVotoId?: number,
    sesionTemaId?: number,
    sesionParticipanteId?: number,
    esAprobado?: boolean,
    observacion?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    eliminado?: boolean,

    nombreParticipante?: string,

}

export interface ComiteGrilla{
    id?: number,
    fechaComite?: string,
    numeroComite?: string,
    estadoComite?: string, 
    estadoComiteCodigo?: string,
    
}

export interface SesionSolicitudObservacionProyecto{
    sesionSolicitudObservacionProyectoId?: number,
    sesionComiteSolicitudId?: number,
    contratacionProyectoId?: number,
    sesionParticipanteId?: number,
    observacion?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    usuarioModificacion?: string,
    fechaModificacion?: Date,
    eliminado?: boolean,

    proyecto?: Proyecto,

    nombreParticipante?: string,

}

interface EstadoComite{
    sinConvocatoria?: string,
    convocada?: string,
    desarrolladaSinActa?: string,
    conActaDeSesionEnviada?: string,
    conActaDeSesionAprobada?: string,
    aplazada?: string,
    fallida?: string,
  }
  
  export const EstadosComite: EstadoComite = {
    sinConvocatoria: "1",
    convocada: "2",
    desarrolladaSinActa: "3",
    conActaDeSesionEnviada: "4",
    conActaDeSesionAprobada: "5",
    aplazada: "6",
    fallida: "7",

  }

  interface TipoSolicitud{
    AperturaDeProcesoDeSeleccion?: string,
    Contratacion?: string,
    ModificacionContractual?: string,
    
  }
  
  export const TiposSolicitud: TipoSolicitud = {
    AperturaDeProcesoDeSeleccion: "1",
    Contratacion: "2",
    ModificacionContractual: "3",

  }

