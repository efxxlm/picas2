import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidarCumplimientoInformeFinalService {

    private urlApi = `${ environment.apiUrl }`;

    constructor( private http: HttpClient ) { }

    final_report = 'ValidateFulfilmentFinalReport';
  
    getListInformeFinal(){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetListInformeFinal`);
    }

    getInformeFinalByProyecto( pProyectoId: string ){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetInformeFinalByProyectoId?pProyectoId=${ pProyectoId }`);
    }

    getInformeFinalListaChequeoByInformeFinalId( pInformeFinalId: number){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetInformeFinalListaChequeoByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
    }

    createEditObservacionInformeFinal( informeFinalObservacion: any , tieneObservacion: boolean){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditObservacionInformeFinal?tieneObservacion=${ tieneObservacion }`, informeFinalObservacion );
    }

    createEditObservacionInformeFinalInterventoria( informeFinalObservacion: any , tieneObservacion: boolean){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditObservacionInformeFinalInterventoria?tieneObservacion=${ tieneObservacion }`, informeFinalObservacion );
    }
      
    sendFinalReportToSupervision(pProyectoId: number){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/SendFinalReportToSupervision?pProyectoId=${ pProyectoId }`,null);
    }

    approveFinalReportByFulfilment(pProyectoId: number){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/ApproveFinalReportByFulfilment?pProyectoId=${ pProyectoId }`,null);
    }
}
