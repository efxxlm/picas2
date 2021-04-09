export interface LiquidacionContratacionObservacion {
        liquidacionContratacionObservacionId?: number,
        contratacionProyectoId?: number,
        tipoObservacionCodigo?: string,
        idPadre?: number,
        tieneObservacion?: boolean,
        registroCompleto?: boolean,
        observacion?: string,
        eliminado?: boolean,
        archivado?: boolean,
        usuarioCreacion?: string,
        fechaCreacion?: Date,
        usuarioModificacion?: string,
        fechaModificacion?: Date,
        menuId?: number
}

export interface HistoricoLiquidacionContratacionObservacion {
        historialObservaciones: LiquidacionContratacionObservacion[],
        obsVigente: {
                liquidacionContratacionObservacionId?: number,
                contratacionProyectoId?: number,
                tipoObservacionCodigo?: string,
                idPadre?: number,
                tieneObservacion?: boolean,
                registroCompleto?: boolean,
                observacion?: string,
                eliminado?: boolean,
                archivado?: boolean,
                usuarioCreacion?: string,
                fechaCreacion?: Date,
                usuarioModificacion?: string,
                fechaModificacion?: Date,
                menuId?: number
        }
}