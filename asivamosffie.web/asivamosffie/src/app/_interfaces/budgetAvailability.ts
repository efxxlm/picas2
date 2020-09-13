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
    estadoSolicitudCodigo?: string,
    objeto?: string,
    eliminado?: boolean,
    rutaDdp?: string,
    observacion?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    registroCompleto?: boolean,
    contratacionId?: number,

    plazoMeses: number,
    plazoDias: number,

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