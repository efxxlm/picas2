export enum EstadosActualizacionPoliza {
    enRevisionActualizacionPoliza = '1',
    conPolizaObservadaDevuelta = '2',
    conAprobacionActualizacionPoliza = '3'
}

export interface TipoActualizacion {
    fecha: string;
    valor: string;
    seguros: string;
}

export enum TipoActualizacionCodigo {
    fecha = '1',
    valor = '2',
    seguros = '3',
}

export enum PerfilCodigo {
    fiduciaria = 10
}

export enum EstadosRevision {
    devuelta = '1',
    aprobacion = '2'
}