import { Respuesta } from 'src/app/core/_services/common/common.service';
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

  createEditContractSettlement( pContratacion: any ) {
    return this.http.post<Respuesta>( `${ this.apiUrl }/CreateEditContractSettlement`, pContratacion )
  }

}
