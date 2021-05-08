import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { pid } from 'process';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';
import { FuenteFinanciacion } from '../fuenteFinanciacion/fuente-financiacion.service';

@Injectable({
  providedIn: 'root'
})
export class ContractualNoveltyService {

  private urlApi = `${ environment.apiUrl }/ContractualNovelty`;

  constructor(private http: HttpClient) { }

  createEditNovedadContractual( novedadContractual: NovedadContractual ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditNovedadContractual`, novedadContractual );
  }

  getContratosAutocomplete()
  {
    return this.http.get<Contrato[]>( `${ this.urlApi }/GetListContract` ); 
  }
  
  postRegistroNovedadContractual( pNContrato: FormData) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditarModification`, pNContrato );
  }
  
  getListGrillaNovedadContractualObra()
  {
    return this.http.get<any[]>( `${ this.urlApi }/getListGrillaNovedadContractualObra` ); 
  }

  getListGrillaNovedadContractualInterventoria()
  {
    return this.http.get<any[]>( `${ this.urlApi }/getListGrillaNovedadContractualInterventoria` ); 
  }

  getProyectosContrato(ncontrato) {
    return this.http.get<any[]>( `${ this.urlApi }/GetProyectsByContract?pContratoId=${ncontrato}` ); 
  }

  eliminarNovedadContractual(id) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/eliminarNovedadContractual?pNovedaContractual=${ id }` ); 
  }

  eliminarNovedadClausula(id) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/EliminarNovedadClausula?pNovedadContractuaClausulalId=${ id }` ); 
  }

  getNovedadContractualById(id) {
    return this.http.get<NovedadContractual>( `${ this.urlApi }/getNovedadContractualById?pId=${ id }` ); 
  }

  aprobarSolicitud( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/aprobarSolicitud?pNovedaContractual=${ id }`, null ); 
  }

  enviarAlSupervisor( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/enviarAlSupervisor?pNovedaContractual=${ id }`, null ); 
  }

  createEditObservacion( novedad: NovedadContractual, esSupervisor?: boolean, esTramite?: boolean ){
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditObservacion?esSupervisor=${ esSupervisor }&esTramite=${ esTramite }`, novedad ); 
  }

  tramitarSolicitud( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/tramitarSolicitud?pNovedaContractual=${ id }`, null ); 
  }

  devolverSolicitud( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/devolverSolicitud?pNovedaContractual=${ id }`, null ); 
  }

  devolverSolicitudASupervisor( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/DevolverSolicitudASupervisor?pNovedaContractual=${ id }`, null ); 
  }

  getListGrillaNovedadContractualGestionar()
  {
    return this.http.get<any[]>( `${ this.urlApi }/getListGrillaNovedadContractualGestionar` ); 
  }

  createEditNovedadContractualTramite( novedadContractual: NovedadContractual ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditNovedadContractualTramite`, novedadContractual );
  }

  enviarAComite( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/enviarAComite?pNovedaContractual=${ id }`, null ); 
  }

  rechazarPorInterventor( novedad ){
    return this.http.put<Respuesta>( `${ this.urlApi }/rechazarPorInterventor`, novedad ); 
  }

  rechazarPorSupervisor( novedad ){
    return this.http.put<Respuesta>( `${ this.urlApi }/rechazarPorSupervisor`, novedad ); 
  }

  GetAportanteByContratacion(pId)
  {
    return this.http.get<any[]>( `${ this.urlApi }/GetAportanteByContratacion?pId=${pId}` ); 
  }

  
  GetFuentesByAportante(pId)
  {
    return this.http.get<FuenteFinanciacion[]>( `${ this.urlApi }/GetFuentesByAportante?pConfinanciacioAportanteId=${pId}` ); 
  }

  CancelarNovedad( pId ){
    return this.http.put<Respuesta>( `${ this.urlApi }/CancelarNovedad?pNovedadContractualId=${pId}`, null ); 
  }

  AprobacionTecnicaJuridica( id ){
    return this.http.put<Respuesta>( `${ this.urlApi }/AprobacionTecnicaJuridica?pNovedaContractual=${ id }`, null ); 
  }

  eliminarNovedadContractualAportante(pNovedadContractualAportante : number) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/EliminarNovedadContractualAportante?pNovedadContractualAportante=${ pNovedadContractualAportante }` ); 
  }

  eliminarComponenteAportanteNovedad(pComponenteAportanteNovedad : number) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/EliminarComponenteAportanteNovedad?pComponenteAportanteNovedad=${ pComponenteAportanteNovedad }` ); 
  }

  eliminarComponenteFuenteNovedad(pComponenteFuenteNovedad : number) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/EliminarComponenteFuenteNovedad?pComponenteFuenteNovedad=${ pComponenteFuenteNovedad }` ); 
  }

  eliminarComponenteUsoNovedad(pComponenteUsoNovedad : number) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/EliminarComponenteUsoNovedad?pComponenteUsoNovedad=${ pComponenteUsoNovedad }` ); 
  }
}
