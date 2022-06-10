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

    getAjusteProgramacionById( id: number ){
      return this.http.get<any>( `${ this.urlApi }/GetAjusteProgramacionById?pAjusteProgramacionId=${id}`);
    }

    aprobarAjusteProgramacion( id : number ){
      return this.http.post<Respuesta>( `${ this.urlApi }/AprobarAjusteProgramacion?pAjusteProgramacionId=${ id }`, null );
    }

    enviarAlSupervisorAjusteProgramacion( id: number ){
      return this.http.post<Respuesta>( `${ this.urlApi }/EnviarAlSupervisorAjusteProgramacion?pAjusteProgramacionId=${ id }`, null );
    }

    enviarAlInterventor( id: number ){
      return this.http.post<Respuesta>( `${ this.urlApi }/EnviarAlInterventor?pAjusteProgramacionId=${ id }`, null );
    }

    createEditObservacionAjusteProgramacion( ajusteProgramacion: any, esObra: boolean ){
      return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditObservacionAjusteProgramacion?esObra=${esObra}`, ajusteProgramacion );
    }

    createEditObservacionFile( ajusteProgramacion: any, esObra : boolean ){
      return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditObservacionFile?esObra=${esObra}`, ajusteProgramacion );
    }

    uploadFileToValidateAdjustmentProgramming ( pAjusteProgramacionId: number, pContratacionProyectId: number, pNovedadContractualId: number, pContratoId: number, pProyectoId: number, documento: File ) {
      const formData = new FormData();
      formData.append('file', documento, documento.name);
      return this.http.post( `${ this.urlApi }/uploadFileToValidateAdjustmentProgramming?pAjusteProgramacionId=${ pAjusteProgramacionId }&pContratacionProyectId=${ pContratacionProyectId }&pNovedadContractualId=${ pNovedadContractualId }&pContratoId=${pContratoId}&pProyectoId=${pProyectoId}`, formData )
    };

    validateReprogrammingFile ( pIdDocument: string, pAjusteProgramacionId: number ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/ValidateReprogrammingFile?pIdDocument=${ pIdDocument }&pAjusteProgramacionId=${pAjusteProgramacionId}`, '' )
    };

    uploadFileToValidateAdjustmentInvestmentFlow ( pAjusteProgramacionId: number, pContratacionProyectId: number, pNovedadContractualId: number, pContratoId: number, pProyectoId: number, documento: File ) {
      const formData = new FormData();
      formData.append('file', documento, documento.name);
      return this.http.post( `${ this.urlApi }/UploadFileToValidateAdjustmentInvestmentFlow?pAjusteProgramacionId=${ pAjusteProgramacionId }&pContratacionProyectId=${ pContratacionProyectId }&pNovedadContractualId=${ pNovedadContractualId }&pContratoId=${pContratoId}&pProyectoId=${pProyectoId}`, formData )
    };

    validateInvestmentFlowFile ( pIdDocument: string, pAjusteProgramacionId: number ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/ValidateInvestmentFlowFile?pIdDocument=${ pIdDocument }&pAjusteProgramacionId=${pAjusteProgramacionId}`, '' )
    };

    getLoadAdjustProgrammingGrid ( pAjusteProgramacionId: number ) {
      return this.http.get( `${ this.urlApi }/GetLoadAdjustProgrammingGrid?pAjusteProgramacionId=${ pAjusteProgramacionId }` );
    };

    getLoadAdjustInvestmentFlowGrid ( pAjusteProgramacionId: number ) {
      return this.http.get( `${ this.urlApi }/GetLoadAdjustInvestmentFlowGrid?pAjusteProgramacionId=${ pAjusteProgramacionId }` );
    };

    deleteAdjustProgrammingOrInvestmentFlow( pArchivoCargueId: number, pAjusteProgramacionId: number, esProgramacionObra: boolean ){
      return this.http.post<Respuesta>( `${ this.urlApi }/DeleteAdjustProgrammingOrInvestmentFlow?pArchivoCargueId=${pArchivoCargueId}&pAjusteProgramacionId=${pAjusteProgramacionId}&esProgramacionObra=${esProgramacionObra}`, null );
    }

    getFileReturn ( pAjusteProgramacionId: number, esProgramacion: boolean ) {
      return this.http.get<any>( `${ this.urlApi }/GetFileReturn?pAjusteProgramacionId=${pAjusteProgramacionId}&esProgramacion=${ esProgramacion }`);
    };
}
