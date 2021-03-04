export interface InformeFinal {
        informeFinalId?: number,
        proyectoId?: number,
        usuarioCreacion?: string,
        fechaCreacion?: Date,
        usuarioModificacion?: string,
        fechaModificacion?: Date,
        estadoInforme?: string,
        registroCompleto?: boolean,
        fechaSuscripcion?: Date,
        urlActa?: string,
        eliminado?: boolean,
        estadoValidacion?: string,
        observacionesValidacion?: string,
        tieneObservacionesValidacion?: boolean,
        registroCompletoValidacion?: boolean,
        informeFinalInterventoria?: InformeFinalInterventoria[]
}

export interface InformeFinalInterventoria{
        informeFinalInterventoriaId?: number,
        informeFinalId?: number,
        usuarioCreacion?: string,
        fechaCreacion?: Date,
        usuarioModificacion?: string,
        fechaModificacion?: Date,
        calificacionCodigo?: string,
        informeFinalAnexoId?: number,
        informeFinalListaChequeoId?: number,
        fechaEnvioSupervisor?: Date,
        fechaAprobacion?: Date,
        tieneObservacionSupervisor?: boolean,
        validacionCodigo?: string,
        aprobacionCodigo?: string,
        informeFinalAnexo?:{
                informeFinalAnexoId?: number,
                fechaCreacion?: Date,
                usuarioCreacion?: string,
                fechaModificacion?: Date,
                usuarioModificacion?: number,
                tipoAnexo?: string,
                numRadicadoSac?: number,
                fechaRadicado?: Date,
                urlSoporte?: string
              },
        informeFinalInterventoriaObservaciones?: InformeFinalInterventoriaObservaciones[],
        tieneModificacionInterventor?: boolean
}

export interface InformeFinalAnexo{
        informeFinalAnexoId?: number,
        fechaCreacion?: Date,
        usuarioCreacion?: string,
        fechaModificacion?: Date,
        usuarioModificacion?: number,
        tipoAnexo?: string,
        numRadicadoSac?: number,
        fechaRadicado?: Date,
        urlSoporte?: string
}

export interface InformeFinalInterventoriaObservaciones{
        informeFinalInterventoriaObservacionesId?:number,
        informeFinalInterventoriaId?:number,
        observaciones?: string,
        esSupervision?: boolean,
        esCalificacion?: boolean,
        esApoyo?: boolean,
        archivado?: boolean,
}