import { Component, OnInit } from '@angular/core';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-detalle-acta-ini-f-i-prc',
  templateUrl: './ver-detalle-acta-ini-f-i-prc.component.html',
  styleUrls: ['./ver-detalle-acta-ini-f-i-prc.component.scss']
})
export class VerDetalleActaIniFIPreconstruccioComponent implements OnInit {
  public conObservaciones:boolean;
  public botonDescargarActaSuscrita: boolean;

  public contratoId;
  public rolAsignado;
  public opcion;
  public numContrato;
  public fechaFirmaContrato;

  public fechaActaFase1Prc;
  public fechaTermPrevista;
  public diasFase1;
  public mesesFase1;
  public diasFase2;
  public mesesFase2;
  public observaciones;
  fechaAprobacionRequisitos: any;
  contratacionId: any;
  fechaTramite: any;
  tipoContratoCodigo: any;
  estadoDocumentoCodigo: any;
  fechaEnvioFirma: any;
  fechaFirmaContratista: any;
  fechaFirmaFiduciaria: any;
  numDRP: any;
  fechaDRP: any;
  objeto: any;
  valorIni: any;
  nitContratistaInterventoria: any;
  nomContratista: any;
  public mesPlazoIni: number = 10;
  public diasPlazoIni: number = 25;
  fechaAprobGarantiaPoliza: any;
  vigenciaContrato: any;
  valorFUno: any;
  valorFDos: any;
  nomEntidadContratistaIntervn: any;
  numIdContratistaObra: any;
  tieneObservacionesBool: any;
  observacionesUltimas: any;
  dataElements: any;
  contratoObservacionId: any;
  fechaCreacionObs: any;
  numIdRepresentanteLegal: any;
  nomRepresentanteLegal: any;
  tipoProponente: any;
  dataSupervisor: boolean =false; 
  nomSupervisor: string;
  numIdentifiacionSupervisor: string;
  constructor( private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarRol();
    this.actasuscritaHabilitada();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.loadService(param.id);
      this.contratoId = param.id;
    });
  }
  loadData(id){
    this.service.GetContratoByContratoId(id).subscribe((data:any)=>{
      this.cargarDataParaInsercion(data);
      this.numContrato = data.numeroContrato;
      this.fechaFirmaContrato = data.fechaFirmaContrato;
      this.fechaActaFase1Prc = data.fechaActaInicioFase1;
      this.fechaTermPrevista = data.fechaTerminacion;
      this.diasFase1 = data.plazoFase1PreDias;
      this.mesesFase1 = data.plazoFase1PreMeses;
      this.diasFase2 = data.plazoFase2ConstruccionDias;
      this.mesesFase2 = data.plazoFase2ConstruccionMeses;
      if(data.observaciones==undefined || data.observaciones==null || data.observaciones=="") {
        this.observaciones = "-----";
      }
      else{
        this.observaciones = data.observaciones;
      }
      this.verObservaciones(data.conObervacionesActa);
    });
  }
  loadService(id){
    this.service.GetListContratoObservacionByContratoId(id).subscribe(data=>{
      this.dataElements = data;
      for(let i=0; i<this.dataElements.length;i++){ 

        this.tieneObservacionesBool = this.dataElements[i].esActaFase1;
        this.observacionesUltimas = this.dataElements[i].observaciones;
        this.contratoObservacionId = this.dataElements[i].contratoObservacionId;
        this.fechaCreacionObs = this.dataElements[i].fechaCreacion
      }
    });
  }
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaAprobacionRequisitos = data.fechaAprobacionRequisitosSupervisor;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
    this.numDRP = data.contratacion.disponibilidadPresupuestal[0].numeroDrp;
    this.fechaDRP = data.contratacion.disponibilidadPresupuestal[0].fechaCreacion;
    this.objeto = data.contratacion.disponibilidadPresupuestal[0].objeto;
    this.valorIni = data.contratacion.disponibilidadPresupuestal[0].valorSolicitud;
    this.numIdRepresentanteLegal = data.contratacion.contratista.representanteLegalNumeroIdentificacion;
    this.nomRepresentanteLegal = data.contratacion.contratista.representanteLegal;
    this.nitContratistaInterventoria = data.contratacion.contratista.numeroIdentificacion;
    this.fechaAprobGarantiaPoliza = data.contratoPoliza[0].fechaAprobacion;
    this.vigenciaContrato = data.fechaTramite;
    this.valorFUno = data.valorFase1;
    this.valorFDos = data.valorFase2;
    this.nomEntidadContratistaIntervn = data.contratacion.contratista.nombre;
    this.numIdContratistaObra = data.contratacion.contratista.representanteLegalNumeroIdentificacion
    this.mesPlazoIni= data.contratacion.disponibilidadPresupuestal[0].plazoMeses;
    this.diasPlazoIni= data.contratacion.disponibilidadPresupuestal[0].plazoDias;
    this.tipoProponente = data.contratacion.contratista.tipoProponenteCodigo;
    if(localStorage.getItem("origin")=="interventoria"){
      this.dataSupervisor = true;
      this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
      this.nomSupervisor = data.usuarioInterventoria.nombres+" "+data.usuarioInterventoria.apellidos;
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

  verObservaciones(observaciones){
    if(observaciones==true){
      this.conObservaciones=true;
    }
    else{
      this.conObservaciones=false;
    }
  }

  actasuscritaHabilitada(){
    if(localStorage.getItem("actaSuscrita")=="true"){
      this.botonDescargarActaSuscrita=true;
    }
    else{
      this.botonDescargarActaSuscrita=false;
    }
  }
  generarActaSuscrita(){
    this.service.GetActaByIdPerfil(this.rolAsignado,this.contratoId).subscribe(resp=>{
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
