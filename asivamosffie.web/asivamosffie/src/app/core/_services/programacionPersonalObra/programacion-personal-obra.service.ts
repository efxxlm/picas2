import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DetalleProgramacionPersonal, ListaProgramacionPersonal, pContratoConstruccion } from 'src/app/_interfaces/programacionPersonal.interface';
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
    return this.http.get<ListaProgramacionPersonal[]>( `${ this.urlApi }/GetListProyectos` );
  };

  getProgramacionPersonalByContratoConstruccionId ( pContratoConstruccionId: number ) {
    return this.http.get<DetalleProgramacionPersonal[]>( `${ this.urlApi }/GetProgramacionPersonalByContratoConstruccionId?pContratoConstruccionId=${ pContratoConstruccionId }` );
  };

  updateProgramacionContratoPersonal ( pContratoConstruccion: pContratoConstruccion ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/UpdateProgramacionContratoPersonal`, pContratoConstruccion )
  };

  changeStatusProgramacionContratoPersonal ( pContratoConstruccionId: number, pEstadoProgramacionCodigo: string ) {
    return this.http.post<Respuesta>( `${ this.urlApi}/ChangeStatusProgramacionContratoPersonal?pContratoConstruccionId=${ pContratoConstruccionId }&pEstadoProgramacionCodigo=${ pEstadoProgramacionCodigo }`, '' )
  };

};