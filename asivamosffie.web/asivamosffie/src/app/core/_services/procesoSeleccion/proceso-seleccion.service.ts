import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class ProcesoSeleccionService implements OnInit {
    
  

  constructor(
               private http: HttpClient
             ) 
             
{ }
  ngOnInit(): void {

  }

  guardarEditarProcesoSeleccion( procesoSeleccion: ProcesoSeleccion )
  {
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/CreateEditarProcesoSeleccion`, procesoSeleccion);
  }

  listaProcesosSeleccion(){
    return this.http.get<ProcesoSeleccion[]>(`${environment.apiUrl}/SelectionProcess/`);
  }

  getProcesoSeleccionById( id: number ){
    return this.http.get<ProcesoSeleccion>(`${environment.apiUrl}/SelectionProcess/${id}`);
  }

  listaActividadesByIdProcesoSeleccion( id: number ){
    return this.http.get<ProcesoSeleccionCronograma[]>(`${environment.apiUrl}/SelectionProcessSchedule/GetListProcesoSeleccionCronogramaByProcesoSeleccionId?pProcesoSeleccionId=${id}`);
  }

  createEditarProcesoSeleccionCronograma( cronograma: ProcesoSeleccionCronograma ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/CreateEditarProcesoSeleccionCronograma`, cronograma );
  }

  createEditarProcesoSeleccionCronogramaMonitoreo( cronograma: ProcesoSeleccionMonitoreo ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcessSchedule/setProcesoSeleccionMonitoreoCronograma`, cronograma );
  }
  deleteProcesoSeleccionCronogramaMonitoreo(pId:number)
  {
    return this.http.delete(`${environment.apiUrl}/SelectionProcess/DeleteProcesoSeleccionCronogramaMonitoreo?pId=${ pId }`);
  }

  deleteProcesoSeleccionIntegranteByID(procesoSeleccionIntegranteId: any) {
    return this.http.delete(`${environment.apiUrl}/SelectionProcess/DeleteProcesoSeleccionIntegrante?pId=${ procesoSeleccionIntegranteId }`);
  }


  listaProcesoSeleccionCronogramaMonitoreo( id: number ){
    return this.http.get<ProcesoSeleccionMonitoreo[]>(`${environment.apiUrl}/SelectionProcessSchedule/GetListProcesoSeleccionMonitoreoCronogramaByProcesoSeleccionId?pProcesoSeleccionId=${id}`);
  }

  deleteProcesoSeleccion( pId: number ){
    return this.http.delete(`${environment.apiUrl}/SelectionProcess/DeleteProcesoSeleccion?pId=${ pId }`);
  }

  changeStateProcesoSeleccion( procesoSeleccion: ProcesoSeleccion ){
    return this.http.put<Respuesta>(`${environment.apiUrl}/SelectionProcess/ChangeStateProcesoSeleccion`, procesoSeleccion);
  }

  createEditarCronogramaSeguimiento( cronograma: CronogramaSeguimiento ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/CreateEditarCronogramaSeguimiento`, cronograma );
  }

  getProcesoSeleccionProponentes(){
    return this.http.get<ProcesoSeleccionProponente[]>(`${environment.apiUrl}/SelectionProcess/getProcesoSeleccionProponentes`);
  }

  setValidateMassiveLoadElegibilidad( archivoParaSubir: File , procesoSeleccionId:number){
    const formData = new FormData(); 
    formData.append('file', archivoParaSubir, archivoParaSubir.name);    
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/setValidateMassiveLoadElegibilidad/${procesoSeleccionId}`, formData);
  }

  uploadMassiveLoadElegibilidad( pId: string,procesoSeleccionId:number ){
    let objeto = { pIdDocument: pId }
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/uploadMassiveLoadElegibilidad?pIdDocument=${ pId }&procesoSeleccionId=${ procesoSeleccionId }`, null);
  }

  createContractorsFromProponent( proceso: ProcesoSeleccion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/createContractorsFromProponent`, proceso);
  }

  deleteProcesoSeleccionCotizacionByID(procesoSeleccionCotizacionId: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/deleteProcesoSeleccionCotizacionByID?procesoSeleccionCotizacionId=${procesoSeleccionCotizacionId}`, null);
  }
  deleteProcesoSeleccionGrupoByID(procesoSeleccionCotizacionId: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/deleteProcesoSeleccionGrupoByID?procesoSeleccionCotizacionId=${procesoSeleccionCotizacionId}`, null);
  }
  deleteProcesoSeleccionActividadesByID(procesoSeleccionCotizacionId: any) {
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/deleteProcesoSeleccionActividadesByID?procesoSeleccionCotizacionId=${procesoSeleccionCotizacionId}`, null);
  }

  getObservacionesByID(id: any) {
    return this.http.get<any[]>(`${environment.apiUrl}/SelectionProcess/getObservacionesProcesoSeleccionProponentes?id=${id}`);
  }
  
  public downloadOrdenElegibilidadFilesByName(name: string) {   
    const retorno = this.http.get(`${environment.apiUrl}/Document/DownloadOrdenElegibilidadFilesByName?pNameFiles=${name}`, { responseType: "blob" });
    return retorno;
  }

}
export interface ProcesoSeleccionMonitoreo{
  procesoSeleccionMonitoreoId:number,
  procesoSeleccionId:number,
  numeroProceso:string,
  estadoActividadCodigo:string,
  fechaCreacion?:string,
  usuarioCreacion?:string,
  eliminado?:boolean,
  enviadoComiteTecnico?:boolean,
  procesoSeleccionCronogramaMonitoreo?:ProcesoSeleccionCronogramaMonitoreo[]
  procesoSeleccion?:ProcesoSeleccion
}

