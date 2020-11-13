import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaFaseUnoPreconstruccion, Contrato } from '../../../_interfaces/faseUnoPreconstruccion.interface';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class FaseUnoConstruccionService {

  private urlApi: string = `${ environment.apiUrl }/TechnicalRequirementsConstructionPhase`;

  constructor ( private http: HttpClient ) { };
  //Peticiones GET
  getContractsGrid ( ) {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.urlApi }/GetContractsGrid` );
  };

  getContractsGridApoyoInterventoria ( ) {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.urlApi }/getContractsGridApoyoInterventoria` );
  };

  GetContractsGridApoyoObra ( ) {
    return this.http.get<GrillaFaseUnoPreconstruccion[]>( `${ this.urlApi }/GetContractsGridApoyoObra` );
  };

  getContratoByContratoId ( pContratoId: number ) {
    return this.http.get<Contrato>( `${ this.urlApi }/GetContratoByContratoId?pContratoId=${ pContratoId }` );
  };
  //Peticiones GET Tabla Carga Masiva "Programación de obra"
  getLoadProgrammingGrid ( pContratoConstruccionId: number ) {
    return this.http.get( `${ this.urlApi }/GetLoadProgrammingGrid?pContratoConstruccionId=${ pContratoConstruccionId }` );
  };
  //Peticiones GET Tabla Carga Masiva "Flujo de inversión de recursos"
  getLoadInvestmentFlowGrid ( pContratoConstruccionId: number ) {
    return this.http.get( `${ this.urlApi }/GetLoadInvestmentFlowGrid?pContratoConstruccionId=${ pContratoConstruccionId }` );
  };
  //Peticiones POST
  createEditDiagnostico ( ContratoConstruccion: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditDiagnostico`, ContratoConstruccion );
  };

  createEditPlanesProgramas ( ContratoConstruccion: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditPlanesProgramas`, ContratoConstruccion );
  };

  createEditManejoAnticipo ( ContratoConstruccion: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditManejoAnticipo`, ContratoConstruccion );
  };

  createEditConstruccionPerfil ( ContratoConstruccion: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditConstruccionPerfil`, ContratoConstruccion );
  };

  createEditObservacionesCarga ( pArchivoCargueId: number, pObservacion: string ) {
    console.log( pArchivoCargueId, pObservacion );
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditObservacionesCarga?pArchivoCargueId=${ pArchivoCargueId }&pObservacion=${ pObservacion }`, '' );
  }

  createEditObservacionDiagnostico( contratoConstruccion ){ 
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacionDiagnostico?esSupervisor=false`, contratoConstruccion );
  }

  createEditObservacionPlanesProgramas( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacionPlanesProgramas?esSupervisor=false`, contratoConstruccion );
  }

  createEditObservacionManejoAnticipo( contratoConstruccion ){
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacionManejoAnticipo?esSupervisor=false`, contratoConstruccion );
  }

  createEditObservacionProgramacionObra( contratoConstruccion ){  
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacionProgramacionObra?esSupervisor=false`, contratoConstruccion );
  }

  createEditObservacionFlujoInversion( contratoConstruccion ){  
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacionFlujoInversion?esSupervisor=false`, contratoConstruccion );
  }

  createEditObservacionPerfil( contratacionPerfil ){  
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacionPerfil?esSupervisor=false`, contratacionPerfil );
  }

  aprobarInicio( id ){  
    return this.http.post<Respuesta>( `${ this.urlApi }/aprobarInicio?pContratoId=${ id }`, null );
  }
  

  //Peticiones POST Carga Masiva "Programación de obra"
  uploadFileToValidateProgramming ( pContratoConstruccinId: number, documento: File ) {
    const formData = new FormData(); 
    formData.append('file', documento, documento.name);
    return this.http.post( `${ this.urlApi }/UploadFileToValidateProgramming?pContratoConstruccinId=${ pContratoConstruccinId }`, formData )
  }

  transferMassiveLoadProgramming ( pIdDocument: string ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/TransferMassiveLoadProgramming?pIdDocument=${ pIdDocument }`, '' )
  };
  //Peticiones POST Carga Masiva "Flujo de inversión de recursos"
  uploadFileToValidateInvestmentFlow ( pContratoConstruccinId: number, documento: File ) {
    const formData = new FormData(); 
    formData.append('file', documento, documento.name);
    return this.http.post( `${ this.urlApi }/UploadFileToValidateInvestmentFlow?pContratoConstruccinId=${ pContratoConstruccinId }`, formData )
  };

  transferMassiveLoadInvestmentFlow ( pIdDocument: string ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/TransferMassiveLoadInvestmentFlow?pIdDocument=${ pIdDocument }`, '' )
  };

  EnviarAlSupervisor ( pIdDocument: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/EnviarAlSupervisor?pContratoId=${ pIdDocument }`, '' )
  };

  //Peticiones DELETE
  deleteConstruccionPerfil ( pConstruccioPerfilId: number ) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/DeleteConstruccionPerfil?pConstruccioPerfilId=${ pConstruccioPerfilId }` )
  };

  deleteConstruccionPerfilNumeroRadicado ( pConstruccionPerfilNumeroRadicadoId: number ) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/DeleteConstruccionPerfilNumeroRadicado?pConstruccionPerfilNumeroRadicadoId=${ pConstruccionPerfilNumeroRadicadoId }` )
  };

  deleteArchivoCargue ( pArchivoCargueId: number, pContratoConstruccionId:number, pEsFlujoInvserion: boolean ) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/DeleteArchivoCargue?pArchivoCargueId=${ pArchivoCargueId }&pContratoConstruccionId=${ pContratoConstruccionId }&pEsFlujoInvserion=${ pEsFlujoInvserion }` );
  };
  

}
