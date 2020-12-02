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
};

export interface DetalleProgramacionPersonal {
    cantidadPersonal: number | string;
    contratoConstruccion: any;
    contratoConstruccionId: number;
    numeroSemana: number;
    programacionPersonalContratoConstruccionId: number;
};

export interface pContratoConstruccion {
    contratoConstruccionId: number;
    programacionPersonalContratoConstruccion: programacionPersonalContratoConstruccion[];
};

interface programacionPersonalContratoConstruccion {
    cantidadPersonal: number;
    programacionPersonalContratoConstruccionId: number;
};