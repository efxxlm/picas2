import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SeguimientoDiario } from 'src/app/_interfaces/DailyFollowUp';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class FollowUpDailyService {

  private url: string = `${ environment.apiUrl }/DailyFollowUp`;

  constructor( private http: HttpClient ) { }

  gridRegisterDailyFollowUp( ) {
    return this.http.get<any[]>( `${ this.url }/gridRegisterDailyFollowUp`);
  };

  createEditDailyFollowUp( seguimiento: SeguimientoDiario ) {
    return this.http.post<Respuesta>( `${ this.url }/createEditDailyFollowUp`, seguimiento);
  };

  
  getDailyFollowUpById( id: number ) {
    return this.http.get<SeguimientoDiario>( `${ this.url }/getDailyFollowUpById?pId=${ id }`);
  };

  getDailyFollowUpByContratacionProyectoId( id: number ) {
    return this.http.get<SeguimientoDiario[]>( `${ this.url }/getDailyFollowUpByContratacionProyectoId?pId=${ id }`);
  };
  

}
