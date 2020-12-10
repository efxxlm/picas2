import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RegistrarAvanceSemanalService {

  private urlApi = `${ environment.apiUrl }/RegisterWeeklyProgress`;

  constructor( private http: HttpClient ) { }

  getVRegistrarAvanceSemanal() {
    return this.http.get<any[]>( `${ this.urlApi }/GetVRegistrarAvanceSemanal` );
  }

  getLastSeguimientoSemanalByContratacionProyectoId( pContratacionProyectoId: number, pSeguimientoSemanalId: number ) {
    return this.http.get( `${ this.urlApi }/GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId?pContratacionProyectoId=${ pContratacionProyectoId }&pSeguimientoSemanalId=${ pSeguimientoSemanalId }` );
  }

}
