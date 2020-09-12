import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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
    return this.http.get( `${ this.url }/GetManagementCommitteeReportById?comiteTecnicoId=${ compromisoId }` )
  };

  getGrillaActas () {
    return this.http.get ( `${ this.url }/GetManagementReport` )
  };

  postCompromisos ( seguimiento: any, estadoId: string ) {

    const descripcion = seguimiento.tarea;
    const sesionComiteTecnicoCompromisoId = seguimiento.sesionComiteTecnicoCompromisoId

    const compromisoSeguimiento = {
      descripcionSeguimiento: `${ descripcion }`,
      sesionComiteTecnicoCompromisoId: `${ sesionComiteTecnicoCompromisoId }`
    }

    const headers = new HttpHeaders({
      'Content-Type':'application/json'
    })

    console.log( compromisoSeguimiento, estadoId );
    return this.http.post( `${ this.url }/CreateOrEditReportProgress?estadoCompromiso=${ estadoId }`, compromisoSeguimiento, { headers } )
  };

  postComentariosActa () {

  };

};