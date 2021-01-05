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
                        const tipoObservacion: any = {};
                        console.log( response );
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
                        }
                        return tipoObservacion;
                    }
                )
            );
    }

}
