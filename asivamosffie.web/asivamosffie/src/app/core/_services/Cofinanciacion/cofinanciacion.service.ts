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
    for (let i = 2015; i < fecha.getFullYear(); i++){
      vigencias.push(i);
    }

    return vigencias;
  }

  CrearOModificarAcuerdoCofinanciacion(cofinanciacion: Cofinanciacion)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/Cofinancing/CreateorUpdateCofinancing`,cofinanciacion);
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
  eliminado?:boolean
}

export interface CofinanciacionAportante{
  cofinanciacionAportanteId: number,
  cofinanciacionId: number,
  tipoAportanteId: any,
  nombreAportanteId?: any,
  municipioId: number,
  cofinanciacionDocumento: CofinanciacionDocumento[],
  eliminado?:boolean,
  nombreAportante?: string,
  registroPresupuestal?: RegistroPresupuestal[],
  cofinanciacion?: Cofinanciacion
}

export interface CofinanciacionDocumento{
  cofinanciacionDocumentoId: number,
  cofinanciacionAportanteId: number,
  vigenciaAporte: number,
  valorDocumento: string,
  tipoDocumentoId: number,
  numeroActa: string,
  fechaActa?: Date,
  numeroAcuerdo?: number,
  fechaAcuerdo: Date,
  valorTotalAportante: string,
  eliminado?:boolean,
  tipoDocumento?: Dominio
}
