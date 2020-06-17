import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutenticacionService {

  constructor(private http: HttpClient) {  }

  IniciarSesion(usuario: Usuario)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/autenticacion/IniciarSesion`, usuario);
  }

}

export interface Usuario{
  UsuarioId?: number;
  Email: string;
  Contrasena: string;
  Activo?: boolean;
  Bloqueado?: boolean;
  IntentosFallidos?: number;
  Eliminado?: boolean;
}

export interface Respuesta{
  codigo: string;
  validation: true;
  validationmessage: string;
  data?: {};
}
