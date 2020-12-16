import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';
import { map } from 'rxjs/operators';
import { DataSolicitud } from '../../../_interfaces/procesosContractuales.interface';
import { Respuesta } from '../autenticacion/autenticacion.service';

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
    return this.http.get<DataSolicitud>( `${ this.url }/GetContratacionByContratacionId?pContratacionId=${ contratacionId }` );
  };

  getDdp ( sesionComiteSolicitudId: number ) {
    return this.http.get( `${ this.url }/GetDDPBySesionComiteSolicitudID?pSesionComiteSolicitudID=${ sesionComiteSolicitudId }`, { responseType: "blob" } )
  };

  sendTramite ( contratacion: DataSolicitud, documento?: File ) {

    const formData = new FormData();
    formData.append( 'contratacionId', `${ contratacion.contratacionId }` );
    if ( contratacion.observaciones !== null ) {
      formData.append( 'observaciones', contratacion.observaciones );
    };
    if ( documento !== undefined ) {
      formData.append('pFile', documento, documento.name);
    };
    return this.http.post<Respuesta>( `${ this.url }/RegistrarTramiteContratacion?FechaEnvioDocumentacion=${ contratacion.fechaEnvioDocumentacion }`, formData );
  };

  sendCambioTramite ( solicitud: GrillaProcesosContractuales ) {
    return this.http.post<Respuesta>( `${ this.url }/CambiarEstadoSesionComiteSolicitud`, solicitud );
  }

};