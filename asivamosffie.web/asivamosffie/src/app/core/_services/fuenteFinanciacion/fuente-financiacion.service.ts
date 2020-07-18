import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CofinanciacionAportante } from '../Cofinanciacion/cofinanciacion.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FuenteFinanciacionService {

  constructor( private http:HttpClient

  ) { }

  registrarFuenteFinanciacion(fuenteFinanciacion: FuenteFinanciacion){
    return this.http.post(`${environment.apiUrl}/SourceFunding`, fuenteFinanciacion);
  }  

  listaFuenteFinanciacion(){
    return this.http.get<FuenteFinanciacion[]>(`${environment.apiUrl}/SourceFunding`);

  }

  modificarFuenteFinanciacion(fuenteFinanciacion: FuenteFinanciacion){
    return this.http.put(`${environment.apiUrl}/SourceFunding`, fuenteFinanciacion);
  }

}

export interface FuenteFinanciacion{
   fuenteFinanciacionId?: number, 
   aportanteId: number,
   fuenteRecursosCodigo: string,
   valorFuente: number, 
   cantVigencias: number,
   aportante?: CofinanciacionAportante,
   cuentaBancaria?: CuentaBancaria[],
   vigenciaAporte?: VigenciaAporte[],
   controlRecurso?: ControlRecurso[],
   //DateTime FechaCreacion: Date 
   //string UsuarioCreacion: string
}

export interface CuentaBancaria{
  cuentaBancariaId: number, 
  fuenteFinanciacionId: number,
  numeroCuentaBanco: string,
  nombreCuentaBanco: string,
  codigoSifi: string,
  tipoCuentaCodigo: string,
  bancoCodigo: string,
  exenta: boolean,
  fechaCreacion?: Date,
  usuarioCreacion?: string,
}

export interface VigenciaAporte{
  vigenciaAporteId: number,
  fuenteFinanciacionId: number,
  tipoVigenciaCodigo: string,
  valorAporte: number,
  fechaCreacion?: Date,
  usuarioCreacion?: string
}

export interface ControlRecurso{
  controlRecursoId: number,
  fuenteFinanciacionId: number,
  cuentaBancariaId: number,
  registroPresupuestalId: number,
  vigenciaAporteId: number,
  fechaConsignacion: Date,
  valorConsignacion: number,

}

export interface RegistroPresupuestal{
  registroPresupuestalId: number,
  aportanteId: number,
  numeroRp: string,
  fechaRp: Date,
  fechaCreacion?: Date,
  usuarioCreacion?: string
}
