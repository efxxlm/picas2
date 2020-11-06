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

  GetDetailAvailabilityBudgetProyect(id)
  {
    return this.http.get<any[]>(`${environment.apiUrl}/AvailabilityBudgetProyect/GetDetailAvailabilityBudgetProyect?disponibilidadPresupuestalId=${id}`);
  }

  StartDownloadPDF(pdf)
  {
    console.log(pdf[0]);
    var json = JSON.stringify(pdf[0]);    
    return this.http.get(`${environment.apiUrl}/AvailabilityBudgetProyect/StartDownloadPDF?detailValidarDisponibilidadPresupuesal=${encodeURIComponent(json)}`, { responseType: "blob" } );
  }

  
  GenerateDDP(id)
  {        
    return this.http.get(`${environment.apiUrl}/BudgetAvailability/GenerateDDP?id=${id}`, { responseType: "blob" } );
  }
  CreateDDP(id)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/CreateDDP?id=${id}`,null);
  }

  CreateDRP(id: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/CreateDRP?id=${id}`,null);
  }


  SetReturnDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetReturnDDP`,DisponibilidadPresupuestalObservacion);
  }
  SetCancelDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetCancelDDP`,DisponibilidadPresupuestalObservacion);
  }
  SetValidarValidacionDDP(id)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/SetValidarValidacionDDP?id=${id}`,null);
  }
  SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetRechazarValidacionDDP`,DisponibilidadPresupuestalObservacion);
  }
  SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetReturnValidacionDDP`,DisponibilidadPresupuestalObservacion);
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
  DeleteFinancialFundingGestion(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/DeleteFinancialFundingGestion`,DisponibilidadPresupuestalObservacion);
  }
  GetFinancialFundingGestionByDDPP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.get<any[]>(`${environment.apiUrl}/BudgetAvailability/GetFinancialFundingGestionByDDPP?pIdDisponibilidadPresupuestalProyecto=${DisponibilidadPresupuestalObservacion}`);
  }

}
