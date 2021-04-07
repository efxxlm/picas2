export interface EstadosSolicitudLiquidacionContractual {
    enviadoAlSupervisor: string;
    enviadoControlSeguimiento: string;
    enviadoAliquidacion: string;
}
export enum EstadosSolicitudLiquidacionContractualCodigo {
    enviadoAlSupervisor = '3',
    enviadoControlSeguimiento = '3',
    enviadoAliquidacion = '3'
}

export interface ListaMenuSolicitudLiquidacion {
    registrarSolicitudLiquidacionContratacion: number;
    aprobarSolicitudLiquidacionContratacion: number;
    gestionarSolicitudLiquidacionContratacion: number;
}

export enum ListaMenuSolicitudLiquidacionId {
    registrarSolicitudLiquidacionContratacion = 88,
    aprobarSolicitudLiquidacionContratacion = 89,
    gestionarSolicitudLiquidacionContratacion = 90
}

export interface TipoObservacionLiquidacionContrato {
    actualizacionPoliza: string;
    balanceFinanciero: string;
    informeFinal: string;
}

export enum TipoObservacionLiquidacionContratoCodigo {
    actualizacionPoliza = '1',
    balanceFinanciero = '2',
    informeFinal = '3'
}