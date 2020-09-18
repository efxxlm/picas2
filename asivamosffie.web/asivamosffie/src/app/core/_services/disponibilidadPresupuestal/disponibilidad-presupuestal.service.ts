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
}
