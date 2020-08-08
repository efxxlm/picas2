import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../autenticacion/autenticacion.service';
import { environment } from 'src/environments/environment';

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