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

  constructor( private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService) { }

  ngOnInit(): void {
    this.cargarRol();
    this.actasuscritaHabilitada();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
  }
  loadData(id){
    this.service.GetContratoByContratoId(id).subscribe(data=>{
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
    alert("genera PDF");
  }
}
