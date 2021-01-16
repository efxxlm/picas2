import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-registrar-nueva-mesatrabajo-act',
  templateUrl: './registrar-nueva-mesatrabajo-act.component.html',
  styleUrls: ['./registrar-nueva-mesatrabajo-act.component.scss']
})
export class RegistrarNuevaMesatrabajoActComponent implements OnInit {
  idControversia: any;
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  tipoControversia: string;
  fechaSolicitud: any;
  codigoSolicitud: any;
  numeroContrato: any;
  constructor(private services: ContractualControversyService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
    this.services.GetControversiaContractualById(this.controversiaID).subscribe((a:any)=>{
      switch(a.tipoControversiaCodigo){
        case '1':
          this.tipoControversia = 'Terminación anticipada por incumplimiento (TAI)';
        break;
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
      this.fechaSolicitud = a.fechaSolicitud;
      this.codigoSolicitud = a.numeroSolicitud;
      this.numeroContrato = a.contrato.numeroContrato;
    });
  }

}
