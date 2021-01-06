import { map } from 'rxjs/operators';
import { Dominio, Respuesta } from './../common/common.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VerificarAvanceSemanalService {

    private urlApi = `${ environment.apiUrl }/CheckWeeklyProgress`;

    constructor( private http: HttpClient ) { }
    
    seguimientoSemanalObservacion( pSeguimientoSemanalObservacion: any ) {
        return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditSeguimientoSemanalObservacion`, pSeguimientoSemanalObservacion );
    }

    tipoObservaciones() {
        return this.http.get<Dominio[]>( `${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=123` )
            .pipe(
                map(
                    response => {
                        const tipoObservacion: any = {
                            gestionAmbiental: {},
                            gestionCalidad: {},
                            reporteActividades: {}
                        };
                        for ( const dominio of response ) {
                            if ( dominio.codigo === '1' ) {
                                tipoObservacion.avanceFisico = dominio.codigo;
                            }
                            if ( dominio.codigo === '2' ) {
                                tipoObservacion.avanceFinanciero = dominio.codigo;
                            }
                            if ( dominio.codigo === '3' ) {
                                tipoObservacion.gestionObra = dominio.codigo;
                            }
                            if ( dominio.codigo === '4' ) {
                                tipoObservacion.gestionAmbiental.gestionAmbientalCodigo = dominio.codigo;
                            }
                            if ( dominio.codigo === '5' ) {
                                tipoObservacion.gestionAmbiental.manejoMateriales = dominio.codigo;
                            }
                            if ( dominio.codigo === '6' ) {
                                tipoObservacion.gestionAmbiental.residuosConstruccion = dominio.codigo;
                            }
                            if ( dominio.codigo === '7' ) {
                                tipoObservacion.gestionAmbiental.residuosPeligrosos = dominio.codigo;
                            }
                            if ( dominio.codigo === '8' ) {
                                tipoObservacion.gestionAmbiental.manejoOtra = dominio.codigo;
                            }
                            if ( dominio.codigo === '9' ) {
                                tipoObservacion.gestionCalidad.gestionCalidadCodigo = dominio.codigo;
                            }
                            if ( dominio.codigo === '10' ) {
                                tipoObservacion.gestionCalidad.ensayosLaboratorio = dominio.codigo;
                            }
                            if ( dominio.codigo === '11' ) {
                                tipoObservacion.gestionCalidad.muestrasEnsayo = dominio.codigo;
                            }
                            if ( dominio.codigo === '12' ) {
                                tipoObservacion.gestionSst = dominio.codigo;
                            }
                            if ( dominio.codigo === '13' ) {
                                tipoObservacion.gestionSocial = dominio.codigo;
                            }
                            if ( dominio.codigo === '14' ) {
                                tipoObservacion.alertasRelevantes = dominio.codigo;
                            }
                            if ( dominio.codigo === '15' ) {
                                tipoObservacion.reporteActividades.reporteActividadesCodigo = dominio.codigo;
                            }
                            if ( dominio.codigo === '16' ) {
                                tipoObservacion.reporteActividades.actividadEstadoObra = dominio.codigo;
                            }
                            if ( dominio.codigo === '17' ) {
                                tipoObservacion.reporteActividades.actividadRealizada = dominio.codigo;
                            }
                            if ( dominio.codigo === '18' ) {
                                tipoObservacion.reporteActividades.actividadRealizadaSiguiente = dominio.codigo;
                            }
                            if ( dominio.codigo === '19' ) {
                                tipoObservacion.registroFotografico = dominio.codigo;
                            }
                            if ( dominio.codigo === '20' ) {
                                tipoObservacion.comiteObra = dominio.codigo;
                            }
                        }
                        return tipoObservacion;
                    }
                )
            );
    }

}
