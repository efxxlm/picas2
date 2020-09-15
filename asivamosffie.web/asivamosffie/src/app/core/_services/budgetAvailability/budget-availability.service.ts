import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaDisponibilidadPresupuestal, DisponibilidadPresupuestal, CustonReuestCommittee, ListAportantes } from 'src/app/_interfaces/budgetAvailability';
import { Respuesta } from '../common/common.service';
import { Proyecto } from '../project/project.service';

@Injectable({
  providedIn: 'root'
})
export class BudgetAvailabilityService {

  constructor(
    private http: HttpClient
  ) {

  }

  // getGridBudgetAvailability() {
  //   return this.http.get<any>(`${environment.apiUrl}/BudgetAvailability/GetGridBudgetAvailability`);
  // }

  getDisponibilidadPresupuestalById( id: number ){
    return this.http.get<DisponibilidadPresupuestal>(`${environment.apiUrl}/BudgetAvailability/${ id }`);
  }

  createEditarDP( disponibilidad: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/createEditarDP`, disponibilidad);
  }

  getReuestCommittee(){
    return this.http.get<CustonReuestCommittee[]>(`${environment.apiUrl}/RequestBudgetAvailability/GetReuestCommittee`);
  }

  createOrEditInfoAdditional( disponibilidadPresupuestal: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/createOrEditInfoAdditional`, disponibilidadPresupuestal);
  }

  sendRequest( id: number ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/sendRequest?disponibilidadPresupuestalId=${ id }`, null);
  }

  searchLlaveMEN( texto: string ){
    return this.http.get<Proyecto[]>(`${environment.apiUrl}/RequestBudgetAvailability/searchLlaveMEN?LlaveMEN=${ texto }`);    
  }

  getAportantesByProyectoId( id: number ){
    return this.http.get<ListAportantes[]>(`${environment.apiUrl}/RequestBudgetAvailability/getAportantesByProyectoId?proyectoId=${ id }`);    
  }

  createOrEditDDPRequest( disponibilidadPresupuestal: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/createOrEditDDPRequest`, disponibilidadPresupuestal);
  }

  getDDPEspecial(){
    return this.http.get<ListAportantes[]>(`${environment.apiUrl}/RequestBudgetAvailability/getDDPEspecial`);    
  }

  getDetailInfoAdditionalById( id: number ){
    return this.http.get<DisponibilidadPresupuestal>(`${environment.apiUrl}/RequestBudgetAvailability/getDetailInfoAdditionalById?disponibilidadPresupuestalId=${ id }`);    

  }

}
