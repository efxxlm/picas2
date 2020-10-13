import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class ActBeginService {

  constructor(private http: HttpClient) { }
  GetVistaGenerarActaInicio(pContratoId: number) {
    return this.http.get<GetVistaGenerarActaInicio>(`${environment.apiUrl}/actBegin/GetVistaGenerarActaInicio?pContratoId=${pContratoId}`);
  }
  GetListGrillaActaInicio() {
    return this.http.get<GetListGrillaActaInicio>(`${environment.apiUrl}/actBegin/GetListGrillaActaInicio`);
  }
  GetPlantillaActaInicio(pContratoId: number) {
    return this.http.get(`${environment.apiUrl}/actBegin/GetPlantillaActaInicio?pContratoId=${pContratoId}`, { responseType: "blob" } );
  }
}
export interface GetVistaGenerarActaInicio {
  departamentoYMunicipioLlaveMEN: string;
  fechaActaInicio: any;
  fechaAprobacionGarantiaPoliza: any;
  fechaFirmaContrato: any;
  fechaGeneracionDRP1: any;
  fechaGeneracionDRP2: any;
  fechaPrevistaTerminacion: any;
  institucionEducativaLlaveMEN: string;
  llaveMENContrato: string;
  nombreEntidadContratistaObra: string;
  nombreEntidadContratistaSupervisorInterventoria: string;
  numeroContrato: string;
  numeroDRP1: string;
  numeroDRP2: string;
  observacionOConsideracionesEspeciales: string;
  plazoInicialContratoSupervisor: Date;
  valorActualContrato: string;
  valorFase1Preconstruccion: string;
  valorInicialContrato: string;
  valorfase2ConstruccionObra: string;
  vigenciaContrato: Date;
}
export interface GetListGrillaActaInicio {

}
export interface GetPlantillaActaInicio {

}