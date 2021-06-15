import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { CarguePago, CarguePagosRendimientos } from '../../../_interfaces/faseDosPagosRendimientos'
import { Respuesta } from '../common/common.service'

@Injectable({
  providedIn: 'root'
})
export class FaseDosPagosRendimientosService {
  private urlApi0: string = `${environment.apiUrl}`
  private urlApi: string = `${environment.apiUrl}/RegisterPayPerformance`
  private urlPayment = 'RegisterPayment'; 
  private urlPerformance = 'RegisterPerformance'
  constructor(private http: HttpClient) {}


  validateUploadPaymentRegisterFile(
    documento: File,
    saveSuccessProcess: boolean
  ) {
    const formData = new FormData()
    formData.append('file', documento, documento.name)
    return this.http.post(
      `${this.urlApi0}/${this.urlPayment}/file?saveSuccessProcess=${saveSuccessProcess}`,
      formData
    )
  }

  getPayments(status: string = "") {
    return this.http.get<CarguePago[]>(
      `${this.urlApi0}/${this.urlPayment}/payments?status=${status}`
    )
  }

  setObservationPayments(data: any) {
    return this.http.post(
      `${this.urlApi0}/${this.urlPayment}/observations`,
      data
    )
  }

  deletePayment(uploadedOrderId: number){
    return this.http.put<any>(`${this.urlApi0}/${this.urlPayment}/${uploadedOrderId}`, {})
  }

  downloadPayments(uploadedOrderId: number){ 
    return this.http.get(`${this.urlApi0}/${this.urlPayment}/file/${uploadedOrderId}`,  { responseType: "blob" })
  }


/*** Performances Methods */

  getPaymentsPerformances(typeFile: string, status: string = "") {
    return this.http.get<CarguePagosRendimientos[]>(
      `${this.urlApi}/getPaymentsPerformances?typeFile=${typeFile}&&status=${status}`
    )
  }
 
  downloadPaymentsPerformanceStatus(fileRequest: any, fileType: string){
    return this.http.post(`${this.urlApi}/downloadPaymentPerformance?fileType=${fileType}`, fileRequest , { responseType: "blob" })
  }

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

  managePerformance(uploadedOrderId :number){
    return this.http.get<any>(`${this.urlApi}/managePerformance?uploadedOrderId=${uploadedOrderId}`)
  }

  sendInconsistencies(uploadedOrderId :number){
    return this.http.get<any>(`${this.urlApi}/sendInconsistencies?uploadedOrderId=${uploadedOrderId}`)
  }

  requestApproval(uploadedOrderId :number){
    return this.http.get<any>(`${this.urlApi}/requestApproval?uploadedOrderId=${uploadedOrderId}`)
  }

  downloadManagedPerformances(uploadedOrderId :number, queryConsistentOrders?: boolean){
    return this.http.post(
      `${this.urlApi}/downloadManagedPerformances?uploadedOrderId=${uploadedOrderId}&queryConsistentOrders=${queryConsistentOrders}` , {} , {responseType: "blob" })
  }

  downloadPerformancesInconsistencies(uploadedOrderId :number){
    return this.http.post(
      `${this.urlApi}/downloadPerformancesInconsistencies?uploadedOrderId=${uploadedOrderId}&status=${1}`, {} , {responseType: "blob" })
  }

  getRequestedApprovalPerformances() {
    return this.http.get<CarguePagosRendimientos[]>(
      `${this.urlApi}/requestedApprovalPerformances`
    )
  }

  includePerformances(uploadedOrderId :number){
    return this.http.post(`${this.urlApi}/includePerformances?uploadedOrderId=${uploadedOrderId}`, {});
  }

  downloadApprovedIncorporatedPerformances(uploadedOrderId :number){
    return this.http.post(
      `${this.urlApi}/ApprovedIncorporatedPerformances?uploadedOrderId=${uploadedOrderId}`, {} , {responseType: "blob" })
  }

  generateMinute(uploadedOrderId :number){
    return this.http.post(
      `${this.urlApi}/PerformanceMinute?uploadedOrderId=${uploadedOrderId}`, {} , {responseType: "blob" });
  }
  uploadPerformanceUrlMinute(minuteRequest: any){
    return this.http.post<Respuesta>(`${this.urlApi}/uploadMinutes?uploadedOrderId=${minuteRequest.resourceId}`, minuteRequest);
  }

  downloadPerformanceUrlMinute(uploadedOrderId :number){
    return this.http.post(`${this.urlApi}/urlminute?uploadedOrderId=${uploadedOrderId}`, {} , {responseType: "blob" })
  }
  
}
