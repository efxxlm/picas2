import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { GrillaDisponibilidadPresupuestal, DisponibilidadPresupuestal, CustonReuestCommittee, ListAportantes, ListConcecutivoProyectoAdministrativo, ListAdminProyect } from 'src/app/_interfaces/budgetAvailability';
import { Respuesta } from '../common/common.service';
import { Proyecto } from '../project/project.service';
import { DisponibilidadPresupuestalService } from '../disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Injectable({
  providedIn: 'root'
})
export class BudgetAvailabilityService {
  

  constructor(
    private http: HttpClient
  ) {

  }

  // getGridBudgetAvailability() {
  //   return this.http.get<any>(`${environment.apiUrl}/BudgetAvailability/GetGridBudgetAvailability`);
  // }

  getDisponibilidadPresupuestalById( id: number ){    
    return this.http.get<DisponibilidadPresupuestal>(`${environment.apiUrl}/BudgetAvailability/${ id }`);
  }

  createEditarDP( disponibilidad: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/BudgetAvailability/createEditarDP`, disponibilidad);
  }

  getReuestCommittee(){
    return this.http.get<CustonReuestCommittee[]>(`${environment.apiUrl}/RequestBudgetAvailability/GetReuestCommittee`);
  }

  createOrEditInfoAdditional( disponibilidadPresupuestal: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/createOrEditInfoAdditional`, disponibilidadPresupuestal);
  }

  sendRequest( DisponibilidadId: number, RegistroPId: number, esNovedad: boolean ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/sendRequest?disponibilidadPresupuestalId=${ DisponibilidadId }&RegistroPId=${RegistroPId}&esNovedad=${esNovedad}`, null);
  }
  
  deleteRequest( id: number ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/EliminarDisponibilidad?disponibilidadPresupuestalId=${ id }`, null);
  }

  searchLlaveMEN( texto: string ){
    return this.http.get<Proyecto[]>(`${environment.apiUrl}/RequestBudgetAvailability/searchLlaveMEN?LlaveMEN=${ texto }`);    
  }

  getAportantesByProyectoId( id: number ){
    return this.http.get<ListAportantes[]>(`${environment.apiUrl}/RequestBudgetAvailability/getAportantesByProyectoId?proyectoId=${ id }`);    
  }

  createOrEditDDPRequest( disponibilidadPresupuestal: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/createOrEditDDPRequest`, disponibilidadPresupuestal);
  }

  getDDPEspecial(){
    return this.http.get<ListAportantes[]>(`${environment.apiUrl}/RequestBudgetAvailability/getDDPEspecial`);    
  }

  getDetailInfoAdditionalById( id: number ){
    return this.http.get<DisponibilidadPresupuestal>(`${environment.apiUrl}/RequestBudgetAvailability/getDetailInfoAdditionalById?disponibilidadPresupuestalId=${ id }`);    
  }

  getListCocecutivoProyecto(){
    return this.http.get<ListConcecutivoProyectoAdministrativo[]>(`${environment.apiUrl}/RequestBudgetAvailability/getListCocecutivoProyecto`);
  }

  getAportantesByProyectoAdminId( id: number ){
    return this.http.get<ListAdminProyect[]>(`${environment.apiUrl}/RequestBudgetAvailability/GetAportantesByProyectoAdministrativoId?proyectoId=${ id }`);    
  }

  createOrEditProyectoAdministrtivo( disponibilidad: DisponibilidadPresupuestal ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/createOrEditProyectoAdministrtivo`, disponibilidad);
  }

  getDDPAdministrativa(){
    return this.http.get<DisponibilidadPresupuestal[]>(`${environment.apiUrl}/RequestBudgetAvailability/getDDPAdministrativa`);    
  }

  eliminarDisponibilidad( id: number ){
    return this.http.delete<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/eliminarDisponibilidad?disponibilidadPresupuestalId=${ id }`);    
  }

  getNumeroContrato ( numeroContrato: string ) {
    return this.http.get( `${ environment.apiUrl }/RequestBudgetAvailability/GetListContatoByNumeroContrato?pNumero=${ numeroContrato }` );
  }

  getContratoByNumeroContrato ( numeroContrato: string ) {
    return this.http.get( `${ environment.apiUrl }/RequestBudgetAvailability/GetContratoByNumeroContrato?pNumero=${ numeroContrato }` );
  }

  createUpdateDisponibilidaPresupuestalEspecial ( pDisponibilidadPresupuestal: any ) {
    return this.http.post<Respuesta>( `${ environment.apiUrl }/RequestBudgetAvailability/CreateUpdateDisponibilidaPresupuestalEspecial`, pDisponibilidadPresupuestal )
  };

  getAportanteTerritorial ( pProyectoId: number, pTipoAportanteId: number ) {
    return this.http.get( `${ environment.apiUrl }/RequestBudgetAvailability/GetListAportanteByTipoAportanteByProyectoId?pProyectoId=${ pProyectoId }&pTipoAportanteId=${ pTipoAportanteId }` );
  };

  getContratosList(): any {
    return this.http.get<any[]>( `${ environment.apiUrl }/RequestBudgetAvailability/GetContratos` );
  }

  getNovedadContractual(contratacionId: number) {
    return this.http.get<any>( `${ environment.apiUrl }/RequestBudgetAvailability/getNovedadContractualByContratacionId?contratacionId=${contratacionId}`);
  }  
  
  createOrEditInfoAdditionalNoveltly( registroPresupuesta, pContratacionId ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/RequestBudgetAvailability/CreateOrEditInfoAdditionalNoveltly?pContratacionId=${pContratacionId}`, registroPresupuesta);
  }

};
interface TipoDDP{
  DDP_tradicional: string;
  DDP_especial: string;
  DDP_administrativo: string;
}

export const TipoDDP: TipoDDP = {
  DDP_tradicional:   "1",
  DDP_especial:     "2",
  DDP_administrativo:"3"
}
