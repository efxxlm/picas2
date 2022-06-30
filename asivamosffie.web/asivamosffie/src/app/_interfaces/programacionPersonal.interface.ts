export interface ListaProgramacionPersonal {
    contratoConstruccionId: number;
    departamento: string;
    estadoProgramacionInicial: string;
    estadoProgramacionInicialCodigo: string;
    fechaFirmaActaInicio: string;
    institucionEducativaSede: string;
    llaveMen: string;
    municipio: string;
    numeroContrato: string;
    plazoProyecto: number;
    sede: string;
    tipoIntervencion: string;
};

export interface EstadosProgramacion {
    sinProgramacionPersonal: string;
    enRegistroProgramacion: string;
    sinAprobacionProgramacionPersonal: string;
    conAprobacionProgramacionPersonal: string;
    sinAprobacionAjusteProgramacionObra: string;
};

export interface DetalleProgramacionPersonal {
    seguimientoSemanalPersonalObra: any;
    cantidadPersonal: number | string;
    contratoConstruccion: any;
    contratoConstruccionId: number;
    numeroSemana: number;
    programacionPersonalContratoConstruccionId: number;
}

export interface ContratoConstruccion {
    contratoConstruccionId: number;
    contratoId?: number;
    proyectoId?: number;
    programacionPersonalContratoConstruccion?: ProgramacionPersonalContratoConstruccion[];
    fechaInicioObra?: Date;
}

interface ProgramacionPersonalContratoConstruccion {
    cantidadPersonal: number;
    programacionPersonalContratoConstruccionId: number;
}
