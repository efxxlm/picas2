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
  CreateContratoPoliza(contratoPoliza: ContratoPoliza) {
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
  AprobarContratoByIdContrato(pContratoPolizaId:number){
    return this.http.post<ContratoPoliza>(`${environment.apiUrl}/guaranteePolicy/AprobarContratoByIdContrato?pContratoPolizaId=${pContratoPolizaId}`,pContratoPolizaId);
  }
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
  contratoPolizaId: number;
  contratoId: number;
  tipoSolicitudCodigo: string;
  tipoModificacionCodigo: string;
  descripcionModificacion: string;
  nombreAseguradora: string;
  numeroPoliza: string;
  numeroCertificado: string;
  fechaExpedicion: Date;
  vigencia: Date;
  vigenciaAmparo: string;
  valorAmparo: number
  observaciones: string;
  cumpleDatosAsegurado: boolean;
  cumpleDatosBeneficiario: boolean;
  cumpleDatosTomador: boolean;
  incluyeReciboPago: boolean;
  incluyeCondicionesGenerales: boolean;
  observacionesRevisionGeneral: string;
  fechaAprobacion: Date;
  responsableAprobacion: string;
  estado: boolean;
  estadoPolizaCodigo: string;
  fechaCreacion: Date;
  usuarioCreacion: string;
  registroCompleo: boolean;
  fechaModificacion: Date;
  usuarioModificacion: string;
  eliminado: boolean;
  contrato: Contrato;
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
