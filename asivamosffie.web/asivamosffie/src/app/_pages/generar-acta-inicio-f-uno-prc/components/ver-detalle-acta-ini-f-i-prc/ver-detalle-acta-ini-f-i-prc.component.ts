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
  constructor( private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarRol();
    this.actasuscritaHabilitada();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
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
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaAprobacionRequisitos = data.fechaAprobacionRequisitos;
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
    this.objeto = data.objeto;
    this.valorIni = data.valor;
    this.nitContratistaInterventoria = data.contratista.numeroIdentificacion;
    this.nomContratista = data.contratista.nombre;
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
    this.service.GetActaByIdPerfil(this.rolAsignado,this.contratoId).subscribe((resp:any)=>{
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    })
  }
}
