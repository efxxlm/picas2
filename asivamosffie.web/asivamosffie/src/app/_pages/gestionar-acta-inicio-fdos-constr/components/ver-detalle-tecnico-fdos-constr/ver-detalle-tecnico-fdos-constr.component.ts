import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-ver-detalle-tecnico-fdos-constr',
  templateUrl: './ver-detalle-tecnico-fdos-constr.component.html',
  styleUrls: ['./ver-detalle-tecnico-fdos-constr.component.scss']
})
export class VerDetalleTecnicoFdosConstrComponent implements OnInit {
  public rolAsignado;
  public opcion;
  public idContrato;
  contratoCode: string;
  fechaAprobacionSupervisor: Date;
  vigenciaContrato: Date;
  fechaFirmaContrato: Date;
  numeroDRP1: string;
  fechaGeneracionDRP1: Date;
  numeroDRP2: string;
  fechaGeneracionDRP2: Date;
  fechaAprobacionGarantiaPoliza: Date;
  observacionOConsideracionesEspeciales: string;
  valorInicialContrato: string;
  valorActualContrato: string;
  valorFase1Preconstruccion: string;
  valorfase2ConstruccionObra: string;
  nombreEntidadContratistaSupervisorInterventoria: string;
  nombreEntidadContratistaObra: string;
  editable: boolean;
  addressForm: any;
  plazoActualContratoMeses: number;
  plazoActualContratoDias: number;
  plazoEjecucionPreConstruccionMeses: number;
  plazoEjecucionPreConstruccionDias: number;
  fechaActaInicioConstruccion: Date;
  fechaPrevistaTerminacion: Date;
  obsConEspeciales: string;
  plazoEjecucionConstrM: number;
  plazoEjecucionConstrD: number;
  mostrarCarga= false;

  botonDescargar: boolean =false;
  conObservacionesSupervisor: boolean;
  observacionesSupervisor: string;
  fechaCreacion: Date;
  objeto: any;
  numeroIdentificacionRepresentanteContratistaInterventoria: any;
  valorProponente: any;
  rutaActaSuscrita: any;
  constructor(private activatedRoute: ActivatedRoute,private services: ActBeginService, private commonSvc: CommonService, private gestionarActaSvc: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
    if(localStorage.getItem("actaSuscrita") == "true"){
      this.botonDescargar = true;
    }
    else{
      this.botonDescargar = false;
    }
  }

  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 2) {
      this.opcion = 1;
    }
    else {
      this.opcion = 2;
    }
  }

  loadData(id) {
    this.services.GetVistaGenerarActaInicio(id).subscribe((data:any) => {
      /*Titulo*/
      this.contratoCode = data.numeroContrato;
      this.fechaAprobacionSupervisor = data.fechaAprobacionRequisitosSupervisor;
      /*Cuadro 1*/
      this.vigenciaContrato = data.vigenciaContrato;
      this.fechaFirmaContrato = data.fechaFirmaContrato;
      this.numeroDRP1 = data.numeroDRP1;
      this.fechaGeneracionDRP1 = data.fechaGeneracionDRP1;
      this.numeroDRP2 = data.numeroDRP2;
      this.objeto = data.objeto;
      this.fechaGeneracionDRP2 = data.fechaGeneracionDRP2;
      this.fechaAprobacionGarantiaPoliza = data.fechaAprobacionGarantiaPoliza;
      this.observacionOConsideracionesEspeciales = data.objeto;
      this.numeroIdentificacionRepresentanteContratistaInterventoria = data.numeroIdentificacionRepresentanteContratistaInterventoria;
      this.valorInicialContrato = data.valorInicialContrato;
      this.valorActualContrato = data.valorActualContrato;
      this.valorFase1Preconstruccion = data.valorFase1Preconstruccion;
      this.valorfase2ConstruccionObra = data.valorfase2ConstruccionObra;
      this.nombreEntidadContratistaSupervisorInterventoria = data.nombreEntidadContratistaSupervisorInterventoria;
      this.nombreEntidadContratistaObra = data.nombreEntidadContratistaObra;
      this.valorProponente = data.proponenteCodigo;
      /*Campo de texto no editable*/
      this.fechaActaInicioConstruccion = data.fechaActaInicioFase2DateTime;
      this.fechaPrevistaTerminacion = data.fechaPrevistaTerminacionDateTime;
      this.plazoActualContratoMeses = data.plazoActualContratoMeses;
      this.plazoActualContratoDias = data.plazoActualContratoDias;
      this.plazoEjecucionPreConstruccionMeses = data.plazoFase1PreMeses;
      this.plazoEjecucionPreConstruccionDias = data.plazoFase1PreDias;
      this.obsConEspeciales = data.observacionOConsideracionesEspeciales;
      this.plazoEjecucionConstrM = data.plazoFase2ConstruccionDias;
      this.plazoEjecucionConstrD = data.plazoFase2ConstruccionMeses;
      //ruta del acta suscrita
      this.rutaActaSuscrita = data.rutaActaSuscrita;
      //console.log(data.contrato.estadoActaFase2, data.contrato.estadoActaFase2 == 20 );
      if ( data.contrato && ( data.contrato.estadoActaFase2 == 20 || data.contrato.estadoActaFase2 == 7 ) )
        this.mostrarCarga = true;
    });
    this.services.GetContratoObservacionByIdContratoId(id,true).subscribe(data1=>{
      if ( data1 ){
        this.conObservacionesSupervisor = data1.esActa;
        this.observacionesSupervisor = data1.observaciones;
        this.fechaCreacion = data1.fechaCreacion;
      }
    });
    this.idContrato = id;
  }
  descargarActaSuscrita(doc){
    this.commonSvc.getDocumento(doc).subscribe(
      response => {

        const documento = `Acta fase 2 contrato ${this.contratoCode}.pdf`;
        const text = documento,
        blob = new Blob([response], { type: 'application/pdf' }),
        anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();

      },
      err => console.log( `<b>${err.message}</b>` )
    );
  }
  descargarActaDesdeTabla() {
    this.gestionarActaSvc.GetActaByIdPerfil(this.idContrato, 'True').subscribe(resp => {
      const documento = `${this.contratoCode}.pdf`; // Valor de prueba
      const text = documento,
        blob = new Blob([resp], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
}
