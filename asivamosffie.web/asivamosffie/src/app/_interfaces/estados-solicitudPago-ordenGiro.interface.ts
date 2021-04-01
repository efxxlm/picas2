export interface EstadoSolicitudPagoOrdenGiro {
    enProcesoRegistro: string;
    solicitudRevisadaEquipoFacturacion: string;
    enviadaParaVerificacion: string;
    enProcesoVerificacion: string;
    enviadaAutorizacion: string;
    enProcesoAutorizacion: string;
    enviadaVerificacionFinanciera: string; // Sin verificacion financiera en CU 4.3.1
    enProcesoVerificacionFinanciera: string;
    enviarParaValidacionFinanciera: string; // Sin validacion financiera en CU 4.3.2
    enProcesoValidacionFinanciera: string;
    enviadaParaOrdenGiro: string; // Sin generacion en CU 4.3.3
    enProcesoGeneracion: string;
    enviadaVerificacionOrdenGiro: string; // Sin verificacion Orden giro en CU 4.3.4
    enProcesoVerificacionOrdenGiro: string;
    enviadoAprobacionOrdenGiro: string;
    enProcesoAprobacionOrdenGiro: string;
    enviadaParaTramiteFiduciaria: string;
    enProcesoTramiteFiduciaria: string;
    conOrdenGiroTramitada: string;
    solicitudDevueltaEquipoFacturacion: string;
    solicitudDevueltaApoyoSupervision: string;
    solicitudDevueltaPorCoordinador: string;
    enviadaSubsanacionVerificacionFinanciera: string;
    enviadaSubsanacionValidacionFinanciera: string;
    solicitudDevueltaPorGenerarOrdenGiroParaEquipoFacturacion: string;
    solicitudRechazadaPorVerificacionFinanciera: string;
    solicitudRechazadaPorValidacionFinanciera: string;
    ordenGiroAnulada: string;
    ordenGiroDevueltaPorVerificacion: string;
    ordenGiroDevueltaPorAprobacion: string;
    ordenGiroDevueltaPorTramiteFiduciario: string;
}

export enum EstadosSolicitudPagoOrdenGiro {
    enProcesoRegistro = '1',
    solicitudRevisadaEquipoFacturacion = '2',
    enviadaParaVerificacion = '3',
    enProcesoVerificacion = '4',
    enviadaAutorizacion = '5',
    enProcesoAutorizacion = '6',
    enviadaVerificacionFinanciera = '7', // Sin verificacion financiera en CU 4.3.1
    enProcesoVerificacionFinanciera = '8',
    enviarParaValidacionFinanciera = '9', // Sin validacion financiera en CU 4.3.2
    enProcesoValidacionFinanciera = '10',
    enviadaParaOrdenGiro = '11', // Sin generacion en CU 4.3.3
    enProcesoGeneracion = '12',
    enviadaVerificacionOrdenGiro = '13', // Sin verificacion Orden giro en CU 4.3.4
    enProcesoVerificacionOrdenGiro = '14',
    enviadoAprobacionOrdenGiro = '15', // Sin aprobacion Orden giro en CU 4.3.5
    enProcesoAprobacionOrdenGiro = '16',
    enviadaParaTramiteFiduciaria = '17', // Sin tramitar ante fiduciaria en CU 4.4.2
    enProcesoTramiteFiduciaria = '18',
    conOrdenGiroTramitada = '19',
    solicitudDevueltaEquipoFacturacion = '20', // Estados de devolucion y rechazos al CU 4.1.7
    solicitudDevueltaApoyoSupervision = '21',
    solicitudDevueltaPorCoordinador = '22',
    enviadaSubsanacionVerificacionFinanciera = '23',
    enviadaSubsanacionValidacionFinanciera = '24',
    solicitudDevueltaPorGenerarOrdenGiroParaEquipoFacturacion = '25',
    solicitudRechazadaPorVerificacionFinanciera = '26',
    solicitudRechazadaPorValidacionFinanciera = '27',
    ordenGiroAnulada = '28', /* Estados de devolucion y rechazos al CU 4.3.3, se habilita en el CU 4.3.3 el boton ver detalle / editar y el boton anular orden de giro */
    ordenGiroDevueltaPorVerificacion = '29',
    ordenGiroDevueltaPorAprobacion = '30',
    ordenGiroDevueltaPorTramiteFiduciario = '31'
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

export interface ListaMenu {
    generarOrdenGiro: number;
    verificarOrdenGiro: number;
    aprobarOrdenGiro: number;
    tramitarOrdenGiro: number;
}

export enum ListaMenuId {
    generarOrdenGiro = 68,
    verificarOrdenGiro = 76,
    aprobarOrdenGiro,
    tramitarOrdenGiro = 85
}

export interface ListaMediosPagoCodigo {
    transferenciaElectronica: string;
    chequeGerencia: string;
}

export enum MediosPagoCodigo {
    transferenciaElectronica = '1',
    chequeGerencia = '2'
}

export interface TipoAportanteDominio {
    ffie: number;
    et: number;
    tercero: number;
}

export enum TipoAportanteCodigo {
    ffie = 6,
    et = 9,
    tercero
}

export interface TipoObservaciones {
    terceroGiro: string;
    estrategiaPago: string;
    direccionTecnica: string;
    terceroCausacion: string;
    origen: string;
    observaciones: string;
    soporteOrdenGiro: string;
}

export enum TipoObservacionesCodigo {
    terceroGiro = '1',
    estrategiaPago = '2',
    direccionTecnica = '3',
    terceroCausacion = '4',
    origen = '5',
    observaciones = '6',
    soporteOrdenGiro = '7'
}