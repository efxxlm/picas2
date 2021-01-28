import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalleeditar-actuacion-notai',
  templateUrl: './ver-detalleeditar-actuacion-notai.component.html',
  styleUrls: ['./ver-detalleeditar-actuacion-notai.component.scss']
})
export class VerDetalleeditarActuacionNotaiComponent implements OnInit {
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  idControversia: any;
  idActuacion: any;
  tipoControversia: string;
  fechaSolicitud: any;
  codigoSolicitud: any;
  numeroContrato: any;

  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idActuacion = param.id;
    });
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
