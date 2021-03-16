import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class ContractualNoveltyService {

  private urlApi = `${ environment.apiUrl }/ContractualNovelty`;

  constructor(private http: HttpClient) { }

  createEditNovedadContractual( novedadContractual: NovedadContractual ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditNovedadContractual`, novedadContractual );
  }

  getContratosAutocomplete()
  {
    return this.http.get<Contrato[]>( `${ this.urlApi }/GetListContract` ); 
  }
  
  postRegistroNovedadContractual( pNContrato: FormData) {
    return this.http.post<Respuesta>( `${ this.urlApi }/CreateEditarModification`, pNContrato );
  }
  
  getListGrillaNovedadContractual()
  {
    return this.http.get<any[]>( `${ this.urlApi }/GetListGrillaNovedadContractual` ); 
  }

  getProyectosContrato(ncontrato) {
    return this.http.get<any[]>( `${ this.urlApi }/GetProyectsByContract?pContratoId=${ncontrato}` ); 
  }

  eliminarNovedadContractual(id) {
    return this.http.delete<Respuesta>( `${ this.urlApi }/eliminarNovedadContractual?pNovedaContractual=${ id }` ); 
  }

}
