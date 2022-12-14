export interface GrillaProcesosContractuales {
  numeroSolicitud?: string;
  fechaSolicitud?: string;
  tipoSolicitud?: string;
  estadoDelRegistro?: string;
  estadoRegistro?: boolean;
  sesionComiteSolicitudId?: number;
  tipoSolicitudCodigo?: string;
  solicitudId?: number;
  fechaCreacion?: string;
  usuarioCreacion?: string;
  fechaModificacion?: string;
  comiteTecnicoId?: number;
  estadoCodigo?: string;
  generaCompromiso?: boolean;
  cantCompromisos?: number;
  eliminado?: boolean;
  requiereVotacion?: boolean;
  comiteTecnico?: any;
  sesionSolicitudCompromiso?: any[];
  sesionSolicitudObservacionProyecto?: any[];
  sesionSolicitudVoto?: any[];
  usuarioModificacion?: string;
  fechaComiteFiduciario?: string;
  observaciones?: string;
  rutaSoporteVotacion?: string;
};

export interface DataSolicitud {
  contratacionId?: number;
  consideracionDescripcion?: string;
  tipoSolicitudCodigo?: string;
  numeroSolicitud?: string;
  estadoSolicitudCodigo?: string;
  contratistaId?: number;
  usuarioCreacion?: string;
  fechaCreacion?: string;
  eliminado?: boolean;
  fechaEnvioDocumentacion?: string;
  observaciones?: string;
  rutaMinuta?: string;
  fechaTramite?: string;
  tipoContratacionCodigo?: string;
  registroCompleto?: boolean;
  contratista?: Contratista;
  contratacionProyecto?: ContratacionProyecto2[];
  contrato?: any[];
  disponibilidadPresupuestal?: DisponibilidadPresupuestal[];
  pFile?: any;

  urlSoporteGestionar ?: string,
  fechaTramiteGestionar ?: Date,
  observacionGestionar ?: string,
  registroCompletoGestionar ?: boolean
}

interface DisponibilidadPresupuestal {
  numeroComiteFiduciario: string;
  fechaComiteFiduciario: string;
  disponibilidadPresupuestalId: number;
  fechaSolicitud: string;
  tipoSolicitudCodigo: string;
  numeroSolicitud: string;
  opcionContratarCodigo: string;
  valorSolicitud: number;
  estadoSolicitudCodigo: string;
  objeto: string;
  eliminado: boolean;
  plazoDias: number;
  plazoMeses: number;
  fechaDdp: string;
  numeroDdp: string;
  rutaDdp: string;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion: string;
  usuarioModificacion: string;
  registroCompleto: boolean;
  contratacionId: number;
  numeroDrp: string;
  disponibilidadPresupuestalObservacion: any[];
  disponibilidadPresupuestalProyecto: any[];
  //notmapped
  stringAportante?:string;
}

interface ContratacionProyecto2 {
  contratacionProyectoId: number;
  contratacionId: number;
  proyectoId: number;
  fechaCreacion: string;
  usuarioCreacion: string;
  eliminado: boolean;
  esReasignacion?: boolean;
  porcentajeAvanceObra?: number;
  requiereLicencia?: boolean;
  licenciaVigente?: boolean;
  numeroLicencia?: string;
  fechaVigencia?: string;
  usuarioModificacion?: string;
  fechaModificacion?: string;
  activo?: boolean;
  esAvanceobra?: boolean;
  proyecto: Proyecto;
  contratacionProyectoAportante: any[];
  sesionSolicitudObservacionProyecto: any[];
  ejecucionPresupuestal?: any;
  cumpleCondicionesTai?: boolean;
  balanceFinancieroId?: number;
}

interface Proyecto {
  proyectoId: number;
  fechaSesionJunta: string;
  numeroActaJunta: number;
  tipoIntervencionCodigo: string;
  llaveMen: string;
  localizacionIdMunicipio: string;
  institucionEducativaId: number;
  sedeId: number;
  enConvocatoria: boolean;
  convocatoriaId?: number;
  cantPrediosPostulados: number;
  tipoPredioCodigo: string;
  predioPrincipalId: number;
  estadoProyectoObraCodigo: string;
  estadoProyectoInterventoriaCodigo: string;
  eliminado: boolean;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion?: string;
  usuarioModificacion?: string;
  estadoJuridicoCodigo: string;
  registroCompleto: boolean;
  institucionEducativa: InstitucionEducativa;
  contratacionProyecto: any[];
  disponibilidadPresupuestalProyecto: any[];
  infraestructuraIntervenirProyecto: any[];
  proyectoAportante: ProyectoAportante[];
  proyectoPredio: any[];
  proyectoRequisitoTecnico: any[];
  valorObra?: number;
  valorInterventoria?: number;
  valorTotal?: number;
}

