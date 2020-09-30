import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Dominio } from '../common/common.service';
import { RegistroPresupuestal } from '../fuenteFinanciacion/fuente-financiacion.service';

@Injectable({
  providedIn: 'root'
})
export class CofinanciacionService {

  constructor( private http: HttpClient ) {}

  vigenciasAcuerdoCofinanciacion(): number[]{
    const fecha = new Date();
    let vigencias: number[]=[];
    for (let i = 2015; i <= fecha.getFullYear(); i++){
      vigencias.push(i);
    }

    return vigencias;
  }

  CrearOModificarAcuerdoCofinanciacion(cofinanciacion: Cofinanciacion)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/Cofinancing/CreateorUpdateCofinancing`,cofinanciacion);
  }

  EliminarCofinanciacionByCofinanciacionId(idcof:number)
  {    
    return this.http.post<Respuesta>(`${environment.apiUrl}/Cofinancing/EliminarCofinanciacionByCofinanciacionId?pCofinancicacionId=${idcof}`,null);
  }

  listaAcuerdosCofinanciacion(){ 
    return this.http.get<Cofinanciacion[]>(`${environment.apiUrl}/Cofinancing/GetListCofinancing`);
  }

  getAcuerdoCofinanciacionById(id: number)
  {
    return this.http.get<Cofinanciacion>(`${environment.apiUrl}/Cofinancing/GetCofinancingByIdCofinancing?IdCofinancing=${id}`);
  }

  getDocumentoApropiacionByAportante(id: number){
    return this.http.get<CofinanciacionDocumento[]>(`${environment.apiUrl}/Cofinancing/GetDocument?ContributorId=${id}`);
  }

  listaAportantesByTipoAportante(pTipoAportanteID: number){
    return this.http.get<CofinanciacionAportante[]>(`${environment.apiUrl}/Cofinancing/GetAportantesByTipoAportante?pTipoAportanteID=${pTipoAportanteID}`);
  }
}

export interface Cofinanciacion{
  cofinanciacionId: number,
  vigenciaCofinanciacionId: number,
  cofinanciacionAportante: CofinanciacionAportante[],
  fechaCreacion?: Date,
  valorTotal?: number,
  estadoRegistro?: string,
  registroCompleto?:boolean,//segun bd este es el nombre del campo
  eliminado?:boolean
}

export interface CofinanciacionAportante{
  cofinanciacionAportanteId: number,
  cofinanciacionId: number,
  tipoAportanteId: any,
  nombreAportanteId?: any,
  municipioId: number,
  departamentoId:number,
  cofinanciacionDocumento: CofinanciacionDocumento[],
  eliminado?:boolean,
  valortotal?:number//just for view form
  nombreAportante?: string,
  registroPresupuestal?: RegistroPresupuestal[],
  cofinanciacion?: Cofinanciacion,
  nombreAportanteString?:string,
  fuenteFinanciacion?:any[],
  cuentaConRp?:boolean
}

export interface CofinanciacionDocumento{
  cofinanciacionDocumentoId: number,
  cofinanciacionAportanteId: number,
  vigenciaAporte: number,
  valorDocumento: number,
  tipoDocumentoId: number,
  numeroActa: string,
  fechaActa?: Date,
  numeroAcuerdo?: number,
  fechaAcuerdo: Date,
  valorTotalAportante: string,
  eliminado?:boolean,
  tipoDocumento?: Dominio
}
