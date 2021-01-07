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
  
  getDatesAvailableByContratacioProyectoId( id ){
    return this.http.get<string[]>( `${ this.url }/getDatesAvailableByContratacioProyectoId?pId=${ id }`);
  } 

  deleteDailyFollowUp( id ){
    return this.http.delete<Respuesta>( `${ this.url }/deleteDailyFollowUp?pId=${ id }`);
  }

  sendToSupervisionSupport( id ){
    return this.http.put<Respuesta>( `${ this.url }/sendToSupervisionSupport?pId=${ id }`, null);
  }

  gridVerifyDailyFollowUp(){
    return this.http.get<any[]>( `${ this.url }/gridVerifyDailyFollowUp`);
  };

  gridValidateDailyFollowUp(){
    return this.http.get<any[]>( `${ this.url }/gridValidateDailyFollowUp`);
  };

  createEditObservacion( seguimiento: SeguimientoDiario, esSupervisor: boolean ){
    return this.http.post<Respuesta>( `${ this.url }/createEditObservacion?esSupervisor=${ esSupervisor }`, seguimiento);
  };

  sendToSupervision( id ){
    return this.http.put<Respuesta>( `${ this.url }/SendToSupervision?pId=${ id }`, null);
  }

  approveDailyFollowUp( id ){
    return this.http.put<Respuesta>( `${ this.url }/approveDailyFollowUp?pId=${ id }`, null);
  }

  returnToComptroller( id ){
    return this.http.put<Respuesta>( `${ this.url }/returnToComptroller?pId=${ id }`, null);
  }

}
