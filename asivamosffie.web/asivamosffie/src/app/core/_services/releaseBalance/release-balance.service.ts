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

}
