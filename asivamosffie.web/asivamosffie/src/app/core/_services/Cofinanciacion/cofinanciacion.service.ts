import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

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

    console.log(vigencias);

    return vigencias;
  }

  CrearOModificarAcuerdoCofinanciacion(cofinanciacion: Cofinanciacion)
  {
    return this.http.post(`${environment.apiUrl}/Cofinancing/CreateorUpdateCofinancing`,cofinanciacion);
  }
}

export interface Cofinanciacion{
  cofinanciacionId: number,
  vigenciaCofinanciacionId: number,
  cofinanciacionAportante: CofinanciacionAportante[]
}

export interface CofinanciacionAportante{
  cofinanciacionAportanteId: number,
  cofinanciacionId: number,
  tipoAportanteId: number,
  nombreAportanteId?: number,
  municipioId: number,
  cofinanciacionDocumento: CofinanciacionDocumento[]
}

export interface CofinanciacionDocumento{
  CofinanciacionDocumentoId: number,
  CofinanciacionAportanteId: number,
  VigenciaAporteId: number,
  ValorDocumento: string,
  TipoDocumentoId: number,
  NumeroActa: string,
  FechaActa?: Date,
  NumeroAcuerdo?: number,
  FechaAcuerdo: Date,
  ValorTotalAportante: string 
}
