import { environment } from './../../../../environments/environment';
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
    
    updateStateValidateInformeFinalInterventoria( pInformeFinalInterventoriaId: number, code:string){
      return this.http.post( `${ this.urlApi }/${this.final_report}/UpdateStateValidateInformeFinalInterventoria?pInformeFinalInterventoriaId=${ pInformeFinalInterventoriaId }&code=${ code }`, '' );
    }
    
    sendFinalReportToSupervision(pProyectoId: number){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/SendFinalReportToSupervision?pProyectoId=${ pProyectoId }`,null);
    }

    createEditInformeFinalInterventoriaObservacion( informeFinalInterventoriaObservaciones: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditInformeFinalInterventoriaObservacion`, informeFinalInterventoriaObservaciones );
    }

    getInformeFinalInterventoriaObservacionByInformeFinalObservacion(pObservacionId: number){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetInformeFinalInterventoriaObservacionByInformeFinalObservacion?pObservacionId=${ pObservacionId }`);
    }
}
