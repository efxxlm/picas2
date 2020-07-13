import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';

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
    return this.http.post<Respuesta>(`${environment.apiUrl}/Cofinancing/CreateorUpdateCofinancing`,cofinanciacion);
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
  cofinanciacionDocumentoId: number,
  cofinanciacionAportanteId: number,
  vigenciaAporte: number,
  valorDocumento: string,
  tipoDocumentoId: number,
  numeroActa: string,
  fechaActa?: Date,
  numeroAcuerdo?: number,
  fechaAcuerdo: Date,
  valorTotalAportante: string 
}
