export interface Anexo {
  fechaTerminacionProyecto: string,
  llaveMen: string,
  tipoIntervencion: string,
  institucionEducativa: string,
  sedeEducativa: string,
  proyectoId: number,
  registroCompleto: boolean,
  estadoInforme: string
}

export interface InformeFinalInterventoria {
  calificacionCodigo: string,
  fechaCreacion: Date,
  informeFinalAnexo:{
    fechaCreacion: Date,
    informeFinalAnexoId: number,
    informeFinalInterventoria: [],
    tipoAnexo: string,
    urlSoporte: string,
    usuarioCreacion: string
  },
  informeFinalAnexoId: number,
  informeFinalId: number,
  informeFinalInterventoriaId: number,
  informeFinalInterventoriaObservaciones: [],
  informeFinalListaChequeoId: number,
  usuarioCreacion: string
}