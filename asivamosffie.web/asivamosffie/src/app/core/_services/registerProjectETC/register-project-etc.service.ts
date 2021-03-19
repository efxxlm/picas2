import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RegisterProjectEtcService {

    private urlApi = `${ environment.apiUrl }`;

    constructor( private http: HttpClient ) { }

    final_report = 'RegisterProjectEtc';
  
    getListInformeFinal(){
      return this.http.get(`${environment.apiUrl}/${this.final_report}/GetListInformeFinal`);
    }

    getProyectoEntregaETCByInformeFinalId( pInformeFinalId: number ){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetProyectoEntregaETCByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
    }

    getInformeFinalListaChequeoByInformeFinalId( pInformeFinalId: number){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetInformeFinalListaChequeoByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
    }

    createEditRecorridoObra( pRecorrido: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditRecorridoObra`, pRecorrido );
    }

    createEditRepresentanteETC( pRepresentante: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditRepresentanteETC`, pRepresentante );
    }

    createEditRemisionDocumentosTecnicos( pDocumentos: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditRemisionDocumentosTecnicos`, pDocumentos );
    }

    createEditActaBienesServicios( pActaServicios: any ){
      return this.http.post(`${environment.apiUrl}/${this.final_report}/CreateEditActaBienesServicios`, pActaServicios );
    }

    getProyectoEntregaEtc( informeFinalId: number ){
      return this.http.get(`${ this.urlApi }/${this.final_report}/GetProyectoEntregaEtc?informeFinalId=${ informeFinalId }`);
    }

}