interface InstitucionEducativa {
  institucionEducativaSedeId: number;
  nombre: string;
  codigoDane: number;
  localizacionIdMunicipio: string;
  fechaCreacion: string;
  usuarioCreacion: string;
  activo: boolean;
  predio: any[];
  proyectoInstitucionEducativa: (ProyectoInstitucionEducativa | ProyectoInstitucionEducativa2)[];
  proyectoSede: any[];
}

interface ProyectoInstitucionEducativa2 {
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
  disponibilidadPresupuestalProyecto: any[];
  infraestructuraIntervenirProyecto: any[];
  proyectoAportante: ProyectoAportante[];
  proyectoPredio: any[];
  proyectoRequisitoTecnico: any[];
}

interface ProyectoInstitucionEducativa {
  proyectoId: number;
  fechaSesionJunta: string;
  numeroActaJunta: number;
  tipoIntervencionCodigo: string;
  llaveMen: string;
  localizacionIdMunicipio: string;
  institucionEducativaId: number;
  sedeId: number;
  enConvocatoria: boolean;
  convocatoriaId: number;
  cantPrediosPostulados: number;
  tipoPredioCodigo: string;
  predioPrincipalId: number;
  estadoProyectoCodigo: string;
  eliminado: boolean;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion: string;
  usuarioModificacion: string;
  estadoJuridicoCodigo: string;
  registroCompleto: boolean;
  contratacionProyecto: ContratacionProyecto[];
  disponibilidadPresupuestalProyecto: any[];
  infraestructuraIntervenirProyecto: any[];
  proyectoAportante: ProyectoAportante[];
  proyectoPredio: any[];
  proyectoRequisitoTecnico: any[];
  valorTotal: number;
}

interface ProyectoAportante {
  proyectoAportanteId: number;
  proyectoId: number;
  aportanteId: number;
  valorObra: number;
  valorInterventoria: number;
  valorTotalAportante: number;
  eliminado: boolean;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion?: string;
  usuarioModificacion?: string;
}

interface ContratacionProyecto {
  contratacionProyectoId: number;
  contratacionId: number;
  proyectoId: number;
  fechaCreacion: string;
  usuarioCreacion: string;
  eliminado: boolean;
  esReasignacion: boolean;
  porcentajeAvanceObra: number;
  requiereLicencia: boolean;
  licenciaVigente: boolean;
  numeroLicencia: string;
  fechaVigencia: string;
  usuarioModificacion: string;
  fechaModificacion: string;
  activo: boolean;
  esAvanceobra: boolean;
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
  contratacion: any[];

  tipoIdentificacionNotMapped?: string;
}

export interface TipoSolicitud {
  Inicio_De_Proceso_De_Seleccion              :string;
  Contratacion                                :string;
  Modificacion_Contractual                    :string;
  ControversiasContractuales                  :string;
  Defensa_judicial                            :string;
  Actualizacion_Cronograma_Proceso_Seleccion  :string;
  Evaluacion_De_Proceso                       :string;
  Actuaciones_Controversias_Contractuales     :string;
  Actuaciones_Controversias_Reclamaciones     :string;
  Actuaciones_Defensa_judicial                :string;
  Novedad_Contractual                         :string;
  Liquidacion_Contractual                     :string;
}

export enum TipoSolicitudCodigo {
  Inicio_De_Proceso_De_Seleccion              = "1" ,
  Contratacion                                = "2" ,
  Modificacion_Contractual                    = "3" ,
  ControversiasContractuales                  = "4" ,
  Defensa_judicial                            = "5" ,
  Actualizacion_Cronograma_Proceso_Seleccion  = "6" ,
  Evaluacion_De_Proceso                       = "7" ,
  Actuaciones_Controversias_Contractuales     = "8" ,
  Actuaciones_Controversias_Reclamaciones     = "9" ,
  Actuaciones_Defensa_judicial                = "10",
  Novedad_Contractual                         = "11",
  Liquidacion_Contractual                     = "12",
}
