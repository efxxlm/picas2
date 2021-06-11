import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

    private urlApi = `${ environment.apiUrl }`;

    constructor( private http: HttpClient ) { }

    report = 'Report';

    //Retorna lista de la tabla indicadorReporte
    getIndicadorReporte(){
      return this.http.get<any[]>(`${environment.apiUrl}/${this.report}/GetIndicadorReporte`);
    }

    //Retorna lista de la tabla indicadorReporte , mas data del embebido
    getReportEmbedInfo(){
      return this.http.get<any[]>(`${environment.apiUrl}/${this.report}/GetReportEmbedInfo`);
    }

    //Trae info del embebido por id
    getReportEmbedInfoByIndicadorReporteId( indicadorReporteId: number ){
      return this.http.get(`${ this.urlApi }/${this.report}/GetReportEmbedInfoByIndicadorReporteId?indicadorReporteId=${ indicadorReporteId }`);
    }
}
