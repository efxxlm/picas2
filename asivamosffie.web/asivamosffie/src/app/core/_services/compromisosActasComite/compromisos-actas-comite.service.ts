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
    comiteTecnicoId: 0,
    fecha: null,
    sesionComentarioId: 0,
    observacion: ''
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

  aprobarActa ( comiteTecnicoId: number ) {
    return this.http.post( `${ this.url }/AcceptReport?comiteTecnicoId=${ comiteTecnicoId }`, '' );
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

  postComentariosActa ( acta: any ) {

    this.devolverActa.comiteTecnicoId = acta.comiteTecnicoId;
    this.devolverActa.fecha = acta.fecha;
    this.devolverActa.observacion = acta.observaciones;
    this.devolverActa.sesionComentarioId = acta.sesionComentarioId;

    const headers = new HttpHeaders({
      'Content-Type':'application/json'
    })

    return this.http.post( `${ this.url }/CreateOrEditCommentReport`, this.devolverActa, { headers } )

  };

};