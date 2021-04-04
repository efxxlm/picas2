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
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/gridRegisterContractualLiquidationObra`);
  }

  getListContractualLiquidationInterventoria(){
    return this.http.get<any[]>(`${environment.apiUrl}/${this.contractual_liquidation}/gridRegisterContractualLiquidationInterventoria`);
  }
}
