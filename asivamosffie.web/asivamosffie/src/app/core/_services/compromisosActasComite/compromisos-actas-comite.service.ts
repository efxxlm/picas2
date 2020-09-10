import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompromisosActasComiteService {

  private url: string = environment.apiUrl;

  constructor ( private http: HttpClient ) {};

  getGrillaCompromisos () {
    return this.http.get( `${ this.url }/ManagementCommitteeReport/GetManagementCommitteeReport` )
  };

  getCompromiso ( compromisoId: number ) {
    return this.http.get( `${ this.url }/ManagementCommitteeReport/GetManagementCommitteeReportById?SesionComiteTecnicoCompromisoId=${ compromisoId }` )
  };

  postCompromisos ( seguimiento: any, estadoId: string ) {
    seguimiento = {
      "DescripcionSeguimiento": "Hola c:",
      "SesionComiteTecnicoCompromisoId": 1
  }
    return this.http.post( `${ this.url }/ManagementCommitteeReport/CreateOrEditReportProgress?estadoCompromiso=${ estadoId }`, seguimiento )
  };

};