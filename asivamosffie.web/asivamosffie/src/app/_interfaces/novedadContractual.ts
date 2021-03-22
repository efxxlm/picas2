import { Contrato } from "./faseUnoPreconstruccion.interface";

export interface NovedadContractual{
        novedadContractualId?: number,
        fechaSolictud?: Date,
        numeroSolicitud?: string,
        instanciaCodigo?: string,
        fechaSesionInstancia?: Date,
        fechaCreacion?: Date,
        usuarioCreacion?: string,
        eliminado?: boolean,
        registroCompleto?: boolean,
        fechaModificacion?: Date,
        usuarioModificacion?: string,
        esAplicadaAcontrato?: boolean,
        contratoId?: number,
        proyectoId?: number,
        urlSoporte?: string,
        obervacionSupervisorId?: number,
        tieneObservacionesApoyo?: boolean,
        tieneObservacionesSupervisor?: boolean,
        fechaVerificacion?: Date,
        fechaValidacion?: Date,
        registroCompletoVerificacion?: boolean,
        estadoCodigo?: string,
        causaRechazo?: string,
        registroCompletoTramiteNovedades?: boolean,
        fechaEnvioGestionContractual?: Date,
        estadoProcesoCodigo?: string,
        abogadoRevisionId?: number,
        deseaContinuar?: boolean,
        fechaEnvioActaContratistaObra?: Date,
        fechaFirmaActaContratistaObra?: Date,
        fechaEnvioActaContratistaInterventoria?: Date,
        fechaFirmaContratistaInterventoria?: Date,
        fechaEnvioActaApoyo?: Date,
        fechaFirmaApoyo?: Date,
        fechaEnvioActaSupervisor?: Date,
        fechaFirmaSupervisor?: Date,
        urlSoporteFirmas?: string,
        observacionesDevolucionId?: number,
        razonesNoContinuaProceso?: string,

        instanciaNombre?: string,

        novedadContractualDescripcion?: NovedadContractualDescripcion[],
        contrato?: Contrato,
        proyectosContrato?: any[],
        novedadContractualObservaciones?: NovedadContractualObservaciones[],

        observacionApoyo?: NovedadContractualObservaciones,
        


}

export interface NovedadContractualDescripcion{
        novedadContractualDescripcionId?: number,
        novedadContractualId?: number,
        tipoNovedadCodigo?: string,
        motivoNovedadCodigo?: string,
        resumenJustificacion?: string,
        esDocumentacionSoporte?: boolean,
        conceptoTecnico?: string,
        fechaConcepto?: Date,
        fechaInicioSuspension?: Date,
        fechaFinSuspension?: Date,
        presupuestoAdicionalSolicitado?: number,
        plazoAdicionalDias?: number,
        plazoAdicionalMeses?: number,
        clausulaModificar?: string,
        ajusteClausula?: string,
        fechaCreacion?: Date,
        usuarioCreacion?: string,
        eliminado?: boolean,
        registroCompleto?: boolean,
        fechaModificacion?: Date,
        usuarioModificacion?: string,
        numeroRadicado?: string,

        nombreTipoNovedad?: string,
        
        novedadContractualDescripcionMotivo?: NovedadContractualDescripcionMotivo[],
        novedadContractualClausula?: NovedadContractualClausula[],

}

export interface NovedadContractualClausula{
        novedadContractualClausulaId?: number,
        novedadContractualDescripcionId?: number,
        clausulaAmodificar?: string,
        ajusteSolicitadoAclausula?: string,
        eliminado?: boolean,
}

export interface NovedadContractualDescripcionMotivo{
        novedadContractualDescripcionMotivoId?: number,
        novedadContractualDescripcionId?: number,
        motivoNovedadCodigo?: string,
        eliminado?: boolean,
}

export interface NovedadContractualObservaciones{
        novedadContractualObservacionesId?:number,
        novedadContractualId?:number,
        observaciones?:string,
        esSupervision?:boolean,
        archivado?:boolean,
        esTramiteNovedades?:boolean,
}