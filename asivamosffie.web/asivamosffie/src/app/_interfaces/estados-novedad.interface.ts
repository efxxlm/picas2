export interface TipoNovedad {
    suspension: string;
    prorroga_a_las_Suspension: string;
    adicion: string;
    prorroga: string;
    modificacion_de_Condiciones_Contractuales: string;
    reinicio: string;
}

export enum TipoNovedadCodigo {
    suspension = '1',
    prorroga_a_las_Suspension = '2',
    adicion = '3',
    prorroga = '4',
    modificacion_de_Condiciones_Contractuales = '5',
    reinicio = '6',
}