import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Respuesta } from '../autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private http: HttpClient) {  }

  public uploadFileProject(archivoParaSubir: File) {
    const formData = new FormData(); 
    formData.append('file', archivoParaSubir, archivoParaSubir.name);
    //si se requieren mas variables
    //formData.append('proyectId',projectid);
    const retorno = this.http.post<Respuesta>(`${environment.apiUrl}/Project/SetValidateMassiveLoadProjects`,formData);
    return retorno;
  }

  public uploadOkProjectsFileProject(idProject: string) {   
    const retorno = this.http.post<Respuesta>(`${environment.apiUrl}/Project/UploadMassiveLoadProjects?pIdDocument=${idProject}`,null);
    return retorno;
  }
  
}
export interface RespuestaProyecto{
  cantidadDeRegistros: number,
  cantidadDeRegistrosInvalidos: number,
  cantidadDeRegistrosValidos: number,
  llaveConsulta: string
}