export interface ProcesoSeleccion{
  listaContratistas?: any[];
  procesoSeleccionId?: number,
  numeroProceso?: string,
  objeto?: string,
  alcanceParticular?: string,
  justificacion?: string,
  criteriosSeleccion?: string,
  tipoIntervencionCodigo?: string,
  tipoAlcanceCodigo?: string,
  tipoProcesoCodigo?: string,
  tipoProcesoNombre?: string,
  esDistribucionGrupos?: boolean,
  cantGrupos?: number,

  tipoResponsableTecnicoCodigo?: string,
  tipoResponsableEstructuradorCodigo?: string,

  condicionesJuridicasHabilitantes?: string,
  condicionesFinancierasHabilitantes?: string,
  condicionesTecnicasHabilitantes?: string,
  condicionesAsignacionPuntaje?: string,
  cantidadCotizaciones?: number,
  cantidadProponentes?: number,
  esCompleto?: Boolean,
  estadoProcesoSeleccionCodigo?: string,
  estadoProcesoSeleccionNombre?: string,
  etapaProcesoSeleccionCodigo?: string,
  etapaProcesoSeleccionNombre?: string,
  evaluacionDescripcion?: string,
  urlSoporteEvaluacion?: string,
  tipoOrdenEligibilidadCodigo?: string,
  cantidadProponentesInvitados?: number,

  fechaCreacion?:string,
  urlSoporteProponentesSeleccionados?:string,

  procesoSeleccionGrupo?: ProcesoSeleccionGrupo[],
  procesoSeleccionCronograma?: ProcesoSeleccionCronograma[],
  procesoSeleccionCotizacion?: ProcesoSeleccionCotizacion[],
  procesoSeleccionProponente?: ProcesoSeleccionProponente[],
  procesoSeleccionIntegrante?: ProcesoSeleccionIntegrante[],

}

export interface ProcesoSeleccionGrupo{
  procesoSeleccionGrupoId?: string,
  procesoSeleccionId?: number,
  nombreGrupo?: string,
  tipoPresupuestoCodigo?: string,
  valor?: number,
  valorMinimoCategoria?: number,
  valorMaximoCategoria?: number,
  plazoMeses?: number,
  procesoSeleccion?: ProcesoSeleccion
}

export interface ProcesoSeleccionCronograma{
  
  procesoSeleccionCronogramaId?: number,
  procesoSeleccionId?: number,
  numeroActividad?: number,
  descripcion?: string,
  fechaMaxima?: Date,
  estadoActividadCodigo?: string,
  etapaActualProcesoCodigo?:string,
  procesoSeleccion?: ProcesoSeleccion,
  cronogramaSeguimiento?: CronogramaSeguimiento[];

}

export interface ProcesoSeleccionCronogramaMonitoreo{
  procesoSeleccionCronogramaId?: number,
  procesoSeleccionMonitoreoId?: number,
  numeroActividad?: number,
  descripcion?: string,
  fechaMaxima?: Date,
  estadoActividadCodigo?: string,
  etapaActualProcesoCodigo?:string,
  eliminado?:boolean,
  procesoSeleccionCronogramaMonitoreoId?:number,
  procesoSeleccionMonitoreo?: ProcesoSeleccionMonitoreo
}

export interface ProcesoSeleccionCotizacion {
  procesoSeleccionCotizacionId?: number,
  procesoSeleccionId?: number,
  nombreOrganizacion?: string,
  valorCotizacion?: number,
  descripcion?: string,
  urlSoporte?: string,
  eliminado?:boolean,
  procesoSeleccion?: ProcesoSeleccion,
}

