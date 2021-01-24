import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalleeditar-mesa-de-trabajo',
  templateUrl: './ver-detalleeditar-mesa-de-trabajo.component.html',
  styleUrls: ['./ver-detalleeditar-mesa-de-trabajo.component.scss']
})
export class VerDetalleeditarMesaDeTrabajoComponent implements OnInit {
  idControversia: any;
  solictud: any;
  numContrato: any;
  tipoControversia: any;
  constructor(private services: ContractualControversyService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
      this.loadBasicData(this.idControversia);
    });
  }
  loadBasicData(id){
    this.services.GetControversiaActuacionById(id).subscribe((a:any)=>{
      this.services.GetControversiaContractualById(a.controversiaContractualId).subscribe((b:any)=>{
        this.solictud = b.numeroSolicitud;
        this.numContrato = b.contrato.numeroContrato;
        switch (b.tipoControversiaCodigo){
          case '2':
            this.tipoControversia = "Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratista";
          break;
          case '3':
            this.tipoControversia = "Arreglo Directo (AD) a solicitud del contratista";
          break;
          case '4':
            this.tipoControversia = "Otras controversias contractuales (OCC) a solicitud del contratista";
          break;
          case '5':
            this.tipoControversia = "Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratante";
          break;
          case '6':
            this.tipoControversia = "Arreglo Directo (AD) a solicitud del contratante";
          break;
          case '7':
            this.tipoControversia = "Otras controversias contractuales (OCC) a solicitud del contratante";
          break;
        }
      });
    });
  }
}
