import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaDisponibilidadPresupuestal, DisponibilidadPresupuestal } from 'src/app/_interfaces/budgetAvailability';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class BudgetAvailabilityService {

  constructor(
    private http: HttpClient
  ) {

  }

  getGridBudgetAvailability() {
    return this.http.get<any>(`${environment.apiUrl}/BudgetAvailability/GetGridBudgetAvailability`);
  }

  getDisponibilidadPresupuestalById( id: number ){
    return this.http.get<DisponibilidadPresupuestal>(`${environment.apiUrl}/BudgetAvailability/${ id }`);
  }

  createEditarDP( disponibilidad: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/createEditarDP`, disponibilidad);
  }

}
