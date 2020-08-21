export interface SolicitudContractual {
  nombreSesion: string;
  fecha: string;
  data: DataSesion[];
};

export interface DataSesion {
  fechaSolicitud: string;
  numeroSolicitud: string;
  tipoSolicitud: string;
};

export interface ComiteFiduciario {
  solicitudesSeleccionadas?: SolicitudContractual[];
  proposiciones?: string;
  estadoComite: boolean;
  numeroComite: string;
  temas?: Temas[];
  fechaComite: string; //Cambiar tipo Date de fecha cuando termine pruebas
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
};

export interface ColumnasTabla {
  titulo: string;
  name: string;
};