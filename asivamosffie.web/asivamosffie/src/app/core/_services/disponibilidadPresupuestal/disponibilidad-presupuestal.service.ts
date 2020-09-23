import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DisponibilidadPresupuestalService {

  constructor( private http: HttpClient ) {}

 
  GetListGenerarDisponibilidadPresupuestal()
  {
    return this.http.get<any[]>(`${environment.apiUrl}/BudgetAvailability/GetListGenerarDisponibilidadPresupuestal`);
  }

  
  GetDetailAvailabilityBudgetProyect(id)
  {
    return this.http.get<any[]>(`${environment.apiUrl}/AvailabilityBudgetProyect/GetDetailAvailabilityBudgetProyect?disponibilidadPresupuestalId=${id}`);
  }

  StartDownloadPDF(pdf)
  {
    console.log(pdf[0]);
    var json = JSON.stringify(pdf[0]);
    return this.http.get<any>(`${environment.apiUrl}/AvailabilityBudgetProyect/StartDownloadPDF?detailValidarDisponibilidadPresupuesal=${encodeURIComponent(json)}`);    
  }

  CreateDDP(id)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/CreateDDP?id=${id}`,null);
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
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetValidarValidacionDDP?id=${id}`,null);
  }
  SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetRechazarValidacionDDP`,DisponibilidadPresupuestalObservacion);
  }
  SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/SetReturnValidacionDDP`,DisponibilidadPresupuestalObservacion);
  }

  //gestionar fuentes de financiacion
  CreateFinancialFundingGestion(DisponibilidadPresupuestalObservacion:any)
  {
    return this.http.post<any[]>(`${environment.apiUrl}/BudgetAvailability/CreateFinancialFundingGestion`,DisponibilidadPresupuestalObservacion);
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
