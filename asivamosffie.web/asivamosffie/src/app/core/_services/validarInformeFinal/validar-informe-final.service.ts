import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidarInformeFinalService {

    private urlApi = `${ environment.apiUrl }`;

    constructor( private http: HttpClient ) { }

    final_report = 'ValidateFinalReport';
  
    getListInformeFinal(){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetListInformeFinal`);
    }

    getInformeFinalByProyecto( pProyectoId: string ){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetInformeFinalByProyectoId?pProyectoId=${ pProyectoId }`);
    }

    getInformeFinalListaChequeoByInformeFinalId( pInformeFinalId: number){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetInformeFinalListaChequeoByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
    }
    
    sendFinalReportToInterventor(pProyectoId: number){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/SendFinalReportToInterventor?pProyectoId=${ pProyectoId }`,null);
    }

    sendFinalReportToFinalVerification(pProyectoId: number){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/SendFinalReportToFinalVerification?pProyectoId=${ pProyectoId }`,null);
    }


    createEditInformeFinalInterventoriaObservacion( informeFinalInterventoriaObservaciones: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinalInterventoriaObservacion`, informeFinalInterventoriaObservaciones );
    }

    getInformeFinalInterventoriaObservacionByInformeFinalObservacion(pObservacionId: number){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalInterventoriaObservacionByInformeFinalObservacion?pObservacionId=${ pObservacionId }`);
    }

    getInformeFinalInterventoriaObservacionByInformeFinalInterventoria(pInformeFinalInterventoriaId: number){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalInterventoriaObservacionByInformeFinalInterventoria?pInformeFinalInterventoriaId=${ pInformeFinalInterventoriaId }`);
    }

    updateStateApproveInformeFinalInterventoriaByInformeFinal( informeFinal: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/UpdateStateApproveInformeFinalInterventoriaByInformeFinal`, informeFinal );
    }

    createEditObservacionInformeFinal( informeFinalObservacion: any , tieneObservacion: boolean){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditObservacionInformeFinal?tieneObservacion=${ tieneObservacion }`, informeFinalObservacion );
    }

    getListInformeFinalObservacionesInterventoria(informeFinalId: number){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetListInformeFinalObservacionesInterventoria?informeFinalId=${ informeFinalId }`);
    }
}
