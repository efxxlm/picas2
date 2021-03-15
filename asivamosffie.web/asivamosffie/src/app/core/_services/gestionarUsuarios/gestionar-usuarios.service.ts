import { Respuesta } from './../autenticacion/autenticacion.service';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GestionarUsuariosService {

  private urlApi = `${ environment.apiUrl }/User`;

  constructor( private http: HttpClient ) { }

  getListPerfil() {
    // Get lista roles del campor roles CU 6.2
    return this.http.get<any[]>( `${ this.urlApi }/GetListPerfil` );
  }

  createEditUsuario( pUsuario: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditUsuario`, pUsuario );
  }

}
