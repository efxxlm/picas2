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

    getTablaProyectosByProyectoIdTipoContratacionVigencia( pProyectoId: number,pTipoIntervencion: string, pVigencia: number ){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetTablaProyectosByProyectoIdTipoContratacionVigencia?pProyectoId=${ pProyectoId }&pTipoIntervencion=${ pTipoIntervencion }&pVigencia=${ pVigencia }`);
    }

    getFlujoProyectoByProyectoId( pProyectoId: number ){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/getFlujoProyectoByProyectoId?pProyectoId=${ pProyectoId }`);
    }

    getVigencias(){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetVigencias`);
    }

    /*PREPARACIÓN*/

    getInfoPreparacionByProyectoId(pProyectoId: number){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetInfoPreparacionByProyectoId?pProyectoId=${ pProyectoId }`);
    }

    /*RESUMEN*/
    getInfoResumenByProyectoId(pProyectoId: number){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetInfoResumenByProyectoId?pProyectoId=${ pProyectoId }`);
    }

    /*CONTRATACIÓN*/
    getInfoContratoByProyectoId(pProyectoId: number){
      return this.http.get<any[]>(`${ this.urlApi }/${this.ficha_proyecto}/GetInfoContratoByProyectoId?pProyectoId=${ pProyectoId }`);
    }

}
