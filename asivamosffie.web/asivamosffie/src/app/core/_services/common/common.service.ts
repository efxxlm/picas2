import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { map } from 'rxjs/operators';
import { Observable, forkJoin } from 'rxjs';
import { Usuario } from '../autenticacion/autenticacion.service';
import { promise } from 'protractor';
import { estadosPreconstruccion } from '../../../_interfaces/faseUnoPreconstruccion.interface';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  
    
  constructor(private http: HttpClient) { }

  public loadProfiles() {
    const retorno = this.http.get<any[]>(`${environment.apiUrl}/common/perfiles`);
    return retorno;
  }

  public loadMenu() {
    const retorno = this.http.get<any[]>(`${environment.apiUrl}/common/GetMenuByRol`);
    return retorno;
  }

  getDocumento ( pPath: string ) {
    return this.http.get( `${ environment.apiUrl }/Document/GetFileByPath?pPath=${ pPath }`, { responseType: "blob" } );
  };

  listaTipoAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=3`);
  }
  

  listaNombreAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=4`);
  }
  listaNombreTipoAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=3`);
  }

  listaDepartamentos(){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListDepartamento`);
  }

  listaMunicipiosByIdDepartamento(pIdDepartamento: string){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListMunicipiosByIdDepartamento?idDepartamento=${pIdDepartamento}`);
  }

  listaTipoDocFinanciacion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=7`);
  }

  listaTipoIntervencion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=1`);
  }

  listaRegion(){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListRegion`);
  }

  listaDepartamentosByRegionId(pIdRegion:string){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListDepartamentoByRegionId?idRegion=${pIdRegion}`);
  }

  listaIntitucionEducativaByMunicipioId(pidMunicipio:string){
    return this.http.get<any[]>(`${environment.apiUrl}/Common/ListIntitucionEducativaByMunicipioId?idMunicipio=${pidMunicipio}`);
  }

  listaSedeByInstitucionEducativaId(pidInstitucionEducativaId:number){
    return this.http.get<any[]>(`${environment.apiUrl}/Common/ListSedeByInstitucionEducativaId?idInstitucionEducativaId=${pidInstitucionEducativaId}`);
  }
  listaTipoPredios() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=19`);
  }

  listaDocumentoAcrditacion() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=15`);
  }
  
  listaInfraestructuraIntervenir() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=5`);
  }

  listaCoordinaciones() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=6`);
  }

  listaConvocatoria() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=2`);
  }

  listaVigencias() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/ListVigenciaAporte`);
  }
  
  listaAportanteByTipoAportanteId(pTipoAportanteID:number){
    return this.http.get<any[]>(`${environment.apiUrl}/Cofinancing/GetListAportanteByTipoAportanteId?pTipoAportanteID=${pTipoAportanteID}`);
  }

  listaDocumentoByAportanteId(pAportanteID:number){
    return this.http.get<any[]>(`${environment.apiUrl}/Cofinancing/GetListDocumentoByAportanteId?pAportanteID=${pAportanteID}`);
  }

  listMunicipiosByIdMunicipio(idMunicipio:string){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/ListMunicipiosByIdMunicipio?idMunicipio=${idMunicipio}`);
  }
  
  listDepartamentoByIdMunicipio(idMunicipio:string){
    return this.http.get<Localizacion[]>(`${environment.apiUrl}/Common/listDepartamentoByIdMunicipio?idMunicipio=${idMunicipio}`);
  }
  
  listaTipoAlcance(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=9`);
  }

  listaTipoProcesoSeleccion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=27`);
  }

  listaPresupuestoProcesoSeleccion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=10`);
  }

  listaTipoProponente(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=28`);
  }

  listaEtapaProcesoSeleccion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=34`);
  }

  listaEstadoProcesoSeleccion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=35`);
  }
  listaEstadoProcesoSeleccionMonitoreo(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=79`);
  }

  listaTipoDisponibilidadPresupuestal(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=36`);
  }
  
  listaTipoDisponibilidadPresupuestalNotCode(minCode:string){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominioNotCode?pIdDominio=36&pMinCode=${minCode}`);
  }

  listaEstadoCronogramaSeguimiento(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=40`);
  }

  getUsuariosByPerfil( pIdPerfil: number ){
    return this.http.get<any[]>(`${environment.apiUrl}/Common/GetUsuariosByPerfil?pIdPerfil=${ pIdPerfil }`);
  }

  listaFases(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=16`);
  }
  listaSalarios(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=64`);
  }
  listaEtapaActualProceso(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=83`);
  }
  listaLimiteSalarios(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=65`);
  }

  listaComponentes(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=12`);
  }

  listaUsos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=13`);
  }

  listaMiembrosComiteTecnico(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=46`);
  }

  listaTipoSolicitud(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=29`);
  }

  listaEstadoSolicitud(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=50`);
  }

  listaPerfil () {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=11`);
  }

  listaEstadoCompromisos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=45`);
  }

  listaTipoTema(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=42`);
  }
  
  listaEstadoProyecto(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=63`);
  }

  listaTipoActividades() {
    return this.http.get<Dominio[]>( `${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=109` );
  }


listaEstadoRevision(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=61`);
  }

  listaTipoEnsayos() {
    return this.http.get<Dominio[]>( `${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=74` );
  }

  listaCausaAccidente() {
    return this.http.get<Dominio[]>( `${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=75` );
  }

  listaInstanciasdeSeguimientoTecnico(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=76`);
  }

  listaTipoNovedadModificacionContractual(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=76`);
  }
  
  listaProcesosJudiciales() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=105`);
  }

  listaTipoAccionJudicial() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=106`);
  }

  listaJurisdiccion() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=116`);
  }

  listaTipodocumento(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=30`);
  }

  getTipoActuacion() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=119`);
  }

  listaCanalIngreso(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=121`);
  }

  getEstadoAvanceProcesosDefensa() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=122`);
  }

  getEstadoActuacionDerivada() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=125`);
  }

  public listaUsuarios(){

    let lista: Usuario[] = [];

    return new Promise<Usuario[]>( resolve => {
      forkJoin([
        this.getUsuariosByPerfil(1),
        this.getUsuariosByPerfil(2),
        this.getUsuariosByPerfil(3),
        this.getUsuariosByPerfil(4),
        this.getUsuariosByPerfil(5),

      ]).subscribe( response => {

        for (let i = 0; i < 5; i++)
        {
          lista = lista.concat(response[i])  
        }

        resolve(lista);
      });
    })
  }

  public forkProject():Observable<any[]>
  {
    return forkJoin([
      this.listaTipoIntervencion(),
      this.listaRegion(),
      this.listaTipoPredios(),
      this.listaDocumentoAcrditacion(),
      this.listaTipoAportante(),
      this.listaInfraestructuraIntervenir(),
      this.listaCoordinaciones(),
      this.listaConvocatoria()
    
      ]);  
  }
  forkDepartamentoMunicipio(idMunicipio:string){
    return forkJoin([
      this.listMunicipiosByIdMunicipio(idMunicipio),
      this.listDepartamentoByIdMunicipio(idMunicipio)
      ]);  
    }
    
  listaFuenteRecursos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=18`);
  }

  listaFuenteTipoFinanciacion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=8`);
  }

  listaBancos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=22`);
  }

  listaTipoDDPEspecial(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=49`);
  }

  listaGarantiasPolizas(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=58`);
  }
  
  listaTiposDeControversiaContractual(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=100`);
  }
  listaMotivosSolicitudControversiaContractual(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=99`);
  }
  listaEstadosAvanceTramite(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=90`);
  }
  listaActuacionAdelantada(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=104`);
  }

  listaDisponibilidadMaterial(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=92`);
  }

  listaDisponibilidadEquipo(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=93`);
  }

  listaProductividad(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=94`);
  }

  listaCausaBajaDisponibilidadMaterial(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=95`);
  }

  listaCausaBajaDisponibilidadEquipo(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=96`);
  }

  listaCausaBajaDisponibilidadProductividad(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=97`);
  }

  listaProximaActuacionRequerida(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=103`);
  }

  listaEstadosAvanceReclamacion(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=127`);
  }

  listaEstadosControversiaNoTAI(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=128`);
  }

  listaRequiereSolicitudConcepto(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=129`);
  }
  listaEstadoAvanceMesaTrabajo(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=130`);
  }
  listaEtapaJudicial() {
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=124`);
  }

  vigenciasDesde2015(): number[]{
    const fecha = new Date();
    let vigencias: number[]=[];
    for (let i = 2015; i < fecha.getFullYear(); i++){
      vigencias.push(i);
    }

    return vigencias;
  }

  public getFileById(id: number) {   
    const retorno = this.http.get(`${environment.apiUrl}/Document/DownloadFilesById?pArchivoCargueId=${id}`, { responseType: "blob" });
    return retorno;
  }
}

export interface Dominio{
  descripcion?: string;
  dominioId?: number,
  tipoDominioId?: number,
  nombre?: string,
  activo?: boolean,
  codigo?: string,
}

export interface Localizacion{
  localizacionId: string,
  descripcion: string,
  idPadre:string
}

export interface Respuesta{
  isSuccessful: boolean;
  isValidation: boolean;
  isException: boolean;
  code: string;
  message: string;
  data?: any;
  token?: any;
}

interface TipoAportante{
  FFIE: string[];
  ET: string[];
  Tercero: string[];
}

export const TiposAportante: TipoAportante = {
  FFIE:   ["6"],
  ET:     ["9"],
  Tercero:["10"]
}

interface InstanciaSeguimientoTecnico{
  ComitedeObra: string[];
  ComitedeAsesores: string[];
  NoAplica: string[];
}

export const InstanciasSeguimientoTecnico: InstanciaSeguimientoTecnico = {
  ComitedeObra:   ["1"],
  ComitedeAsesores:     ["2"],
  NoAplica:["3"]
}


interface TipoNovedadModificacionContractual{
  Suspension: string[];
  ProrrogaSuspension: string[];
  Adicion: string[];
  Prorroga: string[];
  ModificacionCondicionesContractuales: string[];
}

export const TiposNovedadModificacionContractual: TipoNovedadModificacionContractual = {
  Suspension: ["1"],
  ProrrogaSuspension:["2"],
  Adicion: ["3"],
  Prorroga: ["4"],
  ModificacionCondicionesContractuales: ["5"]
}