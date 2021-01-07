import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Contratacion, ContratacionProyecto, ContratistaGrilla } from 'src/app/_interfaces/project-contracting';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProjectContractingService {

  constructor(
                private http: HttpClient,
                
             ) 
  { }

  createContratacionProyecto( contratacion: Contratacion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ProjectContracting/CreateContratacionProyecto`, contratacion);
  }
  getListContratacionProyectoByContratacionId( contratacionId: number ){
    return this.http.get<ContratacionProyecto[]>(`${environment.apiUrl}/ProjectContracting/GetListContratacionProyectoByContratacionId?idContratacion=${ contratacionId }`);
  }

  getListContratacion(){
    return this.http.get<Contratacion[]>(`${environment.apiUrl}/ProjectContracting/getListContratacion`);
  }

  getContratacionByContratacionId( contratacionId: number ){
    return this.http.get<Contratacion>(`${environment.apiUrl}/ProjectContracting/getContratacionByContratacionId?pContratacionId=${ contratacionId }`);
  }

  getListContractingByFilters(pNumeroIdentidicacion: string, pNombre: string, EsConsorcio: boolean){
    return this.http.get<ContratistaGrilla[]>(`${environment.apiUrl}/ProjectContracting/getListContractingByFilters?pNumeroIdentidicacion=${ pNumeroIdentidicacion }&pNombre=${ pNombre }&EsConsorcio=${ EsConsorcio }`);
  }

  getContratacionProyectoById( contratacionProyectoId: number ){
    return this.http.get<ContratacionProyecto>(`${environment.apiUrl}/ProjectContracting/getContratacionProyectoById?idContratacionProyecto=${ contratacionProyectoId }`);
  }

  createEditContratacionProyecto( contratacionProyecto: ContratacionProyecto ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ProjectContracting/createEditContratacionProyecto`, contratacionProyecto );
  }

  createEditContratacion( contratacion: Contratacion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ProjectContracting/createEditContratacion`, contratacion );
  }

  createEditContratacionProyectoAportanteByContratacionproyecto( contratacionProyecto: ContratacionProyecto  ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/ProjectContracting/createEditContratacionProyectoAportanteByContratacionproyecto`, contratacionProyecto );

  }

  eliminarContratacion( id: number )
  {
    return this.http.delete<Respuesta>(`${environment.apiUrl}/ProjectContracting/DeleteContratacionByIdContratacion?idContratacion=${ id }`);
  }

  changeStateContratacionByIdContratacion( id: number, estado: string )
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/ProjectContracting/changeStateContratacionByIdContratacion?idContratacion=${ id }&PCodigoEstado=${ estado }`, null);
  }

  getContratacionByContratacionIdWithGrillaProyecto( id: number ){
    return this.http.get<Contratacion>(`${environment.apiUrl}/ProjectContracting/getContratacionByContratacionIdWithGrillaProyecto?pContratacionId=${ id }`);
   }

  getListFaseComponenteUso( ){
    return this.http.get<any>( `${ environment.apiUrl }/ProjectContracting/getListFaseComponenteUso` );
  }

}
