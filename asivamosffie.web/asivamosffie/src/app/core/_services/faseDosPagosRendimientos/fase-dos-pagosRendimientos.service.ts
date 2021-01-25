import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { CarguePagosRendimientos } from '../../../_interfaces/faseDosPagosRendimientos'
import exportFromJSON from 'export-from-json'

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

  getPaymentsPerformances(typeFile: string, status: string = "") {
    return this.http.get<CarguePagosRendimientos[]>(
      `${this.urlApi}/getPaymentsPerformances?typeFile=${typeFile}&&status=${status}`
    )
  }

  setObservationPaymentsPerformances(data: any) {
    console.log(data)
    return this.http.post(
      `${this.urlApi}/setObservationPaymentsPerformances`,
      data
    )
  }

  deletePaymentsPerformanceStatus(uploadedOrderId: number){
    return this.http.get<any>(`${this.urlApi}/deletePaymentPerformance?uploadedOrderId=${uploadedOrderId}`)
  }

  downlaodPaymentsPerformanceStatus(uploadedOrderId: number){
    return this.http.get<any>(`${this.urlApi}/downloadPaymentPerformance?uploadedOrderId=${uploadedOrderId}`)
  }

  managePerformance(uploadedOrderId :number){
    return this.http.get<any>(`${this.urlApi}/managePerformance?uploadedOrderId=${uploadedOrderId}`)
  }

  sendInconsistencies(uploadedOrderId :number){
    return this.http.get<any>(`${this.urlApi}/sendInconsistencies?uploadedOrderId=${uploadedOrderId}`)
  }

  requestApproval(uploadedOrderId :number){
    return this.http.get<any>(`${this.urlApi}/requestApproval?uploadedOrderId=${uploadedOrderId}`)
  }

  downloadManagedPerformances(uploadedOrderId :number, statusType: number){
    return this.http.get<any>(`${this.urlApi}/downloadManagedPerformances?uploadedOrderId=${uploadedOrderId}&status=${statusType}`)
  }
}
