import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Contrato } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Respuesta } from '../common/common.service';


@Injectable({
  providedIn: 'root'
})
export class DefensaJudicialService {
  
  tieneDemanda= new Subject();
  url: string = environment.apiUrl;

  constructor( private http: HttpClient ) { }

  GetListContract( ) {
    return this.http.get<Contrato[]>( `${ this.url }/JudicialDefense/GetListContract` );
  }
  GetListContractAutoComplete(pNumeroContrato : any) {
    return this.http.get<any[]>( `${ this.url }/JudicialDefense/GetContratoByTipoSolicitudCodigoModalidadContratoCodigoOrNumeroContrato?pNumeroContrato=${pNumeroContrato}` );
  }
  GetListProyectsByContract( pContratoId: any ) {
    return this.http.get<any[]>( `${ this.url }/JudicialDefense/GetListProyectsByContract?pContratoId=${pContratoId}` );
  }
  
  GetListGrillaProcesosDefensaJudicial( ) {
    return this.http.get<any[]>( `${ this.url }/JudicialDefense/GetListGrillaProcesosDefensaJudicial` );
  }

  
  EliminarDefensaJudicial( pDefensaJudicialId:number ) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/EliminarDefensaJudicial?pDefensaJudicialId=${ pDefensaJudicialId }`, null );
  }

  EnviarAComite( pDefensaJudicialId:number ) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/EnviarAComite?pDefensaJudicialId=${ pDefensaJudicialId }`, null );
  }

  cerrarProceso(pDefensaJudicialId: any) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CerrarProceso?pDefensaJudicialId=${ pDefensaJudicialId }`, null );
  }
  
  CambiarEstadoDefensaJudicial( pDefensaJudicialId:number,pCodigoEstado:string ) {
    return this.http.put<Respuesta>( `${ this.url }/JudicialDefense/CambiarEstadoDefensaJudicial?pDefensaJudicialId=${ pDefensaJudicialId }&pCodigoEstado=${pCodigoEstado}`,null );
  }

  CreateOrEditFichaEstudio( fichaEstudio: FormData) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CreateOrEditFichaEstudio`, fichaEstudio );
  }

  CreateOrEditDefensaJudicial( prmDefensaJudicial: DefensaJudicial) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CreateOrEditDefensaJudicial`, prmDefensaJudicial );
  }

  CreateOrEditDemandadoConvocado( defensaJudicial: FormData) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CreateOrEditDemandadoConvocado`, defensaJudicial );
  }

  GetPlantillaDefensaJudicial(pContratoId:number,tipoArchivo: number)//file
  {
    return this.http.get(`${this.url}/JudicialDefense/GetPlantillaDefensaJudicial?pContratoId=${ pContratoId }&tipoArchivo=${ tipoArchivo }`, { responseType: "blob" } );
  }    

  GetDefensaJudicialById(controlJudicialId: any) {
    return this.http.get<DefensaJudicial>(`${this.url}/JudicialDefense/GetVistaDatosBasicosProceso?pDefensaJudicialId=${ controlJudicialId }` );    
  }

  getActuaciones(controlJudicialId:number) {
    return this.http.get<any[]>(`${this.url}/JudicialDefense/GetActuacionesByDefensaJudicialID?pDefensaJudicialId=${ controlJudicialId }` );    
  }

  eliminarActuacionJudicial(id: number) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/DeleteActuation?id=${ id }`, null );
  }

  finalizarActuacion(id: number) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/FinalizeActuation?id=${ id }`, null );
  }

  createOrEditDefensaJudicialSeguimiento( defensaJudicialSeguimiento: DefensaJudicialSeguimiento) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CreateOrEditDefensaJudicialSeguimiento`, defensaJudicialSeguimiento );
  }

  getDefensaJudicialSeguimiento(defensaJudicialSeguimientoId:number) {
    return this.http.get<any[]>(`${this.url}/JudicialDefense/getDefensaJudicialSeguimiento?defensaJudicialSeguimientoId=${ defensaJudicialSeguimientoId }` );    
  }

  deleteDemandadoConvocado( demandadoConvocadoId: number , numeroDemandados: number) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/DeleteDemandadoConvocado?demandadoConvocadoId=${ demandadoConvocadoId }&numeroDemandados=${numeroDemandados}`, '' );
  }

  deleteDefensaJudicialContratacionProyecto( contratacionId: number , defensaJudicialId: number, cantContratos: number) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/DeleteDefensaJudicialContratacionProyecto?contratacionId=${ contratacionId }&defensaJudicialId=${defensaJudicialId}&cantContratos=${cantContratos}`, null );
  }

  deleteDemandanteConvocante( demandanteConvocadoId: number , numeroDemandantes:number) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/DeleteDemandanteConvocante?demandanteConvocadoId=${ demandanteConvocadoId }&numeroDemandantes=${numeroDemandantes}`, '' );
  }
}

