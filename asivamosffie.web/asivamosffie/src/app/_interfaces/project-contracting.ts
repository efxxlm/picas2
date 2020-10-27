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
    porcentajeAvanceObra?: string,
    requiereLicencia?: boolean,
    licenciaVigente?: boolean,
    dataAportantes?: any[],
    numeroLicencia?: string,
    fechaVigencia?: Date,
    tieneMonitoreoWeb?: boolean,
    contratacionProyectoAportante?: ContratacionProyectoAportante[],    
    proyecto?: any, 
    tipoIntervencionCodigo?: string,
    tipoSolicitudCodigo?: string,

    proyectoGrilla?: ProyectoGrilla,

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
    estadoProyectoCodigo?: string,
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
    contratacionProyectoid?: number,
    
  }
  
  interface EstadoProyecto{
    Disponible?: string,
    AsignadoSolicitudContratacion?: string,
    AprobadoComiteTecnico?: string,
    AprobadoComiteFiduciario?: string,
    RechazadoComiteTecnico?: string,
    RechazadoComiteFiduciario?: string,
    DevueltoComiteTecnico?: string,
    DevueltoComiteFiduciario?: string,
  }
  
  export const EstadosProyecto: EstadoProyecto = {
    Disponible : "1",
    AsignadoSolicitudContratacion : "2",
    AprobadoComiteTecnico : "3",
    AprobadoComiteFiduciario : "4",
    RechazadoComiteTecnico : "5",
    RechazadoComiteFiduciario : "6",
    DevueltoComiteTecnico : "7",
    DevueltoComiteFiduciario : "8",
  }

  interface EstadosSolicitudCronograma{
    Creada: string,
    AprobadaPorComiteTecnico: string,
    AprobadaPorComiteFiduciario: string,
    RechazadaPorComiteTecnico: string,
    RechazadaPorComiteFiduciario: string,
    DevueltaPorComiteTecnico: string,
    DevueltaPorComiteFiduciario: string,
    EnTramite: string,
  }
  export const EstadosSolicitudCronograma: EstadosSolicitudCronograma = {
    Creada: '1',
    AprobadaPorComiteTecnico: '2',
    AprobadaPorComiteFiduciario: '3',
    RechazadaPorComiteTecnico: '4',
    RechazadaPorComiteFiduciario: '5',
    DevueltaPorComiteTecnico: '6',
    DevueltaPorComiteFiduciario: '7',
    EnTramite: '8',
  }