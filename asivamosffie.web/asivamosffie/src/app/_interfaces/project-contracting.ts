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

}

export interface ContratistaGrilla{
    
}
