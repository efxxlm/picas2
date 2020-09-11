import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';
import { map } from 'rxjs/operators';
import { DataSolicitud } from '../../../_interfaces/procesosContractuales.interface';
import { Contratacion } from '../../../_interfaces/project-contracting';

@Injectable({
  providedIn: 'root'
})
export class ProcesosContractualesService {

  private url: string = `${ environment.apiUrl }/ManageContractualProcesses`;

  constructor ( private http: HttpClient ) { }

  getGrilla () {
    return this.http.get<GrillaProcesosContractuales[]>( `${ this.url }/GetListSesionComiteSolicitud` )
    .pipe(
      map( resp => {
        let procesos: GrillaProcesosContractuales[] = [];
        for ( let proceso of resp ) {
          if ( proceso.numeroSolicitud ) {
            procesos.push( proceso );
          };
        };
        return procesos;
      } )
    );
  };

  getContratacion ( contratacionId: number ) {
    return this.http.get<DataSolicitud>( `${ this.url }/GetContratacionByContratacionId/?pContratacionId=${ contratacionId }` );
  };

  getDdp ( sesionComiteSolicitudId: number ) {
    return this.http.get( `${ this.url }/GetDDPBySesionComiteSolicitudID?pSesionComiteSolicitudID=${ sesionComiteSolicitudId }` )
  };

  sendTramite ( contratacion: Contratacion, documento: File ) {

    let formData = new FormData();
    formData.append( 'file', documento, documento.name );

    const minutaDoc = formData.get( 'file' );

    console.log( contratacion, minutaDoc );

    return this.http.put( `${ this.url }/RegistrarTramiteContratacion`, { pContratacion: contratacion, pFile: minutaDoc } );
  };

  sendCambioTramite ( solicitud: any ) {
    return this.http.put( `${ this.url }/CambiarEstadoSesionComiteSolicitud?pSesionComiteSolicitud=${ solicitud.sesionComiteSolicitudId }`, { solicitud } )
  }

};