import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalle-actuacion-contr-contrct',
  templateUrl: './ver-detalle-actuacion-contr-contrct.component.html',
  styleUrls: ['./ver-detalle-actuacion-contr-contrct.component.scss']
})
export class VerDetalleActuacionContrContrctComponent implements OnInit {
  idActuacion: any;
  actuacionNum: any;
  numSolicitud: any;
  numContrato: any;
  tipoControversia: string;
  estadoAvanceTramite: string;
  fechaActuacionAdelantada: any;
  actuacionAdelantada: string;
  actuacionRequerida: string;
  diasVencimiento: any;
  detalleOtra: any;
  fechaVencimiento: any;
  requiereContratista: string;
  requiereInterventor: string;
  requiereSupervisor: string;
  requiereFiduciaria: string;
  requiereComite: string;
  observaciones: any;
  rutaSoporte: any;

  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idActuacion = param.id;
      this.loadData(param.id);
    });
  }
  loadData(id){
    this.services.GetControversiaActuacionById(id).subscribe((data:any)=>{
      this.services.GetControversiaContractualById(data.controversiaContractualId).subscribe((data0:any)=>{
        switch(data.tipoControversiaCodigo){
          case '1':
            this.tipoControversia = 'Terminación anticipada por incumplimiento (TAI)';
          break;
        };
        this.numSolicitud = data0.numeroSolicitudFormat;
        this.numContrato = data0.contrato.numeroContrato;
      });
      this.actuacionNum = data.numeroActuacionFormat;
      switch(data.estadoAvanceTramiteCodigo){
        case '1':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '2':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '3':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '4':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '5':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '6':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '7':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '8':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '9':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '10':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '11':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '12':
          this.estadoAvanceTramite = 'Terminación anticipada por incumplimiento (TAI)';
        break;
      }
      this.fechaActuacionAdelantada= data.fechaActuacion;
      switch(data.actuacionAdelantadaCodigo){
        case '1':
          this.actuacionAdelantada = 'Actuación 1';
        break;
        case '2':
          this.actuacionAdelantada = 'Actuación 2';
        break;
        case '3':
          this.actuacionAdelantada = 'Actuación 3';
        break;
      }
      switch(data.proximaActuacionCodigo){
        case '1':
          this.actuacionRequerida = 'Actuación 1';
        break;
        case '2':
          this.actuacionRequerida = 'Actuación 2';
        break;
        case '3':
          this.actuacionRequerida = 'Actuación 3';
        break;
        case '4':
          this.actuacionRequerida = 'Otra';
        break;
      }
      this.detalleOtra = data.actuacionAdelantadaOtro;
      this.diasVencimiento = data.cantDiasVencimiento;
      this.fechaVencimiento = data.fechaVencimiento;
      switch(data.esRequiereContratista){
        case false:
          this.requiereContratista = 'No';
        break;
        case true:
          this.requiereContratista = 'Sí';
        break;
      }
      switch(data.esRequiereInterventor){
        case false:
          this.requiereInterventor = 'No';
        break;
        case true:
          this.requiereInterventor = 'Sí';
        break;
      }
      switch(data.esRequiereSupervisor){
        case false:
          this.requiereSupervisor = 'No';
        break;
        case true:
          this.requiereSupervisor = 'Sí';
        break;
      }
      switch(data.esRequiereFiduciaria){
        case false:
          this.requiereFiduciaria = 'No';
        break;
        case true:
          this.requiereFiduciaria = 'Sí';
        break;
      }
      switch(data.esRequiereComite){
        case false:
          this.requiereComite = 'No';
        break;
        case true:
          this.requiereComite = 'Sí';
        break;
      }
      this.observaciones = data.observaciones;
      this.rutaSoporte = data.rutaSoporte;
    });
  }
}
