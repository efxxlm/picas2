import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompromisosActasComiteService {

  private url: string = `${ environment.apiUrl }/ManagementCommitteeReport`;

  constructor ( private http: HttpClient ) {};

  getGrillaCompromisos () {
    return this.http.get( `${ this.url }/GetManagementCommitteeReport` )
  };

  getCompromiso ( compromisoId: number ) {
    return this.http.get( `${ this.url }/GetManagementCommitteeReportById?SesionComiteTecnicoCompromisoId=${ compromisoId }` )
  };

  getGrillaActas () {
    return this.http.get ( `${ this.url }/GetManagementReport` )
  };

  postCompromisos ( seguimiento: any, estadoId: string ) {
    seguimiento = {
      "DescripcionSeguimiento": "Hola c:",
      "SesionComiteTecnicoCompromisoId": 1
    }
    return this.http.post( `${ this.url }/CreateOrEditReportProgress?estadoCompromiso=${ estadoId }`, seguimiento )
  };

  postComentariosActa () {

  };

};