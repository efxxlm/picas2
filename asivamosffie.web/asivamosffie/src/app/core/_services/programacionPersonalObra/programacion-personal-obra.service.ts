import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DetalleProgramacionPersonal, ListaProgramacionPersonal, ContratoConstruccion } from 'src/app/_interfaces/programacionPersonal.interface';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class ProgramacionPersonalObraService {

  private urlApi = `${ environment.apiUrl }/RegisterPersonalProgramming`;

  constructor( private http: HttpClient )
  {}

  getListProyectos() {
    return this.http.get<ListaProgramacionPersonal[]>( `${ this.urlApi }/GetListProyectos` );
  }

  getProgramacionPersonalByContratoConstruccionId( pContratoConstruccionId: number ) {
    return this.http.get<DetalleProgramacionPersonal[]>( `${ this.urlApi }/GetProgramacionPersonalByContratoId?pContratacionProyectoId=${ pContratoConstruccionId }` );
  }

  updateProgramacionContratoPersonal( contratoConstruccion: any[] ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/UpdateSeguimientoSemanalPersonalObra`, contratoConstruccion );
  }

  changeStatusProgramacionContratoPersonal( pContratoConstruccionId: number, pEstadoProgramacionCodigo: string ) {
    return this.http.post<Respuesta>( `${ this.urlApi}/ChangeStatusProgramacionContratoPersonal?pContratoConstruccionId=${ pContratoConstruccionId }&pEstadoProgramacionCodigo=${ pEstadoProgramacionCodigo }`, '' );
  }

}
