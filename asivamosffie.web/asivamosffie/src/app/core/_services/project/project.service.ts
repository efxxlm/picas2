import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { CofinanciacionAportante, CofinanciacionDocumento } from '../Cofinanciacion/cofinanciacion.service';
import { Observable, forkJoin } from 'rxjs';
import { ContratacionProyecto, Contratacion } from 'src/app/_interfaces/project-contracting';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient) {  }

  public uploadFileProject(archivoParaSubir: File) {
    const formData = new FormData(); 
    formData.append('file', archivoParaSubir, archivoParaSubir.name);
    //si se requieren mas variables
    //formData.append('proyectId',projectid);
    const retorno = this.http.post<Respuesta>(`${environment.apiUrl}/Project/SetValidateMassiveLoadProjects`,formData);
    return retorno;
  }

  public uploadOkProjectsFileProject(idProject: string) {   
    const retorno = this.http.post<Respuesta>(`${environment.apiUrl}/Project/UploadMassiveLoadProjects?pIdDocument=${idProject}`,null);
    return retorno;
  }

  public getListProjectsFileProject() {   
    const retorno = this.http.get<any>(`${environment.apiUrl}/Document/GetListloadedDocuments`);
    return retorno;
  }

  public getListProjectsFileProjectByOrigenId( pOrigenId: string ) {   
    const retorno = this.http.get<any>(`${environment.apiUrl}/Document/GetListloadedDocuments?pOrigenId=${ pOrigenId }`);
    return retorno;
  }

  public getFileByName(name: string) {   
    const retorno = this.http.get(`${environment.apiUrl}/Document/DownloadFilesByName?pNameFiles=${name}`, { responseType: "blob" });
    return retorno;
  }

  public getListProjects() {   
    const retorno = this.http.get<any>(`${environment.apiUrl}/Project/ListProject`);
    return retorno;
  }

  
  public createOrUpdateProyect(proyecto: Proyecto) {   
    const retorno = this.http.post<Respuesta>(`${environment.apiUrl}/Project/CreateOrUpdateProyect`,proyecto);
    return retorno;
  }
  public getProjectById(pIdProject:number) {   
    const retorno = this.http.get<Proyecto>(`${environment.apiUrl}/Project/GetProyectoByProyectoId?pProyectoId=${pIdProject}`);
    return retorno;
  }

  public deleteProjectById(pIdProject:number) {   
    const retorno = this.http.get<Proyecto>(`${environment.apiUrl}/Project/DeleteProyectoByProyectoId?pProyectoId=${pIdProject}`);
    return retorno;
  }  
  
  public CreateOrUpdateAdministrativeProyect(pIdProject:ProyectoAdministrativo) {   
    const retorno = this.http.post<Respuesta>(`${environment.apiUrl}/Project/CreateOrEditAdministrativeProject`,pIdProject);
    return retorno;
  }

  public ListAdministrativeProject() {   
    const retorno = this.http.get<any>(`${environment.apiUrl}/Project/ListAdministrativeProject`);
    return retorno;
  }

  public DeleteProyectoAdministrativoByProyectoId(pIdProject:number) {   
    const retorno = this.http.get<any>(`${environment.apiUrl}/Project/DeleteProyectoAdministrativoByProyectoId?pProyectoId=${pIdProject}`);
    return retorno;
  }

  public EnviarProyectoAdministrativoByProyectoId(pIdProject:number) {   
    const retorno = this.http.get<any>(`${environment.apiUrl}/Project/EnviarProyectoAdministrativoByProyectoId?pProyectoId=${pIdProject}`);
    return retorno;
  }

  public listaFuentes(pAportanteId:number) {
    return this.http.get<any>(`${environment.apiUrl}/Project/GetFontsByAportantID?pAportanteId=${pAportanteId}`);
  }

  public listaProyectoConFiltros(pTipoIntervencion: string, pLlaveMen: string, pRegion: string, pDepartamento: string, pMunicipio: string, pIdInstitucionEducativa: number, pIdSede: number){
    return this.http.get<ProyectoGrilla[]>(`${environment.apiUrl}/ProjectContracting/getListProyectsByFilters?pTipoIntervencion=${
      pTipoIntervencion }&pLlaveMen=${ pLlaveMen }&pRegion=${ pRegion }&pDepartamento=${ pDepartamento 
      }&pMunicipio=${ pMunicipio }&pIdInstitucionEducativa=${ pIdInstitucionEducativa }&pIdSede=${ pIdSede }`);
  }

  getProyectoGrillaByProyectoId( id: number ){
    return this.http.get<ProyectoGrilla>(`${environment.apiUrl}/Project/getProyectoGrillaByProyectoId?idProyecto=${ id }`);
   }

   

}
export interface RespuestaProyecto{
  cantidadDeRegistros: number,
  cantidadDeRegistrosInvalidos: number,
  cantidadDeRegistrosValidos: number,
  llaveConsulta: string
}

export interface ProyectoAdministrativoAportante {
  proyectoAdministrativoAportanteId?: number,
  proyectoAdminstrativoId?: number,
  aportanteId?: number,
  eliminado?: boolean,
  usuarioCreacion?: string,
  fechaCreacion?: Date,
  usuarioEdicion?: string,
  fechaEdicion?: Date,