export interface DefensaJudicial{
  
  defensaJudicialId?:number,
  legitimacionCodigo?:string,
  tipoProcesoCodigo?:string,
  numeroProceso?:string,
  cantContratos?:number,
  estadoProcesoCodigo?:string,
  solicitudId?:number,
  localizacionIdMunicipio?:string,
  tipoAccionCodigo?:string,
  jurisdiccionCodigo?:string,
  pretensiones?:string,
  cuantiaPerjuicios?:number,
  esRequiereSupervisor?:boolean,
  esLegitimacionActiva?:boolean,
  esCompleto?:boolean,
  urlSoporteProceso?:string,
  defensaJudicialContratacionProyecto?:DefensaJudicialContratacionProyecto[],
  demandadoConvocado?:DemandadoConvocado[],
  demandanteConvocante?:DemandanteConvocante[],
  fichaEstudio?:FichaEstudio[],
  defensaJudicialSeguimiento?: DefensaJudicialSeguimiento[];
  canalIngresoCodigo?: string;
  numeroRadicadoFfie?: string;
  fechaRadicadoFfie?: Date;
  numeroDemandantes?: number;
  numeroDemandados?: number;
  esDemandaFfie?: any;
  //existeConocimiento?:boolean,
  //not maped
  jurisdiccionCodigoNombre?:string,
  tipoAccionCodigoNombre?:string
  departamentoID?: any;
}

export interface DefensaJudicialSeguimiento{
  defensaJudicialSeguimientoId?:number,
  defensaJudicialId?:number,
  estadoProcesoCodigo?:string,
  fechaActuacion?:Date,
  actuacionAdelantada?:string,
  proximaActuacion?:string,
  fechaVencimiento?:Date,
  esRequiereSupervisor?:boolean,
  observaciones?:string,
  esprocesoResultadoDefinitivo?:boolean,
  rutaSoporte?:string,
  jurisdiccionCodigoNombre?: string,
  numeroProceso?: string,
  tipoAccionCodigoNombre?: string
}
export interface DefensaJudicialContratacionProyecto{
  contratacionProyecto?: any;//not mapped
  numeroContrato?:string;//nnot mapped
  defensaJudicialContratacionProyectoId?:number,
  contratacionProyectoId?:number,
  esCompleto?:boolean
}


export interface DemandadoConvocado{
  demandadoConvocadoId?:number,
  esConvocado?:boolean,
  nombre?:string,
  tipoIdentificacionCodigo?:string,
  numeroIdentificacion?:string,
  direccion?:string,
  email?:string,
  convocadoAutoridadDespacho?:string,
  localizacionIdMunicipio?:number,
  radicadoDespacho?:string,
  fechaRadicado?:Date,
  medioControlAccion?:string,
  etapaProcesoFfiecodigo?:string,
  caducidadPrescripcion?:Date,
  defensaJudicialId?:number,
  existeConocimiento?:boolean,
  registroCompleto?: boolean,
  esDemandado?: boolean,
}

export interface DemandanteConvocante{
  demandanteConvocanteId?:number,
  esConvocante?:boolean,
  nombre?:string,
  tipoIdentificacionCodigo?:string,
  numeroIdentificacion?:string,
  direccion?:string,
  email?:string,  
  defensaJudicialId?:number,
  demandanteConvocadoId?:number,
  registroCompleto?: boolean,
}

export interface FichaEstudio{  
  fichaEstudioId?:number,
  defensaJudicialId?:number,
  antecedentes?:string,
  hechosRelevantes?:string,
  jurisprudenciaDoctrina?:string,
  decisionComiteDirectrices?:string,
  analisisJuridico?:string,
  recomendaciones?:string,
  esPresentadoAnteComiteFfie?:boolean,
  fechaComiteDefensa?:Date,
  recomendacionFinalComite?:string,
  esAprobadoAperturaProceso?:boolean,
  tipoActuacionCodigo?:string,
  esActuacionTramiteComite?:boolean,
  abogado?:string,
  rutaSoporte?:string,
  esCompleto?: boolean,
}