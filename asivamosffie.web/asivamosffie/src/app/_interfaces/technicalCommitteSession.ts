import { Usuario } from '../core/_services/autenticacion/autenticacion.service';
import { Contratacion } from './project-contracting';
import { Proyecto } from '../core/_services/project/project.service';
import { dashCaseToCamelCase } from '@angular/compiler/src/util';
import { ProcesoSeleccion, ProcesoSeleccionCronograma } from '../core/_services/procesoSeleccion/proceso-seleccion.service';

export interface SolicitudesContractuales{
    id?: number,
    fechaSolicitud?: Date,
    tipoSolicitud?: string,
    tipoSolicitudNumeroTabla?: string,
    numeroSolicitud?: string,
    sesionComiteSolicitudId?: number,
    tipoSolicitudCodigo?: string,
    solicitudId?: number,
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
    numeroCompromisos?: number,
    numeroCompromisosCumplidos?: number,
    tipoTemaFiduciarioCodigo?: string,
    estadoActaCodigo?: string,

    sesionComiteTema?: SesionComiteTema[],
    sesionComiteSolicitudComiteTecnico?: SesionComiteSolicitud[],
    sesionComiteSolicitudComiteTecnicoFiduciario?: SesionComiteSolicitud[],
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
    estadoTemaCodigo?: string,
    generaCompromiso?: boolean,
    cantCompromisos?: number,
    registroCompleto?: boolean,

    completo?: boolean,

    sesionTemaVoto?: SesionTemaVoto[]
    temaCompromiso?: TemaCompromiso[],

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
    registroCompleto?: boolean,
    desarrolloSolicitud?: string,

    comiteTecnicoFiduciarioId?: number,
    registroCompletoFiduciaria?: boolean,
    desarrolloSolicitudFiduciario?: string,
    estadoActaCodigoFiduciario?: string,
    observacionesFiduciario?: string,
    rutaSoporteVotacionFiduciario?: string,
    generaCompromisoFiduciario?: boolean,
    cantCompromisosFiduciario?: number,
    requiereVotacionFiduciario?: boolean,

    tipoSolicitud?: string,
    numeroSolicitud?: string,
    fechaSolicitud?: Date, 
    completo?: boolean,

    sesionSolicitudVoto?: SesionSolicitudVoto[],
    sesionSolicitudObservacionProyecto?: SesionSolicitudObservacionProyecto[],
    sesionSolicitudCompromiso?: SesionSolicitudCompromiso[],
    contratacion?: Contratacion,
    procesoSeleccion?: ProcesoSeleccion,
    procesoSeleccionMonitoreo?: ProcesoSeleccionMonitoreo,
    sesionSolicitudObservacionActualizacionCronograma?: SesionSolicitudObservacionActualizacionCronograma[],


}

export interface ProcesoSeleccionMonitoreo{
    procesoSeleccionMonitoreoId?: number,
    procesoSeleccionId?: number,
    numeroProceso?: string,
    estadoActividadCodigo?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    enviadoComiteTecnico?: boolean,

    procesoSeleccionCronogramaMonitoreo?: ProcesoSeleccionCronogramaMonitoreo[],
}

export interface ProcesoSeleccionCronogramaMonitoreo{
    procesoSeleccionCronogramaMonitoreoId?: number,
    numeroActividad?: number,
    descripcion?: string,
    fechaMaxima?: Date,
    estadoActividadCodigo?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    procesoSeleccionMonitoreoId?: number,
    procesoSeleccionCronogramaId?: number,

    sesionSolicitudObservacionActualizacionCronograma?: SesionSolicitudObservacionActualizacionCronograma[],
    procesoSeleccionCronograma: ProcesoSeleccionCronograma,
}

export interface SesionSolicitudObservacionActualizacionCronograma{
    sesionSolicitudObservacionActualizacionCronogramaId?: number,
    sesionComiteSolicitudId?: number,
    procesoSeleccionCronogramaMonitoreoId?: number,
    sesionParticipanteId?: number,
    observacion?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    usuarioModificacion?: string,
    fechaModificacion?: Date,
    eliminado?: boolean,

    nombreParticipante?: string,
    procesoSeleccionCronograma?: ProcesoSeleccionCronograma
    procesoSeleccionCronogramaMonitoreo?: ProcesoSeleccionCronogramaMonitoreo,
}

export interface SesionParticipante{
    sesionParticipanteId?: number,
    comiteTecnicoId?: number,
    usuarioId?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,

    nombre?: string;

    usuario?: Usuario,
    sesionSolicitudVoto?: SesionSolicitudVoto[],
    sesionTemaVoto?: SesionTemaVoto[],
    sesionSolicitudObservacionProyecto?: SesionSolicitudObservacionProyecto[],
    sesionSolicitudObservacionActualizacionCronograma?: SesionSolicitudObservacionActualizacionCronograma[],

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
    comiteTecnicoFiduciarioId?: number,

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
    estadoActa?: string,
    estadoActaCodigo?: string,
    registroCompleto?: boolean,
    registroCompletoNombre?: string,
    numeroCompromisos?: number,
    numeroCompromisosCumplidos?: number,
    esComiteFiduciario?: boolean,
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

export interface SesionSolicitudCompromiso{
    sesionSolicitudCompromisoId?: number,
    sesionComiteSolicitudId?: number,
    tarea?: string,
    responsableSesionParticipanteId?: number,
    fechaCumplimiento?: Date,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    eliminado?: boolean,
    estadoCodigo?: string,
    esFiduciario?: boolean,


    nombreResponsable?: string,
    responsableSesionParticipante?: SesionParticipante,
    nombreEstado?: string,

}

export interface TemaCompromiso{
    temaCompromisoId?: number,
    sesionTemaId?: number,
    tarea?: string,
    responsable?: string,
    fechaCumplimiento?: Date,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: Date,
    eliminado?: boolean,
    estadoCodigo?: string,

    nombreResponsable?: string,
    responsableNavigation?: SesionParticipante,
    nombreEstado?: string,

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
    ActualizacionCronogramaProcesoseleccion?: string,
  }
  
  export const TiposSolicitud: TipoSolicitud = {
    AperturaDeProcesoDeSeleccion: "1",
    Contratacion: "2",
    ModificacionContractual: "3",
    ActualizacionCronogramaProcesoseleccion: "6",

  }

