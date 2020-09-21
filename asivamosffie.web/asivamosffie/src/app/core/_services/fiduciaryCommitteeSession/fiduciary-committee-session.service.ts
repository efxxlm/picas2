import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SolicitudesContractuales } from 'src/app/_interfaces/technicalCommitteSession';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FiduciaryCommitteeSessionService {

  constructor(
                private http: HttpClient,

  ) 
  { }

  getCommitteeSessionFiduciario(){
    return this.http.get<SolicitudesContractuales[]>(`${environment.apiUrl}/CommitteeSessionFiduciario/getCommitteeSessionFiduciario`);
  }

}
