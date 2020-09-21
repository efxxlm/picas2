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
    esAvanceobra?: boolean,
    porcentajeAvanceObra?: number,
    requiereLicencia?: boolean,
    licenciaVigente?: boolean,
    numeroLicencia?: string,
    fechaVigencia?: Date,
    tieneMonitoreWeb ?: boolean,
    contratacionProyectoAportante?: ContratacionProyectoAportante[],    
    proyecto?: any, 
    tipoIntervencionCodigo?: string,
    tipoSolicitudCodigo?: string,

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

export interface ContratacionProyectoAportante{
    contratacionProyectoAportanteId?: number,
    contratacionProyectoId?: number,
    proyectoAportanteId?: number,
    valorAporte?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    usuarioModificacion?: string,
    fechaModificacion?: Date,
    eliminado?: number,
    componenteAportante?: ComponenteAportante[],

}

export interface ComponenteAportante{
    componenteAportanteId?: number,
    contratacionProyectoAportanteId?: number,
    tipoComponenteCodigo?: string,
    faseCodigo?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    componenteUso?: ComponenteUso[],

}

export interface ComponenteUso{
    componenteUsoId?: number,
    componenteAportanteId?: number,
    tipoUsoCodigo?: string,
    valorUso?: number,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    eliminado?: boolean,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
}

interface EstadoSolicitud{
    AprobadaPorComiteTecnico: string,
    AprobadaPorComiteFiduciario: string,
    RechazadaPorComiteTecnico: string,
    RechazadaPorComiteFiduciario: string,
    DevueltaPorComiteTecnico: string,
    DevueltaPorComiteFiduciario: string,
    EnTramite: string,
  }
  
  export const EstadosSolicitud: EstadoSolicitud = {
    AprobadaPorComiteTecnico: '1',
    AprobadaPorComiteFiduciario: '2',
    RechazadaPorComiteTecnico: '3',
    RechazadaPorComiteFiduciario: '4',
    DevueltaPorComiteTecnico: '5',
    DevueltaPorComiteFiduciario: '6',
    EnTramite: '7',
  }

  export interface ContratacionObservacion{
    contratacionObservacionId?: number,
    contratacionId?: number,
    observacion?: string,
    usuarioCreacion?: string,
    fechaCreacion?: Date,
    comiteTecnicoId?: number,
  }
  







