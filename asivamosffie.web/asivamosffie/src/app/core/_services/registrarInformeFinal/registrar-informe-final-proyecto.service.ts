import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import { ListaChequeo } from 'src/app/_interfaces/proyecto-final-anexos.model';
@Injectable({
  providedIn: 'root'
})
export class RegistrarInformeFinalProyectoService {
  
  constructor( private http: HttpClient ) { }

  final_report = 'RegisterFinalReport';

  getListReportGrilla(){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/gridRegisterFinalReport`);
  }

  getInformeFinalByProyecto( pProyectoId: string ){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalByProyectoId?pProyectoId=${ pProyectoId }`);
  }

  createInformeFinal( informeFinal: any ){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinal`, informeFinal );
  }

  editInformeFinal( informeFinal ){
    return this.http.put(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinal`, informeFinal );
  }

  getInformeFinalListaChequeo(pProyectoId: string){
    return this.http.get<ListaChequeo[]>(`${environment.apiUrl}/${this.final_report}/GetInformeFinalListaChequeoByProyectoId?pProyectoId=${ pProyectoId }`);
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

  getObservacionesByInformeFinalInterventoriaId(pInformeFinalInterventoriaId: string){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetObservacionesByInformeFinalInterventoriaId?pInformeFinalInterventoriaId=${ pInformeFinalInterventoriaId }`);
  }

  getInformeFinalAnexoByInformeFinalAnexoId(pInformeFinalAnexoId: string){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalAnexoByInformeFinalAnexoId?pInformeFinalAnexoId=${ pInformeFinalAnexoId }`);
  }

  getInformeFinalByInformeFinalId(pInformeFinalId: number){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
  }

  getInformeFinalInterventoriaObservacionByInformeFinalObservacion(pObservacionId: number){
    return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalInterventoriaObservacionByInformeFinalObservacion?pObservacionId=${ pObservacionId }`);
  }

  sendFinalReportToSupervision(pProyectoId: number){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/SendFinalReportToSupervision?pProyectoId=${ pProyectoId }`,null);
  }
  
  createEditInformeFinalInterventoriabyInformeFinal(pInformeFinal: any){
    return this.http.post(`${environment.apiUrl}/${this.final_report}/createEditInformeFinalInterventoriabyInformeFinal`, pInformeFinal );
  }

}
