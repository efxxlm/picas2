import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Respuesta } from '../common/common.service'
import { environment } from 'src/environments/environment';
import { pid } from 'process';

@Injectable({
  providedIn: 'root'
})
export class CuentaBancariaService {

  constructor( private http: HttpClient ) { }

  GetBankAccountById(pId: number){
    return this.http.get<Respuesta>(`${environment.apiUrl}/BankAccount/${pid}`);
  }

}