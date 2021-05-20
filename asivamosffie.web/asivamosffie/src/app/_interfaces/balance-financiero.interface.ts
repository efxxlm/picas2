export enum TipoTrasladoCodigo {
    aportante = 1,
    direccionFinanciera,
    direccionTecnica
}

export enum EstadoTraslado {
    conRegistro = '1',
    trasladoAprobado = '2',
    notificadoFiduciaria = '3',
    anulado = '4'
}

export enum EstadoBalance {
    enProcesoValidacion = '1',
    balanceValidado = '2',
    conNecesidadTraslado = '3',
    balanceAprobado = '4'
}