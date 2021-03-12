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

  /* Banco de requisitos */
  createEditItem( pListaChequeoItem: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditItem`, pListaChequeoItem );
  }

  getListItem() {
    return this.http.get<any[]>( `${ this.urlApi }/GetListItem` );
  }

  getListaChequeoItemByListaChequeoItemId( listaChequeoItemId: number ) {
    return this.http.get<any>( `${ this.urlApi }/GetListaChequeoItemByListaChequeoItemId?ListaChequeoItemId=${ listaChequeoItemId }` );
  }

  activateDeactivateListaChequeoItem( pListaChequeoItem: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/ActivateDeactivateListaChequeoItem`, pListaChequeoItem );
  }

}
