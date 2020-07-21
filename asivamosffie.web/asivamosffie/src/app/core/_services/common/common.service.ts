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

  listaFuenteRecursos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=18`);
  }

  listaBancos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=22`);
  }

  vigenciasDesde2015(): number[]{
    const fecha = new Date();
    let vigencias: number[]=[];
    for (let i = 2015; i < fecha.getFullYear(); i++){
      vigencias.push(i);
    }

    return vigencias;
  }
}

export interface Dominio{
  dominioId: number,
  tipoDominioId: number,
  nombre: string,
  activo: boolean,
  codigo?: string,
}

export interface Localizacion{
  localizacionId: string,
  descripcion: string
}

export interface Respuesta{
  isSuccessful: boolean;
  isValidation: boolean;
  isException: boolean;
  code: string;
  message: string;
  data?: any;
  token?: any;
}

interface TipoAportante{
  FFIE: string[];
  ET: string[];
  Tercero: string[];
}

export const TiposAportante: TipoAportante = {
  FFIE:   ["6"],
  ET:     ["9"],
  Tercero:["10"]
}
