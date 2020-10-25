import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';

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

  botonDescargar: boolean =false;
  constructor(private activatedRoute: ActivatedRoute,private services: ActBeginService) { }

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
    this.services.GetVistaGenerarActaInicio(id).subscribe(data => {
      /*Titulo*/
      this.contratoCode = data.numeroContrato;
      this.fechaAprobacionSupervisor = data.plazoInicialContratoSupervisor;
      /*Cuadro 1*/
      this.vigenciaContrato = data.vigenciaContrato;
      this.fechaFirmaContrato = data.fechaFirmaContrato;
      this.numeroDRP1 = data.numeroDRP1;
      this.fechaGeneracionDRP1 = data.fechaGeneracionDRP1;
      this.numeroDRP2 = data.numeroDRP2;
      this.fechaGeneracionDRP2 = data.fechaGeneracionDRP2;
      this.fechaAprobacionGarantiaPoliza = data.fechaAprobacionGarantiaPoliza;
      this.observacionOConsideracionesEspeciales = data.objeto;
      this.valorInicialContrato = data.valorInicialContrato;
      this.valorActualContrato = data.valorActualContrato;
      this.valorFase1Preconstruccion = data.valorFase1Preconstruccion;
      this.valorfase2ConstruccionObra = data.valorfase2ConstruccionObra;
      this.nombreEntidadContratistaSupervisorInterventoria = data.nombreEntidadContratistaSupervisorInterventoria;
      this.nombreEntidadContratistaObra = data.nombreEntidadContratistaObra;
      /*Campo de texto no editable*/
      this.fechaActaInicioConstruccion = data.fechaActaInicioDateTime;
      this.fechaPrevistaTerminacion = data.fechaPrevistaTerminacionDateTime;
      this.obsConEspeciales = data.observacionOConsideracionesEspeciales;
      this.plazoActualContratoMeses = 12;
      this.plazoActualContratoDias = 26;
      this.plazoEjecucionPreConstruccionMeses = data.plazoFase1PreMeses;
      this.plazoEjecucionPreConstruccionDias = data.plazoFase1PreDias;
      this.plazoEjecucionConstrM = data.plazoFase2ConstruccionMeses;
      this.plazoEjecucionConstrD = data.plazoFase2ConstruccionDias;
    });
    this.idContrato = id;
  }
  descargarActaSuscrita(){
    this.services.GetPlantillaActaInicio(this.idContrato).subscribe(resp=>{
      const documento = `Prueba.pdf`; // Valor de prueba
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
