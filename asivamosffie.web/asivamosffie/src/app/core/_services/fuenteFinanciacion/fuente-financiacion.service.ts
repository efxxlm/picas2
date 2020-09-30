import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CofinanciacionAportante } from '../Cofinanciacion/cofinanciacion.service';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';
import { forkJoin, from } from 'rxjs';
import { mergeMap, tap, toArray } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FuenteFinanciacionService {

  constructor( private http:HttpClient

  ) { }

  createEditFuentesFinanciacion(fuenteFinanciacion: FuenteFinanciacion){
    return this.http.post(`${environment.apiUrl}/SourceFunding/CreateEditFuentesFinanciacion/`, fuenteFinanciacion);
  }  

  listaFuenteFinanciacion(){
    return this.http.get<FuenteFinanciacion[]>(`${environment.apiUrl}/SourceFunding/GetListFuentesFinanciacion`);

  }

  listaFuenteFinanciacionByAportante( id: number ){
    return this.http.get<FuenteFinanciacion[]>(`${environment.apiUrl}/SourceFunding/GetFuentesFinanciacionByAportanteId?AportanteId=${id}`);
  }

  
  GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid( id: number, idaportante:number ){
    return this.http.get<any[]>(`${environment.apiUrl}/SourceFunding/GetListFuentesFinanciacionByDisponibilidadPresupuestalProyectoid?disponibilidadPresupuestalProyectoid=${id}&aportanteID=${idaportante}`);
  }

  registrarRegistroPresupuestal( registroPresupuestal: RegistroPresupuestal ){
    return this.http.post(`${environment.apiUrl}/CofinancingContributor/SaveBudgetRegister/`, registroPresupuestal);
  }

  modificarRegistroPresupuestal( registroPresupuestal: RegistroPresupuestal ){
    return this.http.put(`${environment.apiUrl}/CofinancingContributor/UpdateRegisterBudget/`, registroPresupuestal);
  }

  crearModificarVigenciaAporte( vigenciaAporte: VigenciaAporte ){
    return this.http.post(`${environment.apiUrl}/SourceFunding/CreateEditarVigenciaAporte/`, vigenciaAporte);
  }

  crearModificarCuentaBancaria( cuentaBancaria: CuentaBancaria ){
    return this.http.post(`${environment.apiUrl}/BankAccount/CreateEditarCuentasBancarias/`, cuentaBancaria);
  }

  createEditBudgetRecords( registroPresupuestal: RegistroPresupuestal ){
    return this.http.post(`${environment.apiUrl}/CofinancingContributor/CreateEditBudgetRecords/`, registroPresupuestal);
  }

  eliminarFuentesFinanciacion( id: number ){
    return this.http.delete(`${environment.apiUrl}/SourceFunding/EliminarFuentesFinanciacion?id=${id}`);
  }

  getFuenteFinanciacion( id: number ){
    return this.http.get<FuenteFinanciacion>(`${environment.apiUrl}/SourceFunding/${id}`);
  }

  registrarControlRecurso( controlRecurso: ControlRecurso ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ResourceControl/CreateControlRecurso`, controlRecurso);
  }

  getSourceFundingBySourceFunding( id: number ){
    return this.http.get<ControlRecurso[]>(`${environment.apiUrl}/ResourceControl/GetResourceFundingBySourceFunding/${id}`);
  }

  getResourceControlById( id: number){
    return this.http.get<ControlRecurso>(`${environment.apiUrl}/ResourceControl/GetResourceControlById?pId=${id}`);
  }

  updateControlRecurso( controlRecurso: ControlRecurso){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ResourceControl/updateControlRecurso`, controlRecurso);
  }

  DeleteResourceFundingBySourceFunding(id:number)
  {
    return this.http.put<Respuesta>(`${environment.apiUrl}/ResourceControl/DeleteResourceFundingBySourceFunding?id=${id}`,null);
  }

}

export interface FuenteFinanciacion{
   cofinanciacionDocumento?: any;
   fuenteFinanciacionId?: number, 
   aportanteId: number,
   fuenteRecursosCodigo: string,
   valorFuente: number, 
   cantVigencias: number,
   aportante?: CofinanciacionAportante,
   cuentaBancaria?: CuentaBancaria[],
   vigenciaAporte?: VigenciaAporte[],
   controlRecurso?: ControlRecurso[],
   cofinanciacionDocumentoId?:number
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
  fechaCreacion?: Date,

}

export interface RegistroPresupuestal{
  registroPresupuestalId: number,
  aportanteId: number,
  numeroRp: string,
  fechaRp: Date,
  cofinanciacionDocumentoId:number,
  fechaCreacion?: Date,
  usuarioCreacion?: string,
}
