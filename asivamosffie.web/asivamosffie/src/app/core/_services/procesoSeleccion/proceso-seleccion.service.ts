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

  setValidateMassiveLoadElegibilidad( archivoParaSubir: File ){
    const formData = new FormData(); 
    formData.append('file', archivoParaSubir, archivoParaSubir.name);
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/setValidateMassiveLoadElegibilidad`, formData);
  }

  uploadMassiveLoadElegibilidad( pId: string ){
    let objeto = { pIdDocument: pId }
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/uploadMassiveLoadElegibilidad?pIdDocument=${ pId }`, null);
  }

  createContractorsFromProponent( proceso: ProcesoSeleccion ){
    return this.http.post<Respuesta>(`${environment.apiUrl}/SelectionProcess/createContractorsFromProponent`, proceso);
  }
  
}

export interface ProcesoSeleccion{
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
  responsableTecnicoUsuarioId?: number,
  responsableEstructuradorUsuarioid?: number,

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
  procesoSeleccion?: ProcesoSeleccion
}

export interface ProcesoSeleccionCotizacion {
  procesoSeleccionCotizacionId?: number,
  procesoSeleccionId?: number,
  nombreOrganizacion?: string,
  valorCotizacion?: number,
  descripcion?: string,
  urlSoporte?: string,
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
  direccionProponente?: string,
  telefonoProponente?: string,
  emailProponente?: string,
  nombreRepresentanteLegal?: string,
  cedulaRepresentanteLegal?: string,
  procesoSeleccion?: ProcesoSeleccion,
  
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
  Creado: string;
  AperturaEntramite: string;
  DevueltaAperturaPorComiteTecnico: string;
  EnProcesoDeSeleccion: string;
  RechazadaSeleccionPorComiteTecnico: string;      
}

export const EstadosProcesoSeleccion: EstadoProcesoSeleccion = {
  Creado: "1",
  AperturaEntramite: "2",
  DevueltaAperturaPorComiteTecnico: "3",
  EnProcesoDeSeleccion: "4",
  RechazadaSeleccionPorComiteTecnico: "5",
}

