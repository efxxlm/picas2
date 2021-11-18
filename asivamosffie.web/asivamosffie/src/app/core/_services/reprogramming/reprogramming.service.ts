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

    GetAjusteProgramacionGrid ( ) {
      return this.http.get<any[]>( `${ this.urlApi }/GetAjusteProgramacionGrid` );
    };

    GetAjusteProgramacionById( id ){
      return this.http.get<any>( `${ this.urlApi }/GetAjusteProgramacionById?pAjusteProgramacionId=${id}`);
    }

    AprobarAjusteProgramacion( id ){
      return this.http.post<Respuesta>( `${ this.urlApi }/AprobarAjusteProgramacion?pAjusteProgramacionId=${ id }`, null );
    }

    EnviarAlSupervisorAjusteProgramacion( id ){
      return this.http.post<Respuesta>( `${ this.urlApi }/EnviarAlSupervisorAjusteProgramacion?pAjusteProgramacionId=${ id }`, null );
    }

    CreateEditObservacionAjusteProgramacion( ajusteProgramacion, esObra ){
      return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditObservacionAjusteProgramacion?esObra=${esObra}`, ajusteProgramacion );
    }

    uploadFileToValidateAdjustmentProgramming ( pAjusteProgramacionId: number, pContratacionProyectId: number, pNovedadContractualId: number, pContratoId: number, pProyectoId: number, documento: File ) {
      const formData = new FormData();
      formData.append('file', documento, documento.name);
      return this.http.post( `${ this.urlApi }/uploadFileToValidateAdjustmentProgramming?pAjusteProgramacionId=${ pAjusteProgramacionId }&pContratacionProyectId=${ pContratacionProyectId }&pNovedadContractualId=${ pNovedadContractualId }&pContratoId=${pContratoId}&pProyectoId=${pProyectoId}`, formData )
    };

    transferMassiveLoadAdjustmentProgramming ( pIdDocument: string, pProyectoId, pContratoId ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/transferMassiveLoadAdjustmentProgramming?pIdDocument=${ pIdDocument }&pProyectoId=${pProyectoId}&pContratoId=${pContratoId}`, '' )
    };

    UploadFileToValidateAdjustmentInvestmentFlow ( pAjusteProgramacionId: number, pContratacionProyectId: number, pNovedadContractualId: number, pContratoId: number, pProyectoId: number, documento: File ) {
      const formData = new FormData();
      formData.append('file', documento, documento.name);
      return this.http.post( `${ this.urlApi }/UploadFileToValidateAdjustmentInvestmentFlow?pAjusteProgramacionId=${ pAjusteProgramacionId }&pContratacionProyectId=${ pContratacionProyectId }&pNovedadContractualId=${ pNovedadContractualId }&pContratoId=${pContratoId}&pProyectoId=${pProyectoId}`, formData )
    };

    TransferMassiveLoadAdjustmentInvestmentFlow( pIdDocument: string, pProyectoId: number, pContratoId: number ) {
      return this.http.post<Respuesta>( `${ this.urlApi }/TransferMassiveLoadAdjustmentInvestmentFlow?pIdDocument=${ pIdDocument }&pProyectoId=${pProyectoId}&pContratoId=${pContratoId}`, '' )
    };
}
