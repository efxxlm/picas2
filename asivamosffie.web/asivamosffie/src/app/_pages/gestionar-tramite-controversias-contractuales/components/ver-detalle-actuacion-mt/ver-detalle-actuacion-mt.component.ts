import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalle-actuacion-mt',
  templateUrl: './ver-detalle-actuacion-mt.component.html',
  styleUrls: ['./ver-detalle-actuacion-mt.component.scss']
})
export class VerDetalleActuacionMtComponent implements OnInit {
  idSeguimientoMesa: any;
  public nomMesaTrabajo = localStorage.getItem("numActuacionMT");
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  solicitud: any;
  numContrato: any;
  tipoControversia: string;

  estadoAvanceMesaTrabajo: any;
  fechaActuacionAdelantada: any;
  actuacionAdelantada: any;
  proximaActuacionRequerida: any;
  diasVencimientoTerminos: any;
  fechaVencimientoActReq: any;
  observaciones: any;
  resultadoDefinitivo: any;
  urlSporte: any;

  constructor(private services: ContractualControversyService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idSeguimientoMesa = param.id;
    });
    this.loadBasicData();
  }
  loadBasicData() {
    this.services.GetControversiaContractualById(this.controversiaID).subscribe((b: any) => {
      this.solicitud = b.numeroSolicitud;
      this.numContrato = b.contrato.numeroContrato;
      switch (b.tipoControversiaCodigo) {
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
    this.services.GetActuacionMesaByActuacionMesaId(this.idSeguimientoMesa).subscribe((d: any) => {
      switch (d.estadoAvanceMesaCodigo) {
        case '1':
          this.estadoAvanceMesaTrabajo = "En análisis de evidencias";
          break;
        case '2':
          this.estadoAvanceMesaTrabajo = "Citación remitida";
          break;
        case '3':
          this.estadoAvanceMesaTrabajo = "Sesión No. 1";
          break;
        case '4':
          this.estadoAvanceMesaTrabajo = "Sesión No. 2";
          break;
        case '5':
          this.estadoAvanceMesaTrabajo = "Sesión No. n.";
          break;
      }
      this.fechaActuacionAdelantada = d.fechaActuacionAdelantada;
      this.actuacionAdelantada = d.actuacionAdelantada;
      this.proximaActuacionRequerida = d.proximaActuacionRequerida;
      this.diasVencimientoTerminos = d.cantDiasVencimiento;
      this.fechaVencimientoActReq = d.fechaVencimiento;
      this.observaciones = d.observaciones;
      switch (d.resultadoDefinitivo) {
        case false:
          this.resultadoDefinitivo = "No";
          break;
        case true:
          this.resultadoDefinitivo = "Sí";
          break;
      }
      this.urlSporte = d.rutaSoporte;
    });
  }

}
