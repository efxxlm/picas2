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
        fechaAprobacionGestionContractual?: Date,
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
        registroCompletoInformacionBasica?: boolean,
        registroCompletoSoporte?: boolean,
        registroCompletoDescripcion?: boolean,

        novedadContractualDescripcion?: NovedadContractualDescripcion[],
        contrato?: Contrato,
        proyectosContrato?: any[],
        novedadContractualObservaciones?: NovedadContractualObservaciones[],
        novedadContractualAportante?: NovedadContractualAportante[],

        observacionApoyo?: NovedadContractualObservaciones,
        observacionSupervisor?: NovedadContractualObservaciones,
        observacionTramite?: NovedadContractualObservaciones,
        observacionDevolucion?: NovedadContractualObservaciones,
        observacionDevolucionTramite?: NovedadContractualObservaciones,

        urlSoporteGestionar ?: string,
        fechaTramiteGestionar ?: Date,
        observacionGestionar ?: string,
        registroCompletoGestionar ?: boolean,

        numeroOtroSi ?: string,
        fechaEnvioFirmaContratista ?: Date,
        fechaFirmaContratista ?: Date,
        fechaEnvioFirmaFiduciaria ?: Date,
        fechaFirmaFiduciaria ?: Date,
        observacionesTramite ?: string,
        urlDocumentoSuscrita ?: string,
        registroCompletoTramite ?: boolean,
        tipoModificacion ?: string,
        adicionBoolean ?: boolean,
        vaComite ?: boolean,
        novedadContractualRegistroPresupuestal?: any[],

        proyectosSeleccionado?: any[],
        datosContratoProyectoModificadosXNovedad?: any[]

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
        fechaEstimadaFinalizacion?: Date,
        plazoModificacionDias?: any,
        plazoModificacionMeses?: any,


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

export interface NovedadContractualAportante{
        novedadContractualAportanteId?:number,
        novedadContractualId?:number,
        cofinanciacionAportanteId?:number,
        valorAporte?:number,
        eliminado?:boolean,
        registroCompleto?:boolean,
        fechaCreacion?:Date,

        componenteAportanteNovedad?: ComponenteAportanteNovedad[]

        nombreAportante?: string,
        cofinanciacionAportante?: any
}

export interface ComponenteAportanteNovedad{
        componenteAportanteNovedadId?: number,
        cofinanciacionAportanteId?: number,
        tipoComponenteCodigo?: string,
        faseCodigo?: string,
        eliminado?: boolean,
        activo?: number,
        registroCompleto?: boolean,
        novedadContractualAportanteId?: number,

        componenteFuenteNovedad?: ComponenteFuenteNovedad[],

        nombreTipoComponente?: string,
        nombrefase?: string,
}

export interface ComponenteFuenteNovedad {

        componenteFuenteNovedadId?: number,
        componenteAportanteNovedadId?: number,
        fuenteRecursosCodigo?: string,
        fuenteFinanciacionId?: number,
        eliminado?: boolean,

        componenteUsoNovedad?: ComponenteUsoNovedad[],
        fuenteFinanciacion?: any,
}

export interface ComponenteUsoNovedad {
        componenteUsoNovedadId?:number
        componenteAportanteNovedadId?:number
        tipoUsoCodigo?:string
        valorUso?:number
        eliminado?:boolean
        activo?:number
        registroCompleto?:boolean

        nombreUso?: string,

}
