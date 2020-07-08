import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { CofinanciacionAportante } from '../Cofinanciacion/cofinanciacion.service';

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

  public getFileByName(name: string) {   
    const retorno = this.http.get(`${environment.apiUrl}/Document/DownloadFilesByName?pNameFiles=${name}`, { responseType: "blob" });
    return retorno;
  }
  
}
export interface RespuestaProyecto{
  cantidadDeRegistros: number,
  cantidadDeRegistrosInvalidos: number,
  cantidadDeRegistrosValidos: number,
  llaveConsulta: string
}

export interface ProyectoAdministrativo
{
  Aportante:Aportante[],  
  identificador:string
}
export interface Aportante
{
  AportanteId:number;
  FuenteFinanciacion:FuenteFinanciacion[],
}

export interface FuenteFinanciacion{  
  FuenteRecursosCodigo:string,
  ValorFuente:number
}

export interface Listados{
  id:string,
  valor:string
}

export class Proyecto{
  ProyectoId:number;
  FechaSesionJunta?: Date;
  NumeroActaJunta:number;
  TipoIntervencionCodigo:string;
  LlaveMen:string;
  LocalizacionIdMunicipio:string;
  InstitucionEducativaId:number;
  SedeId:number;
  EnConvocatoria:boolean;
  ConvocatoriaId?:number;
  CantPrediosPostulados:string;
  TipoPredioCodigo:string;
  PredioPrincipalId:number;
  ValorObra:number;
  ValorInterventoria:number;
  ValorTotal:number;
  EstadoProyectoCodigo:string;
  Eliminado?:boolean;
  FechaCreacion: Date;
  UsuarioCreacion:string;
  FechaModificacion?: Date;
  UsuarioModificacion:string;

  InstitucionEducativaSede:InstitucionEducativa;
  LocalizacionIdMunicipioNavigation: Localizacion;
  Predio: PredioPrincipal;
  Sede:InstitucionEducativa;
  InfraestructuraIntervenirProyecto:InfraestructuraIntervenirProyecto[];
  ProyectoAportante:ProyectoAportante[];
  ProyectoPredio:ProyectoPredio[];
  
}

export interface InstitucionEducativa{  
 InstitucionEducativaSedeId:number ;
  PadreId?:number ,
  Nombre:string ,
  CodigoDane?:number ,
  LocalizacionIdMunicipio?:number ,
  FechaCreacion:Date ,
  UsuarioCreacion:string ,
  Activo:boolean ,
}

export interface Localizacion{  
  LocalizacionId:string ,
  Descripcion:string ,
  IdPadre:string ,
  Nivel?:string ,
  Tipo :string,
}
export interface PredioPrincipal{  
  PredioId :number,
  InstitucionEducativaSedeId:number ,
  TipoPredioCodigo:string ,
  UbicacionLatitud:string ,
  UbicacionLongitud:string ,
  Direccion:string ,
  DocumentoAcreditacionCodigo :string,
  NumeroDocumento:string ,
  CedulaCatastral:string ,
  Activo?:boolean ,
  FechaCreacion:Date ,
  UsuarioCreacion:string ,
}
export interface InfraestructuraIntervenirProyecto{  
  InfraestrucutraIntervenirProyectoId :number,
  ProyectoId :number,
  InfraestructuraCodigo:string ,
  Cantidad :number,
  Eliminado:boolean ,
  FechaCreacion:Date ,
  UsuarioCreacion:string ,
  FechaEliminacion?:Date ,
  UsuarioEliminacion:string ,
  PlazoMesesObra :number,
  PlazoDiasObra:number ,
  PlazoMesesInterventoria:number ,
  PlazoDiasInterventoria:number ,
  CoordinacionResponsableCodigo:string ,
}
export interface ProyectoAportante{  
  ProyectoAportanteId :number,
  ProyectoId :number,
  AportanteId :number,
  ValorObra? :number ,
  ValorInterventoria? :number ,
  ValorTotalAportante? :number ,
  Eliminado :boolean,
  FechaCreacion:Date ,
  UsuarioCreacion:string ,
}
export interface ProyectoPredio{  
  ProyectoPredioId:number ,
  ProyectoId?:number ,
  PredioId? :number,
  EstadoJuridicoCodigo :string,
  Activo?:boolean ,
  FechaCreacion?:Date ,
  UsuarioCreacion:string ,
}