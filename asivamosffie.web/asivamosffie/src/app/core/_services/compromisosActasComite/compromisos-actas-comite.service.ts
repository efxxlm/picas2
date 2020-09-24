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
      sesionComiteTecnicoCompromisoId: `${ seguimiento.sesionComiteTecnicoCompromisoId }`
    };

    console.log( compromisoSeguimiento );

    return this.http.post( `${ this.url }/CreateOrEditReportProgress?estadoCompromiso=${ estadoId }`, compromisoSeguimiento )
  };

  postComentariosActa ( acta: any ) {

    this.devolverActa.comiteTecnicoId = acta.comiteTecnicoId;
    this.devolverActa.observacion = acta.observaciones;

    console.log( this.devolverActa );

    return this.http.post( `${ this.url }/CreateOrEditCommentReport`, this.devolverActa )

  };

};