  cofinanciacionAportante?: Aportante
  aportanteFuenteFinanciacion?:AportanteFuenteFinanciacion[]
}
export interface AportanteFuenteFinanciacion{
    aportanteFuenteFinanciacionId?:number,
    proyectoAdministrativoAportanteId:number,
    fuenteFinanciacionId:number,
    valorFuente:number,
    fuenteRecursosCodigo?:string
        
}

export interface ProyectoAdministrativo
{
  proyectoAdministrativoId?: number,
  enviado?: boolean,
  fechaCreado?: Date,
  usuarioCreacion?: string,
  fechaModificacion?: Date,
  usuarioModificacion?: string,
  eliminado?: boolean,
  registroCompleto?: boolean,   
  identificador?:string
  proyectoAdministrativoAportante?:ProyectoAdministrativoAportante[],
}
export interface Aportante
{
  aportanteId?:number;
  tipoAportanteId?:number;
  nombreAportanteId?:number;

  nombreAportante?: string;

  fuenteFinanciacion?:FuenteFinanciacion[],
}

export interface FuenteFinanciacion{  
  fuenteRecursosCodigo:string,
  valorFuente:number
}

export interface Listados{
  id:string,
  valor:string
}

export interface Proyecto{
  proyectoId?:number,
  fechaSesionJunta?: Date,
  numeroActaJunta?:number,
  tipoIntervencionCodigo?:number,
  llaveMen?:string,
  localizacionIdMunicipio?:string,
  institucionEducativaId?:number,
  sedeId?:number,
  enConvocatoria?:boolean,
  convocatoriaId?:number,
  cantPrediosPostulados?:number,
  tipoPredioCodigo?:string,
  predioPrincipalId?:number,
  valorObra?:number,
  valorInterventoria?:number,
  valorTotal?:number,
  estadoProyectoCodigo?:string,
  eliminado?:boolean,
  fechaCreacion?: Date,
  usuarioCreacion?:string,
  fechaModificacion?: Date,
  usuarioModificacion?:string,
  //no modelado
  cantidadAportantes?:number;
  regid?:string;
  depid?:string;

  institucionEducativa?:InstitucionEducativa
  institucionEducativaSede?:InstitucionEducativa,
  localizacionIdMunicipioNavigation?: Localizacion,
  predioPrincipal?: Predio,
  sede?:InstitucionEducativa,
  infraestructuraIntervenirProyecto?:InfraestructuraIntervenirProyecto[],
  proyectoAportante?:ProyectoAportante[],
  proyectoPredio?:ProyectoPredio[],
  
}

export interface InstitucionEducativa{  
 institucionEducativaSedeId:number ;
  padreId?:number ,
  nombre:string ,
  codigoDane?:string ,
  localizacionIdMunicipio?:number ,
  fechaCreacion:Date ,
  usuarioCreacion:string ,
  activo:boolean ,
}

export interface Localizacion{  
  LocalizacionId:string ,
  Descripcion:string ,
  IdPadre:string ,
  Nivel?:string ,
  Tipo :string,
}
export interface Predio{  
  predioId :number,
  institucionEducativaSedeId:number ,
  tipoPredioCodigo:string ,
  ubicacionLatitud:string ,
  ubicacionLongitud:string ,
  ubicacionLatitud2?:string ,
  ubicacionLongitud2?:string ,
  direccion:string ,
  documentoAcreditacionCodigo :string,
  numeroDocumento:string ,
  cedulaCatastral:string ,
  activo?:boolean ,
  fechaCreacion:Date ,
  usuarioCreacion:string ,
}
export interface InfraestructuraIntervenirProyecto{  
  infraestrucutraIntervenirProyectoId :number,
  proyectoId :number,
  infraestructuraCodigo:string ,
  cantidad :number,
  eliminado:boolean ,
  fechaCreacion:Date ,
  usuarioCreacion:string ,
  fechaEliminacion?:Date ,
  usuarioEliminacion:string ,
  plazoMesesObra :number,
  plazoDiasObra:number ,
  plazoMesesInterventoria:number ,
  plazoDiasInterventoria:number ,
  coordinacionResponsableCodigo:string ,
}
export interface ProyectoAportante{
  mun?: string;
  depto?: string;
  nombreAportante?: any;  
  proyectoAportanteId :number,
  proyectoId :number,
  aportanteId :number,
  valorObra? :number ,
  valorInterventoria? :number ,
  valorTotalAportante? :number ,
  eliminado :boolean,
  fechaCreacion:Date ,
  usuarioCreacion:string ,
  cofinanciacionDocumentoID:number,
  aportante?:CofinanciacionAportante,
  cofinanciacionDocumento?:CofinanciacionDocumento
}
export interface ProyectoPredio{  
  ProyectoPredioId:number ,
  ProyectoId?:number ,
  PredioId? :number,
  //EstadoJuridicoCodigo :string,
  Activo?:boolean ,
  FechaCreacion?:Date ,
  UsuarioCreacion:string ,
  Predio:Predio
}

export interface ProyectoGrilla{
  proyectoId?: number,
  departamento?: string,
  municipio?: string,
  institucionEducativa?: string,
  sede?: string,
  estadoRegistro?: string,
  estadoJuridicoPredios?: string,
  fecha?: string,
  tipoIntervencion?: string,
  llaveMen?: string,

}