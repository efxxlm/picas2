import { catchError } from 'rxjs/operators';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegistrarAvanceSemanalService {

  private urlApi = `${ environment.apiUrl }/RegisterWeeklyProgress`;

  constructor( private http: HttpClient ) { }

  getVRegistrarAvanceSemanal() {
    return this.http.get<any[]>( `${ this.urlApi }/GetVRegistrarAvanceSemanal` );
  }

  getLastSeguimientoSemanalContratacionProyectoIdOrSeguimientoSemanalId( pContratacionProyectoId: number, pSeguimientoSemanalId: number ) {
    return this.http.get(
      `${ this.urlApi }/GetLastSeguimientoSemanalByContratacionProyectoIdOrSeguimientoSemanalId?pContratacionProyectoId=${ pContratacionProyectoId }&pSeguimientoSemanalId=${ pSeguimientoSemanalId }`
    );
  }

  getListSeguimientoSemanalByContratacionProyectoId( pContratacionProyectoId: number ) {
    return this.http.get( `${ this.urlApi }/GetListSeguimientoSemanalByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }` );
  }

  saveUpdateSeguimientoSemanal( pSeguimientoSemanal: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/SaveUpdateSeguimientoSemanal`, pSeguimientoSemanal );
  }

}
