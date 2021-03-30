export interface ControversiaContractual {
        controversiaContractualId?: number,
        tipoControversiaCodigo?: string,
        fechaSolicitud?: number,
        numeroSolicitud?: string,
        estadoCodigo?: string,
        esCompleto?: boolean,
        solicitudId?: number,
        contratoId?: number,
        fechaComitePreTecnico?: Date,
        conclusionComitePreTecnico?: string,
        esProcede?: boolean,
        numeroRadicadoSAC?: string,
        motivoJustificacionRechazo?: string,
        esRequiereComite?: boolean,
        rutaSoporte?: string,
        fechaCreacion?: Date,
        usuarioCreacion?: string,
        fechaModificacion?: Date,
        usuarioModificacion?: string,
        eliminado?: boolean,
        cualOtroMotivo?: string,
        controversiaMotivo?: ControversiaMotivo[]
}

export interface ControversiaMotivo{  
        controversiaMotivoId?: number,
        controversiaContractualId?: number,
        motivoSolicitudCodigo?: string,
        fechaCreacion?: Date,
        usuarioCreacion?: number,
        fechaModificacion?: Date,
        usuarioModificacion?: number
}