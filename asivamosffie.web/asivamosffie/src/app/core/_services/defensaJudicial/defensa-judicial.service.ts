import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { environment } from 'src/environments/environment';
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

  CreateOrEditDefensaJudicial( defensaJudicial: DefensaJudicial) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CreateOrEditDefensaJudicial`, defensaJudicial );
  }

  CreateOrEditDemandadoConvocado( defensaJudicial: FormData) {
    return this.http.post<Respuesta>( `${ this.url }/JudicialDefense/CreateOrEditDemandadoConvocado`, defensaJudicial );
  }

  GetPlantillaDefensaJudicial(pContratoId:number)//file
  {
    return this.http.get(`${this.url}/JudicialDefense/GetPlantillaDefensaJudicial?pContratoId=${ pContratoId }`, { responseType: "blob" } );
  }    
}

export interface DefensaJudicial{
  defensaJudicialId:number,
  legitimacionCodigo:string,
  tipoProcesoCodigo:string,
  numeroProceso:string,
  cantContratos:number,
  estadoProcesoCodigo:string,
  solicitudId:number,
  esLegitimacionActiva:boolean,
  esCompleto:boolean,
  defensaJudicialContratacionProyecto:any[]
}