import { Respuesta } from './../common/common.service';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../../../environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CrearRolesService {

  private urlApi = `${ environment.apiUrl }/CreateRoles`

  constructor( private http: HttpClient ) { }

  getListPerfil() {
    return this.http.get<any[]>( `${ this.urlApi }/GetListPerfil` );
  }

  getPerfilByPerfilId( pPerfilId: number ) {
    return this.http.get<any>( `${ this.urlApi }/GetPerfilByPerfilId?pPerfilId=${ pPerfilId }` );
  }

  getMenu() {
    return this.http.get<any[]>( `${ this.urlApi }/GetMenu` );
  }

  createEditRolesPermisos( pPerfil: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditRolesPermisos`, pPerfil );
  }

  validateExistNamePerfil( pNamePerfil: string ) {
    return this.http.get( `${ this.urlApi }/ValidateExistNamePerfil?pNamePerfil=${ pNamePerfil }` );
  }

  activateDeactivatePerfil( pPerfil: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/ActivateDeactivatePerfil`, pPerfil );
  }

}