export interface ProcesoSeleccionProponente {
  procesoSeleccionProponenteId?: string,
  procesoSeleccionId?: number,
  tipoProponenteCodigo?: string,
  nombreProponente?: string,
  tipoIdentificacionCodigo?: string,
  numeroIdentificacion?: string,
  localizacionIdMunicipio?: string,
  nombreMunicipio?: string,
  nombreDepartamento?: string, 
  direccionProponente?: string,
  telefonoProponente?: string,
  emailProponente?: string,
  nombreRepresentanteLegal?: string,
  cedulaRepresentanteLegal?: string,
  procesoSeleccion?: ProcesoSeleccion,
  eliminado?: boolean,
  
}

export interface ProcesoSeleccionIntegrante {
  procesoSeleccionIntegranteId?: number,
  procesoSeleccionId?: number,
  porcentajeParticipacion?: number,
  nombreIntegrante?: string,
  procesoSeleccion?: ProcesoSeleccion,

}

export interface CronogramaSeguimiento{
  
  cronogramaSeguimientoId?: number,
  procesoSeleccionCronogramaId?: number,
  estadoActividadInicialCodigo?: string,
  estadoActividadFinalCodigo?: string,
  observacion?: string,
  fechaCreacion?: Date,
  procesoSeleccionCronograma?: ProcesoSeleccionCronograma

}

interface TipoProcesoSeleccion{
  Privada: string;
  Cerrada: string;
  Abierta: string;
}

export const TiposProcesoSeleccion: TipoProcesoSeleccion = {
  Privada:  "1",
  Cerrada:  "2",
  Abierta:  "3"
}

interface EstadoProcesoSeleccion{
  Creado: string,
  AperturaEntramite: string,
  AprobadaAperturaPorComiteTecnico: string,
  AprobadaAperturaPorComiteFiduciario: string,
  RechazadaAperturaPorComiteTecnico: string,
  RechazadaAperturaPorComiteFiduciario: string,
  DevueltaAperturaPorComiteTecnico: string,
  DevueltaAperturaPorComiteFiduciario: string,
  EnProcesoDeSeleccion: string,
	AprobacionDeSeleccionEnTramite: string,
	AprobadaSelecci??nPorComiteTecnico: string,
	AprobadaSelecci??nPorComiteFiduciario: string,
	RechazadaSeleccionPorComiteTecnico: string,
	RechazadaSeleccionPorComiteFiduciario: string,
	DevueltaSeleccionPorComiteTecnico: string,
	DevueltaSeleccionPorComiteFiduciario: string,
	Cerrado: string,
	AprobadoPorComiteTecnico: string,
	AprobadoPorComiteFiduciario: string,
	RechazadoPorComiteTecnico: string,
	RechazadoPorComiteFiduciario: string,
	DevueltoPorComiteTecnico: string,
	DevueltoPorComiteFiduciario: string,
}

export const EstadosProcesoSeleccion: EstadoProcesoSeleccion = {
  Creado: "1",
  AperturaEntramite: "2",
  AprobadaAperturaPorComiteTecnico: "3",
  AprobadaAperturaPorComiteFiduciario: "4",
  RechazadaAperturaPorComiteTecnico: "5",
  RechazadaAperturaPorComiteFiduciario: "6",
  DevueltaAperturaPorComiteTecnico: "7",
  DevueltaAperturaPorComiteFiduciario: "8",
  EnProcesoDeSeleccion: "9",
  AprobacionDeSeleccionEnTramite: "10",
  AprobadaSelecci??nPorComiteTecnico: "11",
  AprobadaSelecci??nPorComiteFiduciario: "12",
  RechazadaSeleccionPorComiteTecnico: "13",
  RechazadaSeleccionPorComiteFiduciario: "14",
  DevueltaSeleccionPorComiteTecnico: "15",
  DevueltaSeleccionPorComiteFiduciario: "16",
  Cerrado: "17",
  AprobadoPorComiteTecnico: "18",
  AprobadoPorComiteFiduciario: "19",
  RechazadoPorComiteTecnico: "20",
  RechazadoPorComiteFiduciario: "21",
  DevueltoPorComiteTecnico: "22",
  DevueltoPorComiteFiduciario: "23",
}
interface EstadoProcesoSeleccionMonitoreo{
  Creada: string,
	AprobadoPorComiteTecnico: string,
	AprobadoPorComiteFiduciario: string,
	RechazadoPorComiteTecnico: string,
	RechazadoPorComiteFiduciario: string,
	DevueltoPorComiteTecnico: string,
	DevueltoPorComiteFiduciario: string,
  EnTramite: string,
  
}
export const EstadosProcesoSeleccionMonitoreo: EstadoProcesoSeleccionMonitoreo = {
  Creada: "1",  
  AprobadoPorComiteTecnico: "2",
  AprobadoPorComiteFiduciario: "3",
  RechazadoPorComiteTecnico: "4",
  RechazadoPorComiteFiduciario: "5",
  DevueltoPorComiteTecnico: "6",
  DevueltoPorComiteFiduciario: "7",
  EnTramite: "8",

}
