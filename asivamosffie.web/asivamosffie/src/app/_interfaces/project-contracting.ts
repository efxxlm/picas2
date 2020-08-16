export interface Contratacion {
    contratacionId?: number,
    tipoSolicitudCodigo?: string,
    fechaSolicitud?: Date,
    numeroSolicitud?: string,
    estadoSolicitudCodigo?: string,
    registroCompleto?: boolean,
    contratistaId?: number,
    esObligacionEspecial?: boolean,
    consideracionDescripcion?: string,
    eliminado?: boolean,
    fechaEnvioDocumentacion?: Date,
    observaciones?: string,
    rutaMinuta?: string,
    contratacionProyecto?: ContratacionProyecto[],

}

export interface ContratacionProyecto{
    contratacionProyectoId?: number,
    contratacionId?: number,
    proyectoId?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,
    esReasignacion?: boolean,
    esAvanceObra?: boolean,
    porcentajeAvanceObra?: number,
    requiereLicencia?: boolean,
    licenciaVigente?: boolean,
    numeroLicencia?: string,
    fechaVigencia?: Date,
    contempladaServicioMonitoreo?: boolean,
    

}

export interface ContratistaGrilla{
    idContratista?: number,
    nombre?: string,
    representanteLegal?: string,
    numeroInvitacion?: string,
    numeroIdentificacion?: string,
    esConsorcio?: boolean,
}

export interface ProyectoGrilla{
    proyectoId?: string,
    departamento?: string,
    municipio?: string,
    institucionEducativa?: string,
    sede?: string,
    estadoRegistro?: string,
    estadoJuridicoPredios?: string,
    fecha?: string,
    tipoIntervencion?: string,
    llaveMen?: string,
    region?: string,
}
