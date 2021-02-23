import { Component, OnInit } from '@angular/core';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-actualizar-tramite-contvr-contrc',
  templateUrl: './actualizar-tramite-contvr-contrc.component.html',
  styleUrls: ['./actualizar-tramite-contvr-contrc.component.scss']
})
export class ActualizarTramiteContvrContrcComponent implements OnInit {
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  public selTab;
  public tipoControversia;
  public fechaSolicitud;
  public codigoSolicitud;
  public numeroContrato;
  opcion1 = false;
  opcion2 = false;
  tieneReclamaciones: any[] = [];
  tieneMesasTrabajo: any[] = [];
  constructor(private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.loadDataContrato(this.controversiaID);
  }
  cambiarTab(opc) {
    this.selTab=opc;
  }
  loadDataContrato(id){
    this.services.GetControversiaContractualById(id).subscribe((data:any)=>{
      switch(data.tipoControversiaCodigo){
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
      this.fechaSolicitud = data.fechaSolicitud;
      this.codigoSolicitud = data.numeroSolicitud;
      this.numeroContrato = data.contrato.numeroContrato;
    });
    this.services.GetListGrillaControversiaActuacion(id).subscribe((data0:any)=>{
      //cuando hay reclamaciones
      for(let estado of data0){
        if(estado.estadoAvanceTramiteCodigo=='14'){
          this.tieneReclamaciones.push(estado);
        }
      }
      //cuando hay mesas de trabajo
      for(let estadoMT of data0){
        if(estadoMT.requiereMesaTrabajo==true && estadoMT.estadoActuacionCodigo=='2'){
          this.tieneMesasTrabajo.push(estadoMT);
          console.log(this.tieneMesasTrabajo);
        }
      }
    })
  }
}
