import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class ReprogrammingService {

    private urlApi = `${ environment.apiUrl }/Reprogramming`;

    constructor( private http: HttpClient ) { }

    getAjusteProgramacionGrid ( ) {
      return this.http.get<any[]>( `${ this.urlApi }/GetAjusteProgramacionGrid` );
    };

    getAjusteProgramacionById( id ){
      return this.http.get<any>( `${ this.urlApi }/GetAjusteProgramacionById?pAjusteProgramacionId=${id}`);
    }

    aprobarAjusteProgramacion( id ){
      return this.http.post<Respuesta>( `${ this.urlApi }/AprobarAjusteProgramacion?pAjusteProgramacionId=${ id }`, null );
    }

    enviarAlSupervisorAjusteProgramacion( id ){
      return this.http.post<Respuesta>( `${ this.urlApi }/EnviarAlSupervisorAjusteProgramacion?pAjusteProgramacionId=${ id }`, null );
    }

    enviarAlInterventor( id ){
      return this.http.post<Respuesta>( `${ this.urlApi }/EnviarAlInterventor?pAjusteProgramacionId=${ id }`, null );
    }

    createEditObservacionAjusteProgramacion( ajusteProgramacion, esObra ){
      return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditObservacionAjusteProgramacion?esObra=${esObra}`, ajusteProgramacion );
    }

    createEditObservacionFile( ajusteProgramacion, esObra ){
      return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditObservacionFile?esObra=${esObra}`, ajusteProgramacion );
    }

    uploadFileToValidateAdjustmentProgramming ( pAjusteProgramacionId: number, pContratacionProyectId: number, pNovedadContractualId: number, pContratoId: number, pProyectoId: number, documento: File ) {
      const formData = new FormData();
      formData.append('file', documento, documento.name);
      return this.http.post( `${ this.urlApi }/uploadFileToValidateAdjustmentProgramming?pAjusteProgramacionId=${ pAjusteProgramacionId }&pContratacionProyectId=${ pContratacionProyectId }&pNovedadContractualId=${ pNovedadContractualId }&pContratoId=${pContratoId}&pProyectoId=${pProyectoId}`, formData )
    };

    transferMassiveLoadAdjustmentProgramming ( pIdDocument: string, pProyectoId, pContratoId ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/transferMassiveLoadAdjustmentProgramming?pIdDocument=${ pIdDocument }&pProyectoId=${pProyectoId}&pContratoId=${pContratoId}`, '' )
    };

    uploadFileToValidateAdjustmentInvestmentFlow ( pAjusteProgramacionId: number, pContratacionProyectId: number, pNovedadContractualId: number, pContratoId: number, pProyectoId: number, documento: File ) {
      const formData = new FormData();
      formData.append('file', documento, documento.name);
      return this.http.post( `${ this.urlApi }/UploadFileToValidateAdjustmentInvestmentFlow?pAjusteProgramacionId=${ pAjusteProgramacionId }&pContratacionProyectId=${ pContratacionProyectId }&pNovedadContractualId=${ pNovedadContractualId }&pContratoId=${pContratoId}&pProyectoId=${pProyectoId}`, formData )
    };

    transferMassiveLoadAdjustmentInvestmentFlow( pIdDocument: string, pProyectoId: number, pContratoId: number ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/TransferMassiveLoadAdjustmentInvestmentFlow?pIdDocument=${ pIdDocument }&pProyectoId=${pProyectoId}&pContratoId=${pContratoId}`, '' )
    };

    getLoadAdjustProgrammingGrid ( pAjusteProgramacionId: number ) {
      return this.http.get( `${ this.urlApi }/GetLoadAdjustProgrammingGrid?pAjusteProgramacionId=${ pAjusteProgramacionId }` );
    };

    getLoadAdjustInvestmentFlowGrid ( pAjusteProgramacionId: number ) {
      return this.http.get( `${ this.urlApi }/GetLoadAdjustInvestmentFlowGrid?pAjusteProgramacionId=${ pAjusteProgramacionId }` );
    };

    deleteAdjustProgrammingOrInvestmentFlow( pArchivoCargueId, pAjusteProgramacionId ){
      return this.http.post<Respuesta>( `${ this.urlApi }/DeleteAdjustProgrammingOrInvestmentFlow?pArchivoCargueId=${pArchivoCargueId}&pAjusteProgramacionId=${pAjusteProgramacionId}`, null );
    }
}
