import { Respuesta } from './../common/common.service';
import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GestionarListaChequeoService {

  private urlApi = `${ environment.apiUrl }/ManageCheckList`;

  constructor( private http: HttpClient ) { }

  createEditItem( pListaChequeoItem: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditItem`, pListaChequeoItem );
  }

}
