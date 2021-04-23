import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalle-actuacion-notai',
  templateUrl: './ver-detalle-actuacion-notai.component.html',
  styleUrls: ['./ver-detalle-actuacion-notai.component.scss']
})
export class VerDetalleActuacionNotaiComponent implements OnInit {
  idControversia: any;
  idActuacion: any;
  tipoControversia: string;
  numSolicitud: any;
  numContrato: any;
  actuacionNum: any;
  estadoAvanceTramite: string;
  fechaActuacionAdelantada: any;
  actuacionAdelantada: string;
  actuacionRequerida: string;
  detalleOtra: any;
  detalleOtra1: any;
  diasVencimiento: any;
  fechaVencimiento: any;
  requiereContratista: string;
  requiereInterventor: string;
  requiereSupervisor: string;
  requiereFiduciaria: string;
  requiereComite: string;
  observaciones: any;
  rutaSoporte: any;
  requiereMT: string;
  procesoResultadoYDefinitivo: string;

  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.idControversia;
      this.idActuacion = param.id;
      this.loadData(param.id);
    });
  }
  loadData(id) {
    this.services.GetControversiaActuacionById(id).subscribe((a: any) => {
      this.services.GetControversiaContractualById(a.controversiaContractualId).subscribe((b: any) => {
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
        };
        this.numSolicitud = b.numeroSolicitud;
        this.numContrato = b.contrato.numeroContrato;
      });
      this.actuacionNum = a.numeroActuacionFormat;
      switch (a.estadoAvanceTramiteCodigo) {
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
      this.fechaActuacionAdelantada = a.fechaActuacion;
      switch (a.actuacionAdelantadaCodigo) {
        case '1':
          this.actuacionAdelantada = 'Oficio radicado';
          break;
        case '2':
          this.actuacionAdelantada = 'Reparto al abogado';
          break;
        case '3':
          this.actuacionAdelantada = 'Solicitud de concepto al contratista de interventoría';
          break;
        case '4':
          this.actuacionAdelantada = 'Solicitud de concepto al supervisor del contrato';
          break;
        case '5':
          this.actuacionAdelantada = 'Elaboración de minuta o acta';
          break;
        case '6':
          this.actuacionAdelantada = 'Aprobación de minuta o acta por parte del Director Jurídico';
          break;
        case '7':
          this.actuacionAdelantada = 'Presentación al Comité Técnico';
          break;
        case '8':
          this.actuacionAdelantada = 'Presentación al Comité Fiduciario';
          break;
        case '9':
          this.actuacionAdelantada = 'Remisión a Alianza Fiduciaria';
          break;
        case '10':
          this.actuacionAdelantada = 'Remisión de Comunicación de decisión por Alianza Fiduciaria al contratista';
          break;
        case '11':
          this.actuacionAdelantada = 'Reclamación ante la compañía aseguradora';
          break;
        case '12':
          this.actuacionAdelantada = 'Envío de decisiones Comunicadas a la UG-PA FFIE';
          break;
        case '13':
          this.actuacionAdelantada = 'Otra';
          break;
      }
      this.detalleOtra = a.actuacionAdelantadaOtro;
      switch (a.proximaActuacionCodigo) {
        case '1':
          this.actuacionRequerida = 'Oficio radicado';
          break;
        case '2':
          this.actuacionRequerida = 'Reparto al abogado';
          break;
        case '3':
          this.actuacionRequerida = 'Solicitud de concepto al contratista de interventoría';
          break;
        case '4':
          this.actuacionRequerida = 'Solicitud de concepto al supervisor del contrato';
          break;
        case '5':
          this.actuacionRequerida = 'Elaboración de minuta o acta';
          break;
        case '6':
          this.actuacionRequerida = 'Aprobación de minuta o acta por parte del Director Jurídico';
          break;
        case '7':
          this.actuacionRequerida = 'Presentación al Comité Técnico';
          break;
        case '8':
          this.actuacionRequerida = 'Presentación al Comité Fiduciario';
          break;
        case '9':
          this.actuacionRequerida = 'Remisión a Alianza Fiduciaria';
          break;
        case '10':
          this.actuacionRequerida = 'Remisión de Comunicación de decisión por Alianza Fiduciaria al contratista';
          break;
        case '11':
          this.actuacionRequerida = 'Reclamación ante la compañía aseguradora';
          break;
        case '12':
          this.actuacionRequerida = 'Envío de decisiones Comunicadas a la UG-PA FFIE';
          break;
        case '13':
          this.actuacionRequerida = 'Otra';
          break;
      }
      this.detalleOtra1 = a.proximaActuacionOtro;
      this.diasVencimiento = a.cantDiasVencimiento;
      this.fechaVencimiento = a.fechaVencimiento;
      switch (a.esRequiereContratista) {
        case false:
          this.requiereContratista = 'No';
          break;
        case true:
          this.requiereContratista = 'Sí';
          break;
      }
      switch (a.esRequiereInterventor) {
        case false:
          this.requiereInterventor = 'No';
          break;
        case true:
          this.requiereInterventor = 'Sí';
          break;
      }
      switch (a.esRequiereSupervisor) {
        case false:
          this.requiereSupervisor = 'No';
          break;
        case true:
          this.requiereSupervisor = 'Sí';
          break;
      }
      switch (a.esRequiereFiduciaria) {
        case false:
          this.requiereFiduciaria = 'No';
          break;
        case true:
          this.requiereFiduciaria = 'Sí';
          break;
      }
      switch (a.esRequiereComite) {
        case false:
          this.requiereComite = 'No';
          break;
        case true:
          this.requiereComite = 'Sí';
          break;
      }
      this.observaciones = a.observaciones;
      this.rutaSoporte = a.rutaSoporte;
      switch (a.esRequiereMesaTrabajo) {
        case false:
          this.requiereMT = 'No';
          break;
        case true:
          this.requiereMT = 'Sí';
          break;
      }
      switch (a.esprocesoResultadoDefinitivo) {
        case false:
          this.procesoResultadoYDefinitivo = 'No';
          break;
        case true:
          this.procesoResultadoYDefinitivo = 'Sí';
          break;
      }
    });
  }
}
