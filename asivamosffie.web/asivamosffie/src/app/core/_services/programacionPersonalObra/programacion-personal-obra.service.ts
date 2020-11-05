import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

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
    return this.http.get( `${ this.urlApi }/GetProgramacionPersonalByContratoConstruccionId?pContratoConstruccionId=${ pContratoConstruccionId }` );
  };

};