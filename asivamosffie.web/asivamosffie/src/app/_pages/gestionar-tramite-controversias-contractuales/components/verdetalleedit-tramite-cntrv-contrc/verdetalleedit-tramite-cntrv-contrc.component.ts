import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-verdetalleedit-tramite-cntrv-contrc',
  templateUrl: './verdetalleedit-tramite-cntrv-contrc.component.html',
  styleUrls: ['./verdetalleedit-tramite-cntrv-contrc.component.scss']
})
export class VerdetalleeditTramiteCntrvContrcComponent implements OnInit {
  idActuacion: any;
  public controversiaID;
  public tipoControversia;
  public fechaSolicitud;
  public codigoSolicitud;
  public numeroContrato;
  numeroActuacionFormat: any;
  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.controversiaID = param.idControversia;
      this.idActuacion = param.id;
      this.loadDataContrato(param.idControversia);
      this.loadActuacionCodeID(param.id);
    });
  } 
  loadDataContrato(id){
    this.services.GetControversiaContractualById(id).subscribe((data:any)=>{
      switch(data.tipoControversiaCodigo){
        case '1':
          this.tipoControversia = 'Terminación anticipada por incumplimiento (TAI)';
        break;
      };
      this.fechaSolicitud = data.fechaSolicitud;
      this.codigoSolicitud = data.numeroSolicitud;
      this.numeroContrato = data.contrato.numeroContrato;
    });
  }
  loadActuacionCodeID(id){
    this.services.GetControversiaActuacionById(id).subscribe((data:any)=>{
      this.numeroActuacionFormat=data.numeroActuacionFormat;
    });
  }
}
