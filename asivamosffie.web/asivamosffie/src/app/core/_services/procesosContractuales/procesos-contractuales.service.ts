import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';
import { map } from 'rxjs/operators';
import { DataSolicitud } from '../../../_interfaces/procesosContractuales.interface';

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
        console.log( procesos );
        return procesos;
      } )
    );
  };

  getContratacion ( contratacionId: number ) {
    return this.http.get<DataSolicitud>( `${ this.url }/GetContratacionByContratacionId/?pContratacionId=${ contratacionId }` );
  };

  getDdp ( sesionComiteSolicitudId: number ) {
    return this.http.get( `${ this.url }/GetDDPBySesionComiteSolicitudID?pSesionComiteSolicitudID=${ sesionComiteSolicitudId }`, { responseType: "blob" } )
  };

  sendTramite ( contratacion: DataSolicitud ) {

    console.log( contratacion );

    let pContratacion = contratacion;

    return this.http.post( `${ this.url }/RegistrarTramiteContratacion`, pContratacion );
  };

  sendCambioTramite ( solicitud: GrillaProcesosContractuales ) {
    return this.http.post( `${ this.url }/CambiarEstadoSesionComiteSolicitud`, solicitud );
  }

};