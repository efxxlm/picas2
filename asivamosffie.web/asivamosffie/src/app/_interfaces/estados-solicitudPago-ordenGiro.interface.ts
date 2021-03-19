export interface EstadoSolicitudPagoOrdenGiro {
    enProcesoRegistro: string
    solicitudRevisadaEquipoFacturacion: string
    solicitudDevueltaEquipoFacturacion: string
    enviadaAutorizacion: string
    aprobadaPorCoordinacion: string
    devueltaPorCoordinacion: string
    enviadaPorVerificadorFinancieroParaSubsanacion: string
    enviadaPorValidadorFinancieroParaSubsanacion: string
    rechazadaPorEquipoFinanciero: string
    enviadaParaValidacionPorFinanciera: string
    enviadaParaOrdenGiro: string
    solicitudDevueltaParaFacturacionPorOrdenGiro: string
    solicitudDevueltaParaVerificarFinancieramentePorOrdenGiro: string
    solicitudDevueltaParaValidarFinancieramentePorOrdenGiro: string
}

export enum EstadosSolicitudPagoOrdenGiro {
    enProcesoRegistro = '1',
    solicitudRevisadaEquipoFacturacion = '2',
    solicitudDevueltaEquipoFacturacion = '3',
    devueltaPorApoyoSupervisor = '4',
    enviadaAutorizacion = '5',
    aprobadaPorCoordinacion = '6',
    devueltaPorCoordinacion = '7',
    enviadaPorVerificadorFinancieroParaSubsanacion = '8',
    enviadaPorValidadorFinancieroParaSubsanacion = '9',
    rechazadaPorEquipoFinanciero = '10',
    enviadaParaValidacionPorFinanciera = '11',
    enviadaParaOrdenGiro = '12',
    solicitudDevueltaParaFacturacionPorOrdenGiro = '13',
    solicitudDevueltaParaVerificarFinancieramentePorOrdenGiro = '14',
    solicitudDevueltaParaValidarFinancieramentePorOrdenGiro = '15'
}

export interface TipoSolicitud {
    obra: string;
    interventoria: string;
    expensas: string;
    otrosCostos: string;
}

export enum TipoSolicitudes {
    obra = '1',
    interventoria = '2',
    expensas = '3',
    otrosCostos = '4'
}