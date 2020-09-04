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
}
