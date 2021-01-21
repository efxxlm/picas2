export interface GrillaFaseUnoPreconstruccion {
  fechaAprobacion: string;
  numeroContrato: string;
  cantidadProyectosAsociados: number;
  cantidadProyectosRequisitosAprobados: number;
  cantidadProyectosRequisitosPendientes: number;
  estadoCodigo: string;
  estadoNombre: string;
  contratoId: number;
  verBotonAprobarInicio: boolean;
};

export interface estadosPreconstruccion {
  sinAprobacionReqTecnicos?: estadoCodigos,
  enProcesoAprobacionReqTecnicos?: estadoCodigos,
  conReqTecnicosAprobados?: estadoCodigos,
  enProcesoVerificacionReqTecnicos?: estadoCodigos,
  conReqTecnicosVerificados?: estadoCodigos,
  enviadoAlSupervisor?: estadoCodigos,
  enProcesoValidacionReqTecnicos?: estadoCodigos,
  conReqTecnicosValidados?: estadoCodigos,
  conReqTecnicosAprobadosPorSupervisor?: estadoCodigos,
  enviadoAlInterventor?: estadoCodigos,
  enviadoAlApoyo?: estadoCodigos
};

interface estadoCodigos {
  codigo: string;
  nombre: string;
};

export interface ContratoModificado {
  contratacion: Contratacion;
  fechaPoliza: string;
  numeroContrato: string;
}

export interface Contrato {
  
  contratacionId: number;
  fechaTramite: string;
  tipoContratoCodigo: string;
  numeroContrato: string;
  estado: boolean;
  fechaEnvioFirma: string;
  fechaFirmaContratista: string;
  fechaFirmaFiduciaria: string;
  fechaFirmaContrato: string;
  observaciones: string;
  rutaDocumento: string;
  valor: number;
  plazo: string;
  usuarioModificacion: string;
  fechaModificacion: string;
  eliminado: boolean;
  estadoVerificacionCodigo: string;
  estadoActa: string;
  fechaActaInicioFase1: string;
  fechaTerminacion: string;
  plazoFase1PreMeses: number;
  plazoFase1PreDias: number;
  plazoFase2ConstruccionMeses: number;
  plazoFase2ConstruccionDias: number;
  conObervacionesActa: boolean;
  contratoId: number;
  contratacion: Contratacion;
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: ContratoPerfil[];
  contratoPoliza: ContratoPoliza[];
  fechaAprobacionRequisitosConstruccionInterventor?: string;
}

interface ContratoPoliza {
  contratoId: number;
  tipoSolicitudCodigo: string;
  tipoModificacionCodigo: string;
  descripcionModificacion: string;
  nombreAseguradora: string;
  numeroPoliza: string;
  numeroCertificado: string;
  fechaExpedicion: string;
  vigencia: string;
  valorAmparo: number;
  observaciones: string;
  cumpleDatosAsegurado: boolean;
  cumpleDatosBeneficiario: boolean;
  cumpleDatosTomador: boolean;
  incluyeReciboPago: boolean;
  incluyeCondicionesGenerales: boolean;
  observacionesRevisionGeneral: string;
  fechaAprobacion: string;
  responsableAprobacion: string;
  estado: boolean;
  estadoPolizaCodigo: string;
  fechaCreacion: string;
  usuarioCreacion: string;
  registroCompleto: boolean;
  fechaModificacion: string;
  usuarioModificacion: string;
  eliminado: boolean;
  contratoPolizaId: number;
  polizaGarantia: any[];
  polizaObservacion: any[];
}

export interface ContratoPerfil {
  contratoPerfilId?: number;
  contratoId?: number;
  perfilCodigo?: string;
  cantidadHvRequeridas?: number;
  cantidadHvRecibidas?: number;
  cantidadHvAprobadas?: number;
  fechaAprobacion?: string;
  numeroRadicadoFfie?: string;
  rutaSoporte?: string;
  observacion?: string;
  fechaCreacion?: string;
  usuarioCreacion?: string;
  fechaModificacion?: string;
  eliminado?: boolean;
  contratoPerfilNumeroRadicado?: string[];
  registroCompleto?: boolean;
  proyectoId?: number;
  contratoPerfilObservacion?: any[];
  observaciones?: string;
}

interface Contratacion {
  contratacionId: number;
  tipoSolicitudCodigo: string;
  numeroSolicitud: string;
  estadoSolicitudCodigo: string;
  contratistaId: number;
  esObligacionEspecial: boolean;
  consideracionDescripcion: string;
  usuarioCreacion: string;
  fechaCreacion: string;
  eliminado: boolean;
  fechaTramite: string;
  tipoContratacionCodigo: string;
  registroCompleto: boolean;
  contratista: Contratista;
  contratacionObservacion: any[];
  contratacionProyecto: ContratacionProyecto2[];
  contrato: any[];
  disponibilidadPresupuestal: any[];
}

