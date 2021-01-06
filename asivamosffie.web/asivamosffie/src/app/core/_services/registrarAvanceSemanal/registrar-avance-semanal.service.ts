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

  getEnsayoLaboratorioMuestras( pGestionObraCalidadEnsayoLaboratorioId: number ) {
    return this.http.get<any[]>( `${ this.urlApi }/GetEnsayoLaboratorioMuestras?pGestionObraCalidadEnsayoLaboratorioId=${ pGestionObraCalidadEnsayoLaboratorioId }` );
  }

  getListSeguimientoSemanalByContratacionProyectoId( pContratacionProyectoId: number ) {
    return this.http.get( `${ this.urlApi }/GetListSeguimientoSemanalByContratacionProyectoId?pContratacionProyectoId=${ pContratacionProyectoId }` );
  }

  saveUpdateSeguimientoSemanal( pSeguimientoSemanal: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/SaveUpdateSeguimientoSemanal`, pSeguimientoSemanal );
  }

  createEditEnsayoLaboratorioMuestra( pGestionObraCalidadEnsayoLaboratorio: any ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditEnsayoLaboratorioMuestra`, pGestionObraCalidadEnsayoLaboratorio );
  }

  deleteManejoMaterialesInsumosProveedor( ManejoMaterialesInsumosProveedorId: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/DeleteManejoMaterialesInsumosProveedor?ManejoMaterialesInsumosProveedorId=${ ManejoMaterialesInsumosProveedorId }`, '' );
  }

  deleteResiduosConstruccionDemolicionGestor( ResiduosConstruccionDemolicionGestorId: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/DeleteResiduosConstruccionDemolicionGestor?ResiduosConstruccionDemolicionGestorId=${ ResiduosConstruccionDemolicionGestorId }`, '' );
  }

  deleteGestionObraCalidadEnsayoLaboratorio( gestionObraCalidadEnsayoLaboratorioId: number ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/DeleteGestionObraCalidadEnsayoLaboratorio?GestionObraCalidadEnsayoLaboratorioId=${ gestionObraCalidadEnsayoLaboratorioId }`, '' );
  }

}
