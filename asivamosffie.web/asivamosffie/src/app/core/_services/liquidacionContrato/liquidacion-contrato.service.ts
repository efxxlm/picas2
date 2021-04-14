import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LiquidacionContratoService {

  private apiUrl = `${ environment.apiUrl }/RegisterContractSettlement`;

  constructor( private http: HttpClient ) { }

  getListContractSettlemen( ) {
    return this.http.get<any[]>( `${ this.apiUrl }/GetListContractSettlemen` );
  }

}
