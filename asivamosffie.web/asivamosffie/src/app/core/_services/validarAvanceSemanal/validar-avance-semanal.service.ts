import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ValidarAvanceSemanalService {

    private urlApi = `${ environment.apiUrl }/ValidateWeeklyProgress`;

    constructor( private http: HttpClient ) { }

    getListReporteSemanalView() {
        return this.http.get<any[]>( `${ this.urlApi }/GetListReporteSemanalView` );
    }
}
