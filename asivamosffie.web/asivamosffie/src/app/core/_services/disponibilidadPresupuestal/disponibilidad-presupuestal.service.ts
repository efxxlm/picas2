import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class DisponibilidadPresupuestalService {
  
  

  constructor( private http: HttpClient ) {}

 
  GetListGenerarDisponibilidadPresupuestal()
  {
    return this.http.get<any[]>(`${environment.apiUrl}/BudgetAvailability/GetListGenerarDisponibilidadPresupuestal`);
  }

  GetListGenerarRegistroPresupuestal() {
    return this.http.get<any>(`${environment.apiUrl}/BudgetAvailability/GetListGenerarRegistroPresupuestal`);
  }

  GetDisponibilidadPresupuestalByID(id) {
    return this.http.get<any>(`${environment.apiUrl}/BudgetAvailability/GetDisponibilidadPresupuestalByID?DisponibilidadPresupuestalId=${id}`);
  }

  GetDetailAvailabilityBudgetProyect(id, esNovedad='false', RegistroNovedadId='0')
  {
    return this.http.get<any[]>(`${environment.apiUrl}/AvailabilityBudgetProyect/GetDetailAvailabilityBudgetProyect?disponibilidadPresupuestalId=${id}&esNovedad=${esNovedad}&RegistroNovedadId=${RegistroNovedadId}`);
  }
  GetDetailAvailabilityBudgetProyectNew(id, esNovedad='false', RegistroNovedadId='0')
  {
    return this.http.get<any[]>(`${environment.apiUrl}/AvailabilityBudgetProyect/GetDetailAvailabilityBudgetProyectNew?disponibilidadPresupuestalId=${id}&esNovedad=${esNovedad}&RegistroNovedadId=${RegistroNovedadId}`);
  }
  

  StartDownloadPDF(pdf)
  {
    console.log(pdf[0]);
    var json = JSON.stringify(pdf[0]);    
    return this.http.get(`${environment.apiUrl}/AvailabilityBudgetProyect/StartDownloadPDF?detailValidarDisponibilidadPresupuesal=${encodeURIComponent(json)}`, { responseType: "blob" } );
  }
  GenerateDRP(id, esNovedad, pRegistroPresupuestalId)
  {        
    return this.http.get(`${environment.apiUrl}/BudgetAvailability/GenerateDRP?id=${id}&esNovedad=${esNovedad}&pRegistroPresupuestalId=${pRegistroPresupuestalId}`, { responseType: "blob" } );
  }
  
  GenerateDDP(id, esNovedad, pRegistroPresupuestalId)
  {        
    return this.http.get(`${environment.apiUrl}/BudgetAvailability/GenerateDDP?id=${id}&esNovedad=${esNovedad}&pRegistroPresupuestalId=${pRegistroPresupuestalId}`, { responseType: "blob" } );
  }
  CreateDDP(id, esNovedad, RegistroPresupuestalId)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/CreateDDP?id=${id}&esNovedad=${esNovedad}&RegistroPresupuestalId=${RegistroPresupuestalId}`,null);
  }

  CreateDRP(id: any, esNovedad, RegistroPresupuestalId) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/CreateDRP?id=${id}&esNovedad=${esNovedad}&RegistroPresupuestalId=${RegistroPresupuestalId}`,null);
  }


  SetReturnDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetReturnDDP`,DisponibilidadPresupuestalObservacion);
  }
  SetCancelDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetCancelDDP`,DisponibilidadPresupuestalObservacion);
  }
  SetValidarValidacionDDP(id, esNovedad, RegistroPresupuestalId)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/SetValidarValidacionDDP?id=${id}&esNovedad=${esNovedad}&RegistroPresupuestalId=${RegistroPresupuestalId}`,null);
  }
  SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion:any, esNovedad: boolean)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetRechazarValidacionDDP?esNovedad=${esNovedad}`,DisponibilidadPresupuestalObservacion);
  }
  SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion:any, esNovedad, RegistroPresupuestalId)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetReturnValidacionDDP?esNovedad=${esNovedad}&RegistroPresupuestalId=${RegistroPresupuestalId}`,DisponibilidadPresupuestalObservacion);
  }
  SetCancelDDR(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetCancelDDR`,DisponibilidadPresupuestalObservacion);
  }

  //gestionar fuentes de financiacion
  CreateFinancialFundingGestion(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/CreateFinancialFundingGestion`,DisponibilidadPresupuestalObservacion);
  }
  DeleteFinancialFundingGestion(pIdDisponibilidadPresObservacion:number)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/DeleteFinancialFundingGestion?pIdDisponibilidadPresObservacion=${pIdDisponibilidadPresObservacion}`,null);
  }
  GetFinancialFundingGestionByDDPP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.get<any[]>(`${environment.apiUrl}/BudgetAvailability/GetFinancialFundingGestionByDDPP?pIdDisponibilidadPresupuestalProyecto=${DisponibilidadPresupuestalObservacion}`);
  }

}
