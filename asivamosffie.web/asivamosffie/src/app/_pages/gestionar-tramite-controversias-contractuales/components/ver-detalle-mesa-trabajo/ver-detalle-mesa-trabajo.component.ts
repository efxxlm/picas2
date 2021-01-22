import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalle-mesa-trabajo',
  templateUrl: './ver-detalle-mesa-trabajo.component.html',
  styleUrls: ['./ver-detalle-mesa-trabajo.component.scss']
})
export class VerDetalleMesaTrabajoComponent implements OnInit {
  idActuacion: any;
  public nomMesaTrabajo = localStorage.getItem("nomMesaTrabajo");
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  solicitud: any;
  numContrato: any;
  tipoControversia: string;
  public idMesadeTrabajo = parseInt(localStorage.getItem('idMesa'));

  estadoAvanceMesaTrabajo: any;
  fechaActuacionAdelantada: any;
  actuacionAdelantada: any;
  proximaActuacionRequerida: any;
  diasVencimientoTerminos: any;
  fechaVencimientoActReq: any;
  observaciones: any;
  resultadoDefinitivo: any;
  urlSporte: any;

  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idActuacion = param.id;
      this.loadBasicData(this.idActuacion);
    });
  }
  loadBasicData(id) {
    this.services.GetControversiaActuacionById(id).subscribe((a: any) => {
      this.services.GetControversiaContractualById(a.controversiaContractualId).subscribe((b: any) => {
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
    });
    this.services.GetMesaByMesaId(this.idMesadeTrabajo).subscribe((c: any) => {
      switch (c.estadoAvanceMesaCodigo) {
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
      this.fechaActuacionAdelantada = c.fechaActuacionAdelantada;
      this.actuacionAdelantada = c.actuacionAdelantada;
      this.proximaActuacionRequerida = c.proximaActuacionRequerida;
      this.diasVencimientoTerminos = c.cantDiasVencimiento;
      this.fechaVencimientoActReq = c.fechaVencimiento;
      this.observaciones = c.observaciones;
      switch (c.resultadoDefinitivo) {
        case false:
          this.resultadoDefinitivo = "No";
          break;
        case true:
          this.resultadoDefinitivo = "Sí";
          break;
      }
      this.urlSporte = c.rutaSoporte;
    });
  }
}
