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
  detalleOtra1: any;
  fechaVencimiento: any;
  requiereContratista: string;
  requiereInterventor: string;
  requiereSupervisor: string;
  requiereFiduciaria: string;
  requiereComite: string;
  observaciones: any;
  rutaSoporte: any;
  public controversiaId;
  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.controversiaId = param.idControversia;
      this.idActuacion = param.id;
      this.loadData(param.id);
    });
  }
  loadData(id) {
    this.services.GetControversiaActuacionById(id).subscribe((data: any) => {
      this.services.GetControversiaContractualById(data.controversiaContractualId).subscribe((data0: any) => {
        switch (data0.tipoControversiaCodigo) {
          case '1':
            this.tipoControversia = 'Terminación anticipada por incumplimiento (TAI)';
            break;
        };
        this.numSolicitud = data0.numeroSolicitud;
        this.numContrato = data0.contrato.numeroContrato;
      });
      this.actuacionNum = data.numeroActuacionFormat;
      switch (data.estadoAvanceTramiteCodigo) {
        case '1':
          this.estadoAvanceTramite = "Estado 1";
          break;
        case '2':
          this.estadoAvanceTramite = "Estado 2";
          break;
        case '3':
          this.estadoAvanceTramite = "Estado 3";
          break;
        case '4':
          this.estadoAvanceTramite = "Estado 4";
          break;
      }
      this.fechaActuacionAdelantada = data.fechaActuacion;
      this.actuacionAdelantada = data?.actuacionAdelantadaString;
      this.actuacionRequerida = data?.proximaActuacionCodigoString;
      this.detalleOtra1 = data.actuacionAdelantadaOtro;
      this.detalleOtra = data.proximaActuacionOtro;
      this.diasVencimiento = data.cantDiasVencimiento;
      this.fechaVencimiento = data.fechaVencimiento;
      switch (data.esRequiereContratista) {
        case false:
          this.requiereContratista = 'No';
          break;
        case true:
          this.requiereContratista = 'Sí';
          break;
      }
      switch (data.esRequiereInterventor) {
        case false:
          this.requiereInterventor = 'No';
          break;
        case true:
          this.requiereInterventor = 'Sí';
          break;
      }
      switch (data.esRequiereSupervisor) {
        case false:
          this.requiereSupervisor = 'No';
          break;
        case true:
          this.requiereSupervisor = 'Sí';
          break;
      }
      switch (data.esRequiereFiduciaria) {
        case false:
          this.requiereFiduciaria = 'No';
          break;
        case true:
          this.requiereFiduciaria = 'Sí';
          break;
      }
      switch (data.esRequiereComite) {
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
