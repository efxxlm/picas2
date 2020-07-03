import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private http: HttpClient) { }

  public loadProfiles() {
    const retorno = this.http.get<any[]>(`${environment.apiUrl}/common/perfiles`);
    return retorno;
  }

  listaTipoAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=3`);
  }

  listaNombreAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=4`);
  }

  listaDepartamentos(){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListDepartamento`);
  }

  listaMunicipiosByIdDepartamento(pIdDepartamento: string){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListMunicipiosByIdDepartamento?idDepartamento=${pIdDepartamento}`);
  }

  listaTipoDocFinanciacion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=7`);
  }

}

export interface Dominio{
  dominioId: number,
  tipoDominioId: number,
  nombre: string,
  activo: boolean,
}

export interface Localizacion{
  localizacionId: string,
  descripcion: string
}
