import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidacionesLineaPagoService {

    private urlApi = `${ environment.apiUrl }`;

    constructor( private http: HttpClient ) { }

    validacion_controller = 'ValidacionesLineaPagos';

    validacionFacturadosODG(){
      return this.http.get<any>(`${ this.urlApi }/${this.validacion_controller}/ValidacionFacturadosODG`);
    }
}
