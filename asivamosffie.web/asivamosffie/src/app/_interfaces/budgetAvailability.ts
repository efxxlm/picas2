import { Proyecto } from '../core/_services/project/project.service';

export interface GrillaDisponibilidadPresupuestal{
    disponibilidadPresupuestalId?: number,
    fechaSolicitud?: Date,
    tipoSolicitudCodigo?: string,
    tipoSolicitudText?: string,
    numeroSolicitud?: string,
    opcionPorContratarCodigo?: string,
    opcionPorContratarText?: string,
    valorSolicitado?: number,
    estadoSolicitudCodigo?: string,
    estadoSolicitudText?: string,
    estadoRegistro?: string,


}

export interface DisponibilidadPresupuestal{
    disponibilidadPresupuestalId?: number,
    fechaSolicitud?: Date,
    tipoSolicitudCodigo?: string,
    numeroSolicitud?: string,
    opcionContratarCodigo?: string,
    valorSolicitud?: number,
    estadoSolicitudCodigo?: string,
    objeto?: string,
    eliminado?: boolean,
    fechaDdp?: Date,
    numeroDdp?: string,
    rutaDdp?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    registroCompleto?: boolean,
    contratacionId?: number,
    numeroDrp?: string,
    plazoMeses?: number,
    plazoDias?: number,
    cuentaCartaAutorizacion?: boolean,
    aportanteId?: number,
    valorAportante?: number,
    numeroContrato?: string,
    limitacionEspecial?: string,
    numeroRadicadoSolicitud?: string,
    urlSoporte?: string,

    disponibilidadPresupuestalProyecto?: DisponibilidadPresupuestalProyecto[],

}

export interface DisponibilidadPresupuestalProyecto{
    disponibilidadPresupuestalProyectoId?: number,
    disponibilidadPresupuestalId?: number,
    proyectoId?: number,
    usuarioCreacion?: string,
    fechaCreacion?: Date,
    usuarioModificacion?: string,
    fechaModificacion?: Date,
    eliminado?: boolean,

    proyecto?: Proyecto,
    

}

export interface CustonReuestCommittee{
    contratacionId?: number,
    disponibilidadPresupuestalId?: number,
    sesionComiteSolicitudId?: number,
    fechaSolicitud?: Date,
    fechaComite?: Date,
    tipoSolicitudCodigo?: string,
    numeroSolicitud?: string,
    valorSolicitud?: number,
    tipoSolicitudText?: string,
    opcionContratar?: string,
    valorSolicitado?: number,
    estadoSolicitudCodigo?: string,
    estadoSolicitudText?: string,
    
}

export interface ListAportantes{
    contratacionProyectoId?: number,
    contratacionProyectoAportanteId?: number,
    contratacionId?: number,
    cofinanciacionAportanteId?: number,
    tipoAportanteId?: number,
    tipoAportanteText?: string,
    nombreAportanteId?: number,
    nombreAportante?: string,
    valorAporte?: number,
}