import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FinancialBalanceService {

    private apiUrl = `${ environment.apiUrl }/FinancialBalance`;

    constructor( private http: HttpClient ) { }
  
    gridBalance() {
      return this.http.get<any[]>( `${ this.apiUrl }/GridBalance` );
    }

    getContratoByProyectoId( pProyectoId: number) {
      return this.http.get<any[]>( `${ this.apiUrl }/GetContratoByProyectoId?pProyectoId=${ pProyectoId }` );
    }

    createEditBalanceFinanciero( pBalanceFinanciero: any ){
      return this.http.post(`${ this.apiUrl }/CreateEditBalanceFinanciero`, pBalanceFinanciero );
    }
 
    getBalanceFinanciero( pProyectoId: number) {
      return this.http.get( `${ this.apiUrl }/GetBalanceFinanciero?pProyectoId=${ pProyectoId }` );
    }

    approveBalance(pProyectoId: number){
      return this.http.post(`${ this.apiUrl }/ApproveBalance?pProyectoId=${ pProyectoId }`,null);
    }
    
    getDataByProyectoId( pProyectoId: number) {
      return this.http.get<any[]>( `${ this.apiUrl }/GetDataByProyectoId?pProyectoId=${ pProyectoId }` );
    }
}
