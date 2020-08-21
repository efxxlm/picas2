export interface SolicitudesContractuales{
    id?: number,
    fechaSolicitud?: Date,
    tipoSolicitud?: string,
    tipoSolicitudNumeroTabla?: number,
    numeroSolicitud?: string,

}

export interface Sesion{
    sesionId?: number,
    fechaOrdenDia?: Date,
    numeroComite?: string,
    estadoComiteCodigo?: string,
    esCompleto?: boolean,
    rutaActaSesion?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    eliminado?: boolean,
    sesionComiteTema?: SesionComiteTema[],

}

export interface SesionComiteTema{
    sesionTemaId?: number,
    sesionId?: number,
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

}

export interface SesionComiteTecnico{
    sesionComiteTecnicoId?: number,
    sesionId?: number,
    comiteTecnicoId?: number,
    requiereVotacion?: boolean,
    justificacion?: string,
    esAprobado?: boolean,
    observaciones?: string,
    rutaSoporteVotacion?: string,
    tieneCompromisos?: boolean,
    cantCompromisos?: number,
    usuarioCreacion?: string,
    fechaCreacion?: Date,
    usuarioModificacion?: string,
    fechaModificacion?: Date,
    eliminado?: boolean,
}

export interface ComiteGrilla{
    id?: number,
    fechaComite?: string,
    numeroComite?: string,
    estadoComite?: string, 
    estadoComiteCodigo?: string,
    
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
