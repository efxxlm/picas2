import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { environment } from 'src/environments/environment';
import { Respuesta } from '../common/common.service';

@Injectable({
  providedIn: 'root'
})
export class ContractualNoveltyService {

  private urlApi = `${ environment.apiUrl }/RegisterWeeklyProgress`;

  constructor(private http: HttpClient) { }

  createEditNovedadContractual( novedadContractual: NovedadContractual ) {
    return this.http.post<Respuesta>( `${ this.urlApi }/createEditNovedadContractual`, novedadContractual );
}
}
