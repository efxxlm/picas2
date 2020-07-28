import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AutenticacionService {

  private actualUserSubject: BehaviorSubject<Usuario>;
  public actualUser: Usuario;
  public actualUser$: Observable<Usuario>;

  constructor(private http: HttpClient) {
    this.actualUserSubject = new BehaviorSubject<Usuario>(JSON.parse(localStorage.getItem('actualUser')));
    this.actualUser$ = this.actualUserSubject.asObservable();
  }

  IniciarSesion(usuario: Usuario)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/autenticacion/IniciarSesion`, usuario).
    pipe(
      map(user => {
      // login successful if there's a jwt token in the response                
      console.log(user.data);

      if (user && user.token) {
              // store user details and jwt token in local storage to keep user logged in between page refreshes
              user.data.datausuario.token = user.token;
              let usuariocompleto=user.data.datausuario;
              usuariocompleto.rol=user.data.dataperfiles;
              //usuariocompleto.token=user.token;
              localStorage.setItem('actualUser', JSON.stringify(usuariocompleto));
              this.actualUser = usuariocompleto;
              this.actualUserSubject.next(usuariocompleto);
      }
      //console.log(user);
      return user;
      }));
  }

  RecuperarContrasena(usuario: Usuario)
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/user/emailRecover`, usuario);
  }

  public get actualUserValue(): Usuario {
    return this.actualUserSubject.value;
  }
  public get currentUserValue(): Usuario {
      return this.actualUserSubject.getValue();
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('actualUser');
    this.actualUserSubject.next(null);
    this.actualUser = null;
  //   this.saveCloseSesionAudit().subscribe();
  }

  changePass(old:string,newpass:string) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/user/ChangePasswordUser?Oldpwd=${old}&Newpwd=${newpass}`,null);
  }

  ValdiatePass(old:string) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/user/ValidateCurrentPassword?Oldpwd=${old}`,null);
  }

  public setCurrentUserValue(updatedUser: Usuario) {
    localStorage.setItem('actualUser', JSON.stringify(updatedUser));
    this.actualUserSubject.next(updatedUser);
}
  public consumoePrueba()
  {
    const retorno = this.http.get<any[]>(`${environment.apiUrl}/common/perfiles`);
    return retorno;
  }

}

export interface Usuario{
  UsuarioId?: number;
  Email: string;
  Contrasena?: string;
  Activo?: boolean;
  Bloqueado?: boolean;
  IntentosFallidos?: number;
  Eliminado?: boolean;
  token?:any;
  fechaUltimoIngreso?:Date;
  cambiarContrasena?:boolean;
  rol?:any[];
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