export interface ContratacionProyecto2 {
  
  estadoSemaforo: string;
  estadoSemaforoContratacion: string;
  contratacionProyectoId: number;
  contratacionId: number;
  proyectoId: number;
  fechaCreacion: string;
  usuarioCreacion: string;
  eliminado: boolean;
  usuarioModificacion: string;
  fechaModificacion: string;
  proyecto: Proyecto2;
  contratacionProyectoAportante: any[];
  sesionSolicitudObservacionProyecto: any[];
  fasePreConstruccionNotMapped?: any;
}

interface Proyecto2 {
  
  departamento: string;
  municipio: string;
  proyectoId: number;
  fechaSesionJunta: string;
  numeroActaJunta: number;
  tipoIntervencionCodigo: string;
  llaveMen: string;
  localizacionIdMunicipio: string;
  institucionEducativaId: number;
  sedeId: number;
  enConvocatoria: boolean;
  cantPrediosPostulados: number;
  tipoPredioCodigo: string;
  predioPrincipalId: number;
  valorObra: number;
  valorInterventoria: number;
  valorTotal: number;
  estadoProyectoCodigo: string;
  eliminado: boolean;
  fechaCreacion: string;
  usuarioCreacion: string;
  estadoJuridicoCodigo: string;
  registroCompleto: boolean;
  localizacionIdMunicipioNavigation: LocalizacionIdMunicipioNavigation;
  contratacionProyecto: any[];
  contratoConstruccion: any[];
  contratoPerfil?: ContratoPerfil[];
  disponibilidadPresupuestalProyecto: any[];
  infraestructuraIntervenirProyecto: any[];
  proyectoAportante: any[];
  proyectoPredio: any[];
  proyectoRequisitoTecnico: any[];
  semaforoGeneral?: string;//just for class colors
}

interface LocalizacionIdMunicipioNavigation {
  localizacionId: string;
  descripcion: string;
  idPadre: string;
  nivel: number;
  tipo: string;
  cofinanciacionAportanteDepartamento: any[];
  cofinanciacionAportanteMunicipio: any[];
  proyecto: Proyecto[];
}

interface Proyecto {
  departamento: string;
  municipio: string;
  proyectoId: number;
  fechaSesionJunta: string;
  numeroActaJunta: number;
  tipoIntervencionCodigo: string;
  llaveMen: string;
  localizacionIdMunicipio: string;
  institucionEducativaId: number;
  sedeId: number;
  enConvocatoria: boolean;
  cantPrediosPostulados: number;
  tipoPredioCodigo: string;
  predioPrincipalId: number;
  valorObra: number;
  valorInterventoria: number;
  valorTotal: number;
  estadoProyectoCodigo: string;
  eliminado: boolean;
  fechaCreacion: string;
  usuarioCreacion: string;
  estadoJuridicoCodigo: string;
  registroCompleto: boolean;
  contratacionProyecto: ContratacionProyecto[];
  contratoConstruccion: any[];
  contratoPerfil: any[];
  disponibilidadPresupuestalProyecto: any[];
  infraestructuraIntervenirProyecto: any[];
  proyectoAportante: any[];
  proyectoPredio: any[];
  proyectoRequisitoTecnico: any[];
}

interface ContratacionProyecto {
  contratacionProyectoId: number;
  contratacionId: number;
  proyectoId: number;
  fechaCreacion: string;
  usuarioCreacion: string;
  eliminado: boolean;
  usuarioModificacion: string;
  fechaModificacion: string;
  contratacionProyectoAportante: any[];
  sesionSolicitudObservacionProyecto: any[];
}

interface Contratista {
  contratistaId: number;
  tipoIdentificacionCodigo: string;
  numeroIdentificacion: string;
  nombre: string;
  representanteLegal: string;
  numeroInvitacion: string;
  activo: boolean;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion: string;
  usuarioModificacion: string;
  contratacion: any[];
}


interface TipoObservacionConstruccion{
  Diagnostico: string;
  PlanesProgramas: string;
  ManejoAnticipo: string;
  ProgramacionObra: string;
  FlujoInversion: string;
}

export const TiposObservacionConstruccion: TipoObservacionConstruccion = {
  Diagnostico: '1',
  PlanesProgramas: '2',
  ManejoAnticipo: '3',
  ProgramacionObra: '4',
  FlujoInversion: '5',
};
