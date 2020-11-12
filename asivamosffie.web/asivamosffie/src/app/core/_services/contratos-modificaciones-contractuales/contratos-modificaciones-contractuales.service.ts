import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Contrato } from '../../../_interfaces/contratos-modificaciones.interface';
import { map } from 'rxjs/operators';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class ContratosModificacionesContractualesService {

  url: string = environment.apiUrl;

  constructor ( private http: HttpClient ) { };

  getGrilla () {
    return this.http.get( `${ this.url }/RegisterContractsAndContractualModifications/GetListSesionComiteSolicitud` )
      .pipe(
        map( ( resp: any ) => {

          const solicitudes = [];
          for ( let solicitud of resp ) {
            if ( solicitud.contratacion ) {
              solicitudes.push( solicitud );
            }
          }

          return solicitudes;

        } )
      )
  };

  getContratacionId ( solicitudId: number ) {
    return this.http.get( `${ this.url }/RegisterContractsAndContractualModifications/GetContratacionByContratacionId?ContratacionId=${ solicitudId }` );
  };

  postRegistroTramiteContrato ( pContrato: FormData, pEstadoCodigo: string ) {
    return this.http.post<Respuesta>( `${ this.url }/RegisterContractsAndContractualModifications/RegistrarTramiteContrato?pEstadoCodigo=${ pEstadoCodigo }`, pContrato );
  };

};
