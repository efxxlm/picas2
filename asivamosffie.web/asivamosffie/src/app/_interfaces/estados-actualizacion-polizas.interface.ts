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