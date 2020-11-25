import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalleditar-cntrv-contrc',
  templateUrl: './ver-detalleditar-cntrv-contrc.component.html',
  styleUrls: ['./ver-detalleditar-cntrv-contrc.component.scss']
})
export class VerDetalleditarCntrvContrcComponent implements OnInit {
  idControversia: any;
  numContrato: any;
  nombreContratista: any;
  tipoIdentificacion: string;
  tipoIntervencion: string;
  numIdentificacion: string;
  valorContrato: string;
  plazoContrato: any;
  fechaInicioContrato: any;
  fechaFinalizacionContrato: any;
  numeroSolicitud: any;
  estadoAcordeon: string;
  estadoAcordeon1: string;  

  constructor(private activatedRoute: ActivatedRoute,
    private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
      this.loadData(param.id);
    });
  }

  loadData(id) {
    this.services.GetControversiaContractualById(id).subscribe((resp:any)=>{
      this.numeroSolicitud = resp.numeroSolicitudFormat;
      this.services.GetVistaContratoContratista(resp.contratoId).subscribe(resp_1=>{
        this.numContrato = resp_1.numeroContrato;
        this.nombreContratista = resp_1.nombreContratista;
        this.tipoIdentificacion = 'Pendiente de lectura del servicio';
        this.numIdentificacion = 'Pendiente de lectura del servicio';
        this.tipoIntervencion = 'Pendiente de lectura del servicio';
        this.valorContrato = 'Pendiente de lectura del servicio';
        this.plazoContrato = resp_1.plazoFormat;
        this.fechaInicioContrato = resp_1.fechaInicioContrato;
        this.fechaFinalizacionContrato = resp_1.fechaFinContrato;
      });
    });
  }
}
