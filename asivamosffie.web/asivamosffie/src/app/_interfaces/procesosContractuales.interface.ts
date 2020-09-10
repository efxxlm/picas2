export interface GrillaProcesosContractuales {
  numeroSolicitud?: string;
  fechaSolicitud?: string;
  tipoSolicitud: string;
  estadoDelRegistro?: string;
  estadoRegistro: boolean;
  sesionComiteSolicitudId: number;
  tipoSolicitudCodigo: string;
  solicitudId: number;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion: string;
  comiteTecnicoId: number;
  estadoCodigo: string;
  generaCompromiso?: boolean;
  cantCompromisos?: number;
  eliminado: boolean;
  requiereVotacion?: boolean;
  comiteTecnico?: any;
  sesionSolicitudCompromiso: any[];
  sesionSolicitudObservacionProyecto: any[];
  sesionSolicitudVoto: any[];
  usuarioModificacion?: string;
  fechaComiteFiduciario?: string;
  observaciones?: string;
  rutaSoporteVotacion?: string;
};

export interface DataSolicitud {
  contratacionId?: number;
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
  registroCompleto?: boolean;
  contratista?: Contratista;
  contratacionProyecto?: ContratacionProyecto2[];
  contrato?: any[];
  disponibilidadPresupuestal?: any[];
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
  fechaDdp: string;
  numeroDdp: string;
  rutaDdp: string;
  fechaCreacion: string;
  usuarioCreacion: string;
  fechaModificacion: string;
  usuarioModificacion: string;
  registroCompleto: boolean;
  contratacionId: number;
  disponibilidadPresupuestalObservacion: any[];
  disponibilidadPresupuestalProyecto: any[];
}

interface ContratacionProyecto2 {
  contratacionProyectoId: number;
  contratacionId: number;
  proyectoId: number;
  fechaCreacion: string;
  usuarioCreacion: string;
  eliminado: boolean;
  proyecto: Proyecto;
  contratacionProyectoAportante: any[];
  sesionSolicitudObservacionProyecto: any[];
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
  estadoProyectoCodigo: string;
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
}