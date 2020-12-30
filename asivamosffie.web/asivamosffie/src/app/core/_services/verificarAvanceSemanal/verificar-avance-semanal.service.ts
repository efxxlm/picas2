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
        return this.http.get<Dominio[]>( `${environment.apiUrl}/Common/dominioByIdDominio?pIdDominio=112` )
            .pipe(
                map(
                    response => {
                        console.log( response );
                    }
                )
            );
    }

}
