import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { CarguePagosRendimientos } from '../../../_interfaces/faseDosPagosRendimientos'
@Injectable({
  providedIn: 'root'
})
export class FaseDosPagosRendimientosService {
  private urlApi: string = `${environment.apiUrl}/RegisterPayPerformance`

  constructor(private http: HttpClient) {}

  uploadFileToValidate(
    documento: File,
    typeFile: string,
    saveSuccessProcess: boolean
  ) {
    const formData = new FormData()
    formData.append('file', documento, documento.name)
    return this.http.post(
      `${this.urlApi}/uploadFileToValidate?typeFile=${typeFile}&saveSuccessProcess=${saveSuccessProcess}`,
      formData
    )
  }

  getPaymentsPerformances(typeFile: string) {
    return this.http.get<CarguePagosRendimientos[]>(
      `${this.urlApi}/getPaymentsPerformances?typeFile=${typeFile}`
    )
  }

  setObservationPaymentsPerformances(data: any) {
    console.log(data)
    return this.http.post(
      `${this.urlApi}/setObservationPaymentsPerformances`,
      data
    )
  }

  setPaymentsPerformanceStatus(uploadedPaymentPerformanceId){
    return this.http.get(`${this.urlApi}/deletePaymentPerformance`, uploadedPaymentPerformanceId )
  }
}
