import { LocalizedString } from "@angular/compiler/src/output/output_ast";

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

export interface ListaChequeo {
  calificacionCodigo: string,
  estadoInformeFinal: string,
  informeFinalId: number,
  informeFinalInterventoriaId: number,
  informeFinalInterventoriaObservacionesId: number,
  informeFinalListaChequeoId: number,
  nombre: string,
  posicion: number,
  tieneObservacionNoCumple: boolean,
  tieneObservacionSupervisor: boolean,
  informeFinalAnexo:{
    informeFinalAnexoId?: number,
    tipoAnexo?: string,
    numRadicadoSac?: number,
    fechaRadicado?: Date,
    urlSoporte?: string
  },
  tieneAnexo: boolean,
  informeFinalInterventoriaObservaciones?: InformeFinalInterventoriaObservaciones[],
  validacionCodigo: string,
  estadoInforme: string,
  registroCompleto: boolean,
  semaforo: boolean,
  aprobacionCodigo: string,
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
  usuarioCreacion: string,
  observacionVigenteSupervisor: {
    esSupervision: boolean,
    fechaCreacion: Date,
    informeFinalInterventoriaId: number,
    informeFinalInterventoriaObservacionesId: number,
    observaciones: string,
    usuarioCreacion: string
  },
}

export interface InformeFinalAnexo{
  informeFinalAnexoId?: number,
  fechaCreacion?: Date,
  usuarioCreacion?: string,
  fechaModificacion?: Date,
  usuarioModificacion?: number,
  tipoAnexo?: string,
  numRadicadoSac?: number,
  fechaRadicado?: Date,
  urlSoporte?: string
}

export interface InformeFinalInterventoriaObservaciones{
  informeFinalInterventoriaObservacionesId?:number,
  informeFinalInterventoriaId?:number,
  observaciones?: string,
  esSupervision?: boolean,
  esCalificacion?: boolean,
  esApoyo?: boolean,
}