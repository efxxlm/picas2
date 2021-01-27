import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegistrarInformeFinalProyectoService {
  
  constructor( private http: HttpClient ) { }

  final_report = 'RegisterFinalReport';

  getListReportGrilla(){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/gridRegisterFinalReport`);
  }

  getInformeFinalByContratacionProyecto( pContratacionProyectoId: string ){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }`);
  }

  createInformeFinal( informeFinal ){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinal`, informeFinal );
  }

  editInformeFinal( informeFinal ){
    return this.http.put(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinal`, informeFinal );
  }

  getInformeFinalListaChequeo(){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalListaChequeoByInformeFinalId`);
  }
}
