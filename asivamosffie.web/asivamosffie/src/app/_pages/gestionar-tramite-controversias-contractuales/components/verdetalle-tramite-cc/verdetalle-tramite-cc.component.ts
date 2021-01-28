import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-verdetalle-tramite-cc',
  templateUrl: './verdetalle-tramite-cc.component.html',
  styleUrls: ['./verdetalle-tramite-cc.component.scss']
})
export class VerdetalleTramiteCcComponent implements OnInit {
  idControversia: any;
  numeroSolicitud: any;
  nombreContratista: any;
  tipoIdentificacion: any;
  numIdentificacion: any;
  valorContrato: any;
  numContrato: any;
  tipoIntervencion: any;
  plazoContrato: any;
  fechaInicioContrato: any;
  fechaFinalizacionContrato: any;

  tipoControversia: any;
  fechaSolicitud: any;
  motivosList: any;
  fechaComitePreTecnico: any;
  conclusionComitePreTecnico: any;
  laSolicitudProcede: any;
  vaParaComite: string;
  motivosRechazo: any;
  soporteSolicitud: any;
  codAux: any;
  constructor(private activatedRoute: ActivatedRoute,
    private services: ContractualControversyService,
    private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.id;
      this.loadData(param.id);
      this.loadDetailsControversy(param.id);
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
      });
      this.services.GetVistaContratoContratista(resp.contratoId).subscribe((resp_1:any)=>{
        this.numContrato = resp_1.numeroContrato;
        this.tipoIntervencion = resp_1.tipoIntervencion;
        this.plazoContrato = resp_1.plazoFormat;
        this.fechaInicioContrato = resp_1.fechaInicioContrato;
        this.fechaFinalizacionContrato = resp_1.fechaFinContrato;
      });
    });
  }
  loadDetailsControversy(id){
    this.services.GetControversiaContractualById(id).subscribe((resp: any) => {
      this.codAux = resp.tipoControversiaCodigo;
      switch(resp.tipoControversiaCodigo){
        case '1':
          this.tipoControversia = 'Terminación anticipada por incumplimiento (TAI)';
        break;
        case '2':
          this.tipoControversia = 'Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratista';
        break;
        case '3':
          this.tipoControversia = 'Arreglo Directo (AD) a solicitud del contratista';
        break;
        case '4':
          this.tipoControversia = 'Otras controversias contractuales (OCC) a solicitud del contratista';
        break;
        case '5':
          this.tipoControversia = 'Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratante';
        break;
        case '6':
          this.tipoControversia = 'Arreglo Directo (AD) a solicitud del contratante';
        break;
        case '7':
          this.tipoControversia = 'Otras controversias contractuales (OCC) a solicitud del contratante';
        break;
      };
      this.fechaSolicitud = resp.fechaSolicitud;
      this.services.GetMotivosSolicitudByControversiaId(id).subscribe((resp_0: any) => {
        this.motivosList = resp_0;
      }); 
      this.fechaComitePreTecnico = resp.fechaComitePreTecnico;
      this.conclusionComitePreTecnico = resp.conclusionComitePreTecnico;
      switch(resp.esProcede){
        case false:
          this.laSolicitudProcede = 'No';
        break;
        case true:
          this.laSolicitudProcede = 'Sí';
        break;
      }
      switch(resp.esRequiereComite){
        case false:
          this.vaParaComite = 'No';
        break;
        case true:
          this.vaParaComite = 'Sí';
        break;
      }
      if(this.laSolicitudProcede=='No'){
        this.motivosRechazo = resp.motivoJustificacionRechazo;
      }
      this.soporteSolicitud = resp.rutaSoporte;
    });
  }
}
