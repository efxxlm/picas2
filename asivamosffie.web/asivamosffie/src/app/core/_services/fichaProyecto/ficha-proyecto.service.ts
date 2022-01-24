import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FichaProyectoService {

    private urlApi = `${ environment.apiUrl }`;

    constructor( private http: HttpClient ) { }

    ficha_proyecto = 'FichaProyecto';

    getProyectoIdByLlaveMen( pLlaveMen: string ){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetProyectoIdByLlaveMen?pLlaveMen=${ pLlaveMen }`);
    }

    getTablaProyectosByProyectoIdTipoContratacionVigencia( pProyectoId: number, pTipoContrato: string,pTipoIntervencion: string, pVigencia: number ){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetTablaProyectosByProyectoIdTipoContratacionVigencia?pProyectoId=${ pProyectoId }&pTipoContrato=${ pTipoContrato }&pTipoIntervencion=${ pTipoIntervencion }&pVigencia=${ pVigencia }`);
    }

    getFlujoProyectoByContratacionProyectoId( pContratacionProyectoId: string ){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetFlujoProyectoByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }`);
    }

    getVigencias(){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetVigencias`);
    }

}
