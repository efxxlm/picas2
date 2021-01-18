import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-actualizar-mesa-de-trabajo',
  templateUrl: './actualizar-mesa-de-trabajo.component.html',
  styleUrls: ['./actualizar-mesa-de-trabajo.component.scss']
})
export class ActualizarMesaDeTrabajoComponent implements OnInit {
  idControversia: any;
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  tipoControversia: string;
  solicitud: any;
  numContrato: any;
  fechaSolicitud: any;
  fechaSolicitudTramite: any;
  valorContrato: any;
  nomContratista: any;
  tipoIdentificacion: any;
  numeroIdentificacion: any;
  tipoIntervencion: any;
  plazo: any;
  fechaFinContrato: any;
  fechaInicioContrato: any;
  numPoliza: any;
  numRadicacionAseguradora: any;
  fechaRadicacion: any;
  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService, private polService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
    });
    this.loadService();
  }
  loadService(){
    this.services.GetControversiaContractualById(this.controversiaID).subscribe((data:any)=>{
      this.solicitud = data.numeroSolicitud;
      this.numContrato = data.contrato.numeroContrato;
      this.fechaSolicitud = data.fechaSolicitud;
      this.fechaSolicitudTramite = data.contrato.fechaTramite;
      switch(data.tipoControversiaCodigo){
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
      this.valorContrato = data.contrato.valor;
      this.services.GetVistaContratoContratista(data.contratoId).subscribe((data1:any)=>{
        //Datos del contrato
        this.nomContratista = data1.nombreContratista;
        this.tipoIdentificacion = data1.tipoDocumentoContratista;
        this.numeroIdentificacion = data1.numeroIdentificacion;
        this.tipoIntervencion = data1.tipoIntervencion;
        this.plazo = data1.plazoFormat;
        this.fechaInicioContrato = data1.fechaInicioContrato;
        this.fechaFinContrato = data1.fechaFinContrato;
      });
      this.polService.GetContratoPolizaByIdContratoId(data.contratoId).subscribe((data2:any)=>{
        this.numPoliza = data2.numeroPoliza;
        this.numRadicacionAseguradora = data2.numeroCertificado;
        this.fechaRadicacion = data2.vigencia;
      });
    })

  }
}
