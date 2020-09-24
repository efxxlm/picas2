import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class PolizaGarantiaService implements OnInit {

  constructor(private http: HttpClient) { }
  ngOnInit(): void {

  }
  CreatePolizaObservacion(polizaObservacion: CreatePolizaObservacion) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreatePolizaObservacion`, polizaObservacion);
  }
  CreatePolizaGarantia(polizaGarantia: CreatePolizaGarantia) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreatePolizaGarantia`, polizaGarantia);
  }
  CreateContratoPoliza(contratoPoliza) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/guaranteePolicy/CreateContratoPoliza`, contratoPoliza);
  }
  GetListPolizaObservacionByContratoPolizaId(pContratoPolizaId:number){
    return this.http.get<CreatePolizaObservacion>(`${environment.apiUrl}/guaranteePolicy/GetListPolizaObservacionByContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetListPolizaGarantiaByContratoPolizaId(pContratoPolizaId:number){
    return this.http.get<CreatePolizaGarantia>(`${environment.apiUrl}/guaranteePolicy/GetListPolizaGarantiaByContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetContratoPolizaByIdContratoPolizaId(pContratoPolizaId:number){
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetContratoPolizaByIdContratoPolizaId?pContratoPolizaId=${pContratoPolizaId}`);
  }
  GetListVistaContratoGarantiaPoliza(){
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetListVistaContratoGarantiaPoliza`);
  }
  GetListGrillaContratoGarantiaPoliza(){
    return this.http.get<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/GetListGrillaContratoGarantiaPoliza`);
  }
}
export interface GetListVistaContratoGarantiaPoliza {

}
export interface CreatePolizaObservacion {
  PolizaObservacionId: number;
  ContratoPolizaId: number;
  Observacion: string;
  FechaRevision: Date;
  EstadoRevisionCodigo: string;
  ContratoPoliza: ContratoPoliza;
}
export interface CreatePolizaGarantia {
  PolizaGarantiaId: number;
  ContratoPolizaId: number;
  TipoGarantiaCodigo: string;
  EsIncluidaPoliza: boolean;
  ContratoPoliza: ContratoPoliza;
}
export interface ContratoPoliza {
  ContratoPolizaId: number;
  ContratoId: number;
  TipoSolicitudCodigo: string;
  TipoModificacionCodigo: string;
  DescripcionModificacion: string;
  NombreAseguradora: string;
  NumeroPoliza: string;
  NumeroCertificado: string;
  FechaExpedicion: Date;
  Vigencia: Date;
  VigenciaAmparo: string;
  ValorAmparo: number
  Observaciones: string;
  CumpleDatosAsegurado: boolean;
  CumpleDatosBeneficiario: boolean;
  CumpleDatosTomador: boolean;
  IncluyeReciboPago: boolean;
  IncluyeCondicionesGenerales: boolean;
  ObservacionesRevisionGeneral: string;
  FechaAprobacion: Date;
  ResponsableAprobacion: string;
  Estado: boolean;
  EstadoPolizaCodigo: string;
  FechaCreacion: Date;
  UsuarioCreacion: string;
  RegistroCompleo: boolean;
  FechaModificacion: Date;
  UsuarioModificacion: string;
  Eliminado: boolean;
  Contrato: Contrato;
}
export interface Contrato {
  ContratoId: number;
  ContratacionId: number;
  FechaTramite: Date;
  TipoContratoCodigo: string;
  NumeroContrato: string;
  EstadoDocumentoCodigo: string;
  Estado: boolean;
  FechaEnvioFirma: Date;
  FechaFirmaContratista: Date;
  FechaFirmaFiduciaria: Date;
  FechaFirmaContrato: Date;
  Observaciones: string;
  RutaDocumento: string;
  Objeto: string;
  Valor: number;
  Plazo: Date;
  UsuarioCreacion: string;
  FechaCreacion: Date;
  UsuarioModificacion: string;
  FechaModificacion: Date;
  Eliminado: boolean;
  CantidadPerfiles: number;
  EstadoVerificacionCodigo: string;
  EstadoFase1: string;
  EstadoActa: string;
  FechaActaInicioFase1: Date;
  FechaTerminacion: Date;
  PlazoFase1PreMeses: number;
  PlazoFase1PreDias: number;
  PlazoFase2ConstruccionMeses: number;
  PlazoFase2ConstruccionDias: number;
  ConObervacionesActa: boolean;
  FechaFirmaActaContratista: Date;
  FechaFirmaActaContratistaInterventoria: Date;
  RutaActa: string;
  RegistroCompleto: boolean;
  Contratacion: Contratacion;
}
export interface Contratacion {
  ContratacionId:number;
  TipoSolicitudCodigo:string;
  NumeroSolicitud:string;
  EstadoSolicitudCodigo:string;
  ContratistaId:number;
  EsObligacionEspecial:boolean;
  ConsideracionDescripcion:string;
  UsuarioCreacion:string;
  FechaCreacion:Date;
  UsuarioModificacion:string;
  FechaModificacion:Date;
  Eliminado:boolean;
  FechaEnvioDocumentacion:Date;
  Observaciones:string;
  RutaMinuta:string;
  FechaTramite:Date;
  Estado:boolean;
  EsMultiProyecto:boolean;
  TipoContratacionCodigo:string;
  RegistroCompleto:boolean;
  Contratista: Contratista;
}
export interface Contratista {
  ContratistaId:number;
  TipoIdentificacionCodigo:string;
  NumeroIdentificacion:string;
  Nombre:string;
  RepresentanteLegal:string;
  NumeroInvitacion:string;
  Activo:boolean;
  FechaCreacion:Date;
  UsuarioCreacion:string;
  FechaModificacion:Date;
  UsuarioModificacion:string;
}
