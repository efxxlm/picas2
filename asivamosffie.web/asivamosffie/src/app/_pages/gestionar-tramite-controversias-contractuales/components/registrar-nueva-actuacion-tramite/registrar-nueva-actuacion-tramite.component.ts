import { Component, OnInit } from '@angular/core';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-registrar-nueva-actuacion-tramite',
  templateUrl: './registrar-nueva-actuacion-tramite.component.html',
  styleUrls: ['./registrar-nueva-actuacion-tramite.component.scss']
})
export class RegistrarNuevaActuacionTramiteComponent implements OnInit {
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
        case '1':
          this.tipoControversia = 'Terminación anticipada por incumplimiento (TAI)';
        break;
      };
      this.fechaSolicitud = data.fechaSolicitud;
      this.codigoSolicitud = data.numeroSolicitud;
      this.numeroContrato = data.contrato.numeroContrato;
    });
  }
}
