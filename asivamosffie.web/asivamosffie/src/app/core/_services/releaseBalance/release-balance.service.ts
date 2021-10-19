import { Respuesta } from './../common/common.service';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReleaseBalanceService {

    private apiUrl = `${ environment.apiUrl }/ReleaseBalance`;

    constructor( private http: HttpClient ) { }

    getDrpByProyectoId( pProyectoId: number) {
      return this.http.get<any[]>( `${ this.apiUrl }/GetDrpByProyectoId?pProyectoId=${ pProyectoId }` );
    }

    createEditHistoricalReleaseBalance( pUsosHistorico: any ){
      return this.http.post(`${ this.apiUrl }/CreateEditHistoricalReleaseBalance`, pUsosHistorico );
    }

    releaseBalance( pBalanceFinancieroId: any ){
      return this.http.post(`${this.apiUrl}/ReleaseBalance?pBalanceFinancieroId=${ pBalanceFinancieroId }`, null);
    }

    deleteReleaseBalance( pBalanceFinancieroId: any ){
      return this.http.post(`${this.apiUrl}/DeleteReleaseBalance?pBalanceFinancieroId=${ pBalanceFinancieroId }`, null);
    }

}
