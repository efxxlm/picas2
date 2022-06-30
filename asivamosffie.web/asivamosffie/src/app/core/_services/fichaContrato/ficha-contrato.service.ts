import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FichaContratoService {
  private urlApi = `${environment.apiUrl}`;

  constructor(private http: HttpClient) {}

  ficha_contrato = 'FichaContrato';

  getContratosByNumeroContrato(pNumeroContrato: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetContratosByNumeroContrato?pNumeroContrato=${pNumeroContrato}`
    );
  }

  getFlujoContratoByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetFlujoContratoByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoResumenByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoResumenByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoProcesosSeleccionByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoProcesosSeleccionByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoContratacionByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoContratacionByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoPolizasSegurosByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoPolizasSegurosByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoNovedadesByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoNovedadesByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoControversiasByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoControversiasByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoProcesosDefensaByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoProcesosDefensaByContratoId?pContratoId=${pContratoId}`
    );
  }

  getInfoLiquidacionByContratoId(pContratoId: string) {
    return this.http.get<any[]>(
      `${this.urlApi}/${this.ficha_contrato}/GetInfoLiquidacionByContratoId?pContratoId=${pContratoId}`
    );
  }
}
