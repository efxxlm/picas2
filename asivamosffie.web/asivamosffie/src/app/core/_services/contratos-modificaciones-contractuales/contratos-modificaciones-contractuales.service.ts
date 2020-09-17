import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ContratosModificacionesContractualesService {

  url: string = environment.apiUrl;

  constructor ( private http: HttpClient ) { };

  getGrilla () {
    return this.http.get( `${ this.url }/RegisterContractsAndContractualModifications/GetListSesionComiteSolicitud` );
  };

  getContratacionId ( solicitudId: number ) {
    return this.http.get( `${ this.url }/RegisterContractsAndContractualModifications/GetContratacionByContratacionId?pContratacionId=${ solicitudId }` );
  };

  postRegistroTramiteContrato ( pContrato, pEstadoCodigo ) {
    return this.http.post( `${ this.url }/RegisterContractsAndContractualModifications/RegistrarTramiteContrato?pEstadoCodigo=${ pEstadoCodigo }`, pContrato );
  };

};
