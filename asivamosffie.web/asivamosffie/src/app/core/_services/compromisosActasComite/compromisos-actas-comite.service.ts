import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DevolverActa } from '../../../_interfaces/compromisos-actas-comite.interfaces';

@Injectable({
  providedIn: 'root'
})
export class CompromisosActasComiteService {

  private url: string = `${ environment.apiUrl }/ManagementCommitteeReport`;
  devolverActa: DevolverActa = {
    comiteTecnicoId: 14,
    fecha: new Date(),
    sesionComentarioId: 0,
    observacion: 'Probando'
  }

  constructor ( private http: HttpClient ) {};

  getGrillaCompromisos () {
    return this.http.get( `${ this.url }/GetManagementCommitteeReport` )
  };

  getCompromiso ( compromisoId: number ) {
    console.log( compromisoId );
    return this.http.get( `${ this.url }/GetManagementCommitteeReportById?sesionComiteTecnicoCompromisoId=${ compromisoId }` )
  };

  getGrillaActas () {
    return this.http.get ( `${ this.url }/GetManagementReport` )
  };

  getActa ( comiteTecnicoId: number ) {
    return this.http.get( `${ this.url }/GetManagementReportById?comiteTecnicoId=${ comiteTecnicoId }` )
  }

  postCompromisos ( seguimiento: any, estadoId: string ) {

    const descripcion = seguimiento.tarea;
    const sesionComiteTecnicoCompromisoId = 1;

    const compromisoSeguimiento = {
      descripcionSeguimiento: `${ descripcion }`,
      sesionComiteTecnicoCompromisoId: `${ sesionComiteTecnicoCompromisoId }`
    }

    const headers = new HttpHeaders({
      'Content-Type':'application/json'
    })

    return this.http.post( `${ this.url }/CreateOrEditReportProgress?estadoCompromiso=${ estadoId }`, compromisoSeguimiento, { headers } )
  };

  getActaPdf ( comiteTecnicoId: number ) {
    return this.http.get( `${ this.url }/StartDownloadPDF?comiteTecnicoId=${ comiteTecnicoId }`, { responseType: "blob" } )
  }

  postComentariosActa () {

    const headers = new HttpHeaders({
      'Content-Type':'application/json'
    })

    return this.http.post( `${ this.url }/CreateOrEditCommentReport`, this.devolverActa, { headers } )

  };

};