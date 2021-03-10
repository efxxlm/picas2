import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-ver-detalleditar-cntrv-contrc',
  templateUrl: './ver-detalleditar-cntrv-contrc.component.html',
  styleUrls: ['./ver-detalleditar-cntrv-contrc.component.scss']
})
export class VerDetalleditarCntrvContrcComponent implements OnInit {
  idControversia: any;
  idContrato: any;
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
    private services: ContractualControversyService,
    private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
      this.loadData(param.id);
    });
  }

  loadData(id) {
    this.services.GetControversiaContractualById(id).subscribe((resp:any)=>{
      this.numeroSolicitud = resp.numeroSolicitud;
      this.polizaService.GetListVistaContratoGarantiaPoliza(resp.contratoId).subscribe(resp_0=>{
        this.nombreContratista = resp_0[0].nombreContratista;
        this.tipoIdentificacion = resp_0[0].tipoDocumento;
        this.numIdentificacion = resp_0[0].numeroIdentificacion;
        this.valorContrato = resp_0[0].valorContrato;
        //this.plazoContrato = resp_0[0].plazoContrato;
      });
      this.services.GetVistaContratoContratista(resp.contratoId).subscribe((resp_1:any)=>{
        this.numContrato = resp_1.numeroContrato;
        this.tipoIntervencion = resp_1.tipoIntervencion;
        this.fechaInicioContrato = resp_1.fechaInicioContrato;
        this.fechaFinalizacionContrato = resp_1.fechaFinContrato;
        this.plazoContrato = resp_1.plazoFormat;
      });
    });
  }


}
