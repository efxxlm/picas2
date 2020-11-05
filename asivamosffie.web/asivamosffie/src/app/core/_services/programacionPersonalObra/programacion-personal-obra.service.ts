import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class ProgramacionPersonalObraService {

  private urlApi: string = `${ environment.apiUrl }/RegisterPersonalProgramming`;

  constructor ( private http: HttpClient )
  {};

  getListProyectos () {
    return this.http.get<any[]>( `${ this.urlApi }/GetListProyectos` );
  };

  getProgramacionPersonalByContratoConstruccionId ( pContratoConstruccionId: number ) {
    return this.http.get<any[]>( `${ this.urlApi }/GetProgramacionPersonalByContratoConstruccionId?pContratoConstruccionId=${ pContratoConstruccionId }` );
  };

  updateProgramacionContratoPersonal ( pContratoConstruccion: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/UpdateProgramacionContratoPersonal`, pContratoConstruccion )
  }


};