import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import { Anexo } from 'src/app/_interfaces/proyecto-final-anexos.model';
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

  createInformeFinal( informeFinal: any ){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinal`, informeFinal );
  }

  editInformeFinal( informeFinal ){
    return this.http.put(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinal`, informeFinal );
  }

  getInformeFinalListaChequeo(pContratacionProyectoId: string){
    return this.http.get<Anexo[]>(`${environment.apiUrl}/${this.final_report}/GetInformeFinalListaChequeoByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }`);
  }

  createEditInformeFinalInterventoria( informeFinalInterventoria: any ){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinalInterventoria`, informeFinalInterventoria );
  }

  createEditInformeFinalAnexo ( informeFinalAnexo:any,pInformeFinalInterventoriaId:number  ){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinalAnexo/${ pInformeFinalInterventoriaId }`, informeFinalAnexo );
  }

  createEditInformeFinalInterventoriaObservacion( informeFinalInterventoriaObservaciones: any ){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinalInterventoriaObservacion`, informeFinalInterventoriaObservaciones );
  }

  getInformeFinalAnexoByInformeFinalInterventoriaId(pInformeFinalInterventoriaId: string){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalAnexoByInformeFinalInterventoriaId?pInformeFinalInterventoriaId=${ pInformeFinalInterventoriaId }`);
  }

}
