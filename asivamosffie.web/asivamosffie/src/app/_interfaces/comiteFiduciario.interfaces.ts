export interface SolicitudContractual {
  nombreSesion: string;
  fecha: string;
  data: DataSesion[];
};

export interface DataSesion {
  fechaSolicitud: string;
  numeroSolicitud: string;
  tipoSolicitud: string;
  idSolicitud?: number;
};

export interface ComiteFiduciario {
  solicitudesSeleccionadas?: SolicitudContractual[];
  proposiciones?: string;
  temas?: Temas[];
  fechaComite?: string; //Cambiar tipo Date de fecha cuando termine pruebas
  estadoComiteCodigo?: string;
};

export interface Temas {
  numeroTema: number;
  tiempoIntervencion: number;
  responsable: string;
  urlSoporte?: string;
  temaTratar: string;
};

export interface DataTable {
  estado: boolean;
  solicitud: SolicitudContractual;
  data?: any[]
};

export interface ColumnasTabla {
  titulo: string;
  name: string;
};