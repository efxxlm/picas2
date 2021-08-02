import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MonitoringURLService {
  loadDataItems= new Subject();
  constructor(private http: HttpClient) { }
  ngOnInit(): void {

  }
  GetListContratoProyectos(){
    return this.http.get<any[]>(`${environment.apiUrl}/MonitoringURL/GetListContratoProyectos`);
  }
  EditarURLMonitoreo(pProyecto:any){
    return this.http.post<Respuesta>(`${environment.apiUrl}/MonitoringURL/EditarURLMonitoreo`, pProyecto);
  }
  VisitaURLMonitoreo(URLMonitoreo:string){
    return this.http.post<Respuesta>(`${environment.apiUrl}/MonitoringURL/VisitaURLMonitoreo?URLMonitoreo=${URLMonitoreo}`,"");
  }
}
