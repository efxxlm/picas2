import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../common/common.service'
import { environment } from 'src/environments/environment';
import { pid } from 'process';
import { Contratacion, ContratacionProyecto } from 'src/app/_interfaces/project-contracting';

@Injectable({
  providedIn: 'root'
})
export class GestionarActPreConstrFUnoService {

  constructor(private http: HttpClient) { }
  
  GetListGrillaActaInicio(pPerfilId: number){
    return this.http.get<Contrato>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetListGrillaActaInicio?pPerfilId=${pPerfilId}`);
  }
  GetListContrato() {
    return this.http.get<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetListContrato`);
  }
  GetContratoByContratoId(pContratoId: number) {
    return this.http.get<Contrato>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetContratoByContratoId?pContratoId=${pContratoId}`);
  }
  EditContrato(pcontrato: EditContrato) {
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/EditContrato`, pcontrato);
  }
  /*
  LoadActa(pcontrato:ContratoParaActa, pfile: File) {
    const formData = new FormData();
    formData.append('idContrato', `${pcontrato.contratoId}`);
    formData.append('fechaFirmaActaContratista', `${pcontrato.fechaFirmaActaContratista}`);
    formData.append('fechaFirmaActaContratistaInterventoria', `${pcontrato.fechaFirmaActaContratistaInterventoria}`);
    formData.append('pfile', pfile, pfile.name);
    return this.http.post<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/LoadActa`, formData);
  }
  */
  LoadActa ( pContrato: FormData) {
    return this.http.post<Respuesta>( `${ environment.apiUrl}/ManagePreContructionActPhase1/LoadActa`, pContrato );
  };
  /*
  De respaldo
  EditCargarActaSuscritaContrato(pContratoId: number, pFechaFirmaContratista: string, pFechaFirmaActaContratistaInterventoria: string, pFile: File, pUsuarioModificacion: string) {
    const formData = new FormData();
    formData.append('pFile', pFile, pFile.name);
    return this.http.post<Respuesta>(`${environment.apiUrl}/actBegin/EditCargarActaSuscritaContrato?pContratoId=${pContratoId}&pFechaFirmaContratista=${pFechaFirmaContratista}&pFechaFirmaActaContratistaInterventoria=${pFechaFirmaActaContratistaInterventoria}&pUsuarioModificacion=${pUsuarioModificacion}`, formData);
  }
  */
  CambiarEstadoActa(pContratoId:number,pEstadoContrato:string){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/CambiarEstadoActa?pContratoId=${pContratoId}&pEstadoContrato=${pEstadoContrato}`,pContratoId);
  }
  GetActaByIdPerfil(pPerfilId:number, pContratoId:number){
    return this.http.get(`${environment.apiUrl}/ManagePreContructionActPhase1/GetActaByIdPerfil?pPerfilId=${pPerfilId}&pContratoId=${pContratoId}`, { responseType: "blob" } );
  }
  GetListContratoObservacionByContratoId(pContratoId:number){
    return this.http.get<ObservacionesContrato>(`${environment.apiUrl}/ManagePreContructionActPhase1/GetListContratoObservacionByContratoId?pContratoId=${pContratoId}`);
  }
  CreateEditObservacionesActa(pcontratoObservacion: any){
    return this.http.put<Respuesta>(`${environment.apiUrl}/ManagePreContructionActPhase1/CreateEditObservacionesActa`, pcontratoObservacion);
  }

  getFiferenciaMesesDias( pMesesContrato: number, pDiasContrato: number, pMesesFase1: number, pDiasFase1: number ) {
    return this.http.get( `${ environment.apiUrl }/Common/GetFiferenciaMesesDias?pMesesContrato=${ pMesesContrato }&pDiasContrato=${ pDiasContrato }&pMesesFase1=${ pMesesFase1 }&pDiasFase1=${ pDiasFase1 }` )
  }
}

export interface Contrato {
  contratoId: number;
  contratacionId: number;
  fechaTramite: Date;
  tipoContratoCodigo: string;
  numeroContrato: string;
  estado: boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  fechaActaInicioFase1: Date;
  fechaTerminacion: Date;
  plazoFase1PreMeses:number;
  plazoFase1PreDias:number;
  plazoFase2ConstruccionMeses:number;
  plazoFase2ConstruccionDias:number;
  observaciones: string;
  conObervacionesActa: boolean;
  rutaDocumento: any;
  usuarioCreacion: string;
  valor: any;
  plazo: Date;
  eliminado: boolean;
  estadoVerificacionCodigo: string;
  estadoDocumentoCodigo: string;
  estadoActa: string;
  contratacion: any[];
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}
export interface ObservacionesContrato{

}
export interface EditContrato {
  contratoId: number;
  contratacionId: number;
  fechaTramite: Date;
  tipoContratoCodigo: number;
  numeroContrato: string;
  estadoDocumentoCodigo: string;
  estado : boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  fechaActaInicioFase1?: string;
  fechaTerminacion?: string;
  plazoFase1PreMeses:number;
  plazoFase1PreDias:number;
  plazoFase2ConstruccionMeses:number;
  plazoFase2ConstruccionDias:number;
  observaciones: string;
  conObervacionesActa: boolean;
  registroCompleto: boolean;
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}
export interface ContratoObservacionElement{
  ContratoObservacionId: any;
  observaciones: any;
}

export interface ContratoParaActa {
  contratoId: number;
  contratacionId: number;
  fechaTramite: Date;
  tipoContratoCodigo: number;
  numeroContrato: string;
  estadoDocumentoCodigo: string;
  estado : boolean;
  fechaEnvioFirma: Date;
  fechaFirmaContratista: Date;
  fechaFirmaFiduciaria: Date;
  fechaFirmaContrato: Date;
  fechaActaInicioFase1?: string;
  fechaTerminacion?: string;
  fechaFirmaActaContratista?: Date;
  fechaFirmaActaContratistaInterventoria?: Date;
  plazoFase1PreMeses:number;
  plazoFase1PreDias:number;
  plazoFase2ConstruccionMeses:number;
  plazoFase2ConstruccionDias:number;
  observaciones: string;
  conObervacionesActa: boolean;
  registroCompleto: boolean;
  contratoConstruccion: any[];
  contratoObservacion: any[];
  contratoPerfil: any[];
  contratoPoliza: any[];
}