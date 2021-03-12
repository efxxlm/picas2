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

  /* Listas de chequeo */
  createEditCheckList( pListaChequeo: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditCheckList`, pListaChequeo );
  }

  deleteListaChequeoItem( pListaChequeoListaChequeoItemId: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/DeleteListaChequeoItem?pListaChequeoListaChequeoItemId=${ pListaChequeoListaChequeoItemId }`, '' );
  }

  getCheckList() {
    return this.http.get<any[]>( `${ this.urlApi }/GetCheckList` );
  }

  getListaChequeoItemByListaChequeoId( listaChequeoId: number ) {
    return this.http.get<any>( `${ this.urlApi }/GetListaChequeoItemByListaChequeoId?ListaChequeoId=${ listaChequeoId }` );
  }

  getValidateExistNameCheckList( pListaChequeo: any ) {
    return this.http.post( `${ this.urlApi }/GetValidateExistNameCheckList`, pListaChequeo );
  }

  activateDeactivateListaChequeo( pListaChequeo: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/ActivateDeactivateListaChequeo`, pListaChequeo );
  }

  deleteListaChequeo( pListaChequeoId: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/DeleteListaChequeo?pListaChequeoId=${ pListaChequeoId }`, '' );
  }

}
