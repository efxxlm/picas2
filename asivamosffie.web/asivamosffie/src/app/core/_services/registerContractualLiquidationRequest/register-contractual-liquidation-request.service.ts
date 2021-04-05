import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegisterContractualLiquidationRequestService {
  
  constructor( private http: HttpClient ) { }

  contractual_liquidation = 'RegisterContractualLiquidationRequest';

  getListContractualLiquidationObra(){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridRegisterContractualLiquidationObra`);
  }

  getListContractualLiquidationInterventoria(){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridRegisterContractualLiquidationInterventoria`);
  }

  gridInformeFinal( pContratacionProyectoId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GridInformeFinal?pContratacionProyectoId=${ pContratacionProyectoId }`);
  }

  getInformeFinalByProyectoId( pProyectoId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetInformeFinalByProyectoId?pProyectoId=${ pProyectoId }`);
  }

  getInformeFinalAnexoByInformeFinalId( pInformeFinalId: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/GetInformeFinalAnexoByInformeFinalId?pInformeFinalId=${ pInformeFinalId }`);
  }

}
