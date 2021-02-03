import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalle-actuacion-notai',
  templateUrl: './ver-detalle-actuacion-notai.component.html',
  styleUrls: ['./ver-detalle-actuacion-notai.component.scss']
})
export class VerDetalleActuacionNotaiComponent implements OnInit {

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

  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idActuacion = param.id;
      this.loadData(param.id);
    });
  }
  loadData(id){
    this.services.GetControversiaActuacionById(id).subscribe((a:any)=>{
      this.services.GetControversiaContractualById(a.controversiaContractualId).subscribe((b:any)=>{
        switch(b.tipoControversiaCodigo){
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
      switch(a.estadoAvanceTramiteCodigo){
        case '1':
          this.estadoAvanceTramite = "Proyección de Comunicación de Inicio de TAI";
        break;
        case '2':
          this.estadoAvanceTramite = "Aprobación de Comunicación de Inicio de TAI por el Director Jurídico";
        break;
        case '3':
          this.estadoAvanceTramite = "Remisión de Comunicación de Inicio de TAI a Alianza Fiduciaria";
        break;
        case '4':
          this.estadoAvanceTramite = "Remisión de Comunicación de Inicio de TAI por Alianza Fiduciaria al contratista";
        break;
        case '5':
          this.estadoAvanceTramite =  "Recepción de descargos por Alianza Fiduciaria";
        break;
        case '6':
          this.estadoAvanceTramite = "Traslado para pronunciamiento de la interventoría o supervisión del contrato";
        break;
        case '7':
          this.estadoAvanceTramite = "Remisión de pronunciamiento de interventoría frente a descargos a la UG PAFFIE";
        break;
        case '8':
          this.estadoAvanceTramite = "Proyección de documento de decisión TAI";
        break;
        case '9':
          this.estadoAvanceTramite = "Aprobación de documento de decisión TAI por el Director Jurídico";
        break;
        case '10':
          this.estadoAvanceTramite = "Presentación de decisión TAI ante el Comité Técnico";
        break;
        case '11':
          this.estadoAvanceTramite = "Presentación de decisión TAI ante el Comité Fiduciario";
        break;
        case '12':
          this.estadoAvanceTramite = "Remisión de decisión de TAI a Alianza Fiduciaria";
        break;
        case '13':
          this.estadoAvanceTramite = "Remisión de Comunicación de decisión de TAI por Alianza Fiduciaria al contratista ";
        break;
        case '14':
          this.estadoAvanceTramite = "Remisión de Comunicación de decisión de TAI por Alianza Fiduciaria a la  Aseguradora";
        break;
        case '15':
          this.estadoAvanceTramite = " Envío de decisiones Comunicadas a la UG-PA FFIE ";
        break;
      }
      this.fechaActuacionAdelantada= a.fechaActuacion;
      switch(a.actuacionAdelantadaCodigo){
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
      switch(a.proximaActuacionCodigo){
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
      this.detalleOtra = a.actuacionAdelantadaOtro;
      this.diasVencimiento = a.cantDiasVencimiento;
      this.fechaVencimiento = a.fechaVencimiento;
      switch(a.esRequiereContratista){
        case false:
          this.requiereContratista = 'No';
        break;
        case true:
          this.requiereContratista = 'Sí';
        break;
      }
      switch(a.esRequiereInterventor){
        case false:
          this.requiereInterventor = 'No';
        break;
        case true:
          this.requiereInterventor = 'Sí';
        break;
      }
      switch(a.esRequiereSupervisor){
        case false:
          this.requiereSupervisor = 'No';
        break;
        case true:
          this.requiereSupervisor = 'Sí';
        break;
      }
      switch(a.esRequiereFiduciaria){
        case false:
          this.requiereFiduciaria = 'No';
        break;
        case true:
          this.requiereFiduciaria = 'Sí';
        break;
      }
      switch(a.esRequiereComite){
        case false:
          this.requiereComite = 'No';
        break;
        case true:
          this.requiereComite = 'Sí';
        break;
      }
      this.observaciones = a.observaciones;
      this.rutaSoporte = a.rutaSoporte;
      switch(a.esRequiereMesaTrabajo){
        case false:
          this.requiereMT = 'No';
        break;
        case true:
          this.requiereMT = 'Sí';
        break;
      }
      switch(a.esprocesoResultadoDefinitivo){
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
