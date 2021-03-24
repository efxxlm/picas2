import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-verdetalle-reclamacion-aseg-cc',
  templateUrl: './verdetalle-reclamacion-aseg-cc.component.html',
  styleUrls: ['./verdetalle-reclamacion-aseg-cc.component.scss']
})
export class VerdetalleReclamacionAsegCcComponent implements OnInit {
  controversiaId: any;
  idActuacion: any;
  reclamacionCod: any;
  actuacion: any;
  numActuacion: any;
  resumen: any;
  requiereReclamacionComite: string;
  soporteReclamacion: any;
  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.controversiaId = param.idControversia;
      this.idActuacion = param.id;
      this.loadData(this.idActuacion);
    });
  }
  loadData(id){
    this.services.GetControversiaActuacionById(id).subscribe((a:any)=>{
      this.reclamacionCod = a.numeroActuacionReclamacion;
      this.actuacion = a.actuacionAdelantadaString;
      this.numActuacion = a.numeroActuacionFormat;
      this.resumen = a.resumenPropuestaFiduciaria;
      switch(a.esRequiereComiteReclamacion){
        case false:
        this.requiereReclamacionComite = 'No';
        break;
      case true:
        this.requiereReclamacionComite = 'Sí';
        break;
      }
      this.soporteReclamacion = a.rutaSoporte;
    });
  }

}
