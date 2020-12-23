import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Contrato } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Respuesta } from '../common/common.service';


@Injectable({
  providedIn: 'root'
})
export class DefensaJudicialService {
  

  url: string = environment.apiUrl;

  constructor( private http: HttpClient ) { }

  GetListContract( ) {
    return this.http.get<Contrato[]>( `${ this.url }/JudicialDefense/GetListContract` );
  }

  GetListProyectsByContract( pContratoId: string ) {
    return this.http.get<any[]>( `${ this.url }/JudicialDefense/GetListProyectsByContract?pContratoId=${pContratoId}` );
  }
  
  GetListGrillaProcesosDefensaJudicial( ) {
    return this.http.get<Contrato[]>( `${ this.url }/JudicialDefense/GetListGrillaProcesosDefensaJudicial` );
  }

  
  EliminarDefensaJudicial( pDefensaJudicialId:number ) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/EliminarDefensaJudicial?pDefensaJudicialId=${ pDefensaJudicialId }`, null );
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

  GetPlantillaDefensaJudicial(pContratoId:number)//file
  {
    return this.http.get(`${this.url}/JudicialDefense/GetPlantillaDefensaJudicial?pContratoId=${ pContratoId }`, { responseType: "blob" } );
  }    

  GetDefensaJudicialById(controlJudicialId: any) {
    return this.http.get<DefensaJudicial>(`${this.url}/JudicialDefense/GetVistaDatosBasicosProceso?pDefensaJudicialId=${ controlJudicialId }` );    
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
  fichaEstudio?:FichaEstudio
}
export interface DefensaJudicialContratacionProyecto{
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
  defensaJudicialId?:number
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
}