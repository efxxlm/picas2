import { Component, OnInit } from '@angular/core';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-registrar-nueva-actuacion-tram-no-tai',
  templateUrl: './registrar-nueva-actuacion-tram-no-tai.component.html',
  styleUrls: ['./registrar-nueva-actuacion-tram-no-tai.component.scss']
})
export class RegistrarNuevaActuacionTramNoTaiComponent implements OnInit {
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  public tipoControversia;
  public fechaSolicitud;
  public codigoSolicitud;
  public numeroContrato;
  constructor(private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.loadDataContrato(this.controversiaID);
  }
  loadDataContrato(id){
    this.services.GetControversiaContractualById(id).subscribe((data:any)=>{
      switch(data.tipoControversiaCodigo){
        case '2':
          this.tipoControversia = 'Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratista';
        break;
        case '3':
          this.tipoControversia = 'Arreglo Directo (AD) a solicitud del contratista';
        break;
        case '4':
          this.tipoControversia = 'Otras controversias contractuales (OCC) a solicitud del contratista';
        break;
        case '5':
          this.tipoControversia = 'Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratante';
        break;
        case '6':
          this.tipoControversia = 'Arreglo Directo (AD) a solicitud del contratante';
        break;
        case '7':
          this.tipoControversia = 'Otras controversias contractuales (OCC) a solicitud del contratante';
        break;
      };
      this.fechaSolicitud = data.fechaSolicitud;
      this.codigoSolicitud = data.numeroSolicitud;
      this.numeroContrato = data.contrato.numeroContrato;
    });
  }
}
