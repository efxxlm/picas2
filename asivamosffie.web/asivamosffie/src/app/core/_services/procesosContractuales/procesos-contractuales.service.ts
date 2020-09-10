import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';
import { map } from 'rxjs/operators';
import { DataSolicitud } from '../../../_interfaces/procesosContractuales.interface';

@Injectable({
  providedIn: 'root'
})
export class ProcesosContractualesService {

  private url: string = environment.apiUrl;

  constructor ( private http: HttpClient ) { }

  getGrilla () {
    return this.http.get<GrillaProcesosContractuales[]>( `${ this.url }/ManageContractualProcesses/GetListSesionComiteSolicitud` )
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
    )
  };

  getContratacion ( contratacionId: number ) {
    return this.http.get<DataSolicitud>( `${ this.url }/ManageContractualProcesses/GetContratacionByContratacionId/?pContratacionId=${ contratacionId }` );
  };

  sendTramite ( formulario, documento: File ) {
    let formData = new FormData();
    formData.append( 'file', documento, documento.name );
    
    const pContratacion = formulario;
    const pFile = formData.get( 'file' );

    console.log( pContratacion, pFile );

    return this.http.put( `${ this.url }/ManageContractualProcesses/RegistrarTramiteContratacion`, { pContratacion, pFile } );
  }

};