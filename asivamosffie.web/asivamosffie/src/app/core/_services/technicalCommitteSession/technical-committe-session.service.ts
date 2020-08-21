import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Respuesta } from '../common/common.service';
import { environment } from 'src/environments/environment';
import { SolicitudesContractuales, SesionComiteTema, Sesion, ComiteGrilla } from 'src/app/_interfaces/technicalCommitteSession';
import { Session } from 'protractor';


@Injectable({
  providedIn: 'root'
})
export class TechnicalCommitteSessionService {

  constructor(
              private http: HttpClient,

             ) 
  { }

  getListSolicitudesContractuales( fechaComite ){
    return this.http.get<SolicitudesContractuales[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/GetListSolicitudesContractuales?FechaComite=${ fechaComite }`);
  }

  saveEditSesionComiteTema( sesion: Sesion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/saveEditSesionComiteTema`, sesion );
  }

  getComiteGrilla(){
    return this.http.get<ComiteGrilla[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/GetComiteGrilla`);
  }

  getListSesionComiteTemaByIdSesion( id: number){
    return this.http.get<SesionComiteTema[]>(`${environment.apiUrl}/RegisterSessionTechnicalCommittee/getListSesionComiteTemaByIdSesion?pIdSesion=${ id }`);
  }

}
