import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { map } from 'rxjs/operators';
import { Observable, forkJoin } from 'rxjs';
import { Usuario } from '../autenticacion/autenticacion.service';
import { promise } from 'protractor';

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

  listaTipoAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=3`);
  }

  listaNombreAportante(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=4`);
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

  listaEstadoCronogramaSeguimiento(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=40`);
  }

  getUsuariosByPerfil( pIdPerfil: number ){
    return this.http.get<Usuario[]>(`${environment.apiUrl}/Common/GetUsuariosByPerfil?pIdPerfil=${ pIdPerfil }`);
  }

  listaFases(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=16`);
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

  listaEstadoSolicitud(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=31`);
  }

  listaEstadoCompromisos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=45`);
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

  listaBancos(){
    return this.http.get<Dominio[]>(`${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=22`);
  }

  vigenciasDesde2015(): number[]{
    const fecha = new Date();
    let vigencias: number[]=[];
    for (let i = 2015; i < fecha.getFullYear(); i++){
      vigencias.push(i);
    }

    return vigencias;
  }
}

export interface Dominio{
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


