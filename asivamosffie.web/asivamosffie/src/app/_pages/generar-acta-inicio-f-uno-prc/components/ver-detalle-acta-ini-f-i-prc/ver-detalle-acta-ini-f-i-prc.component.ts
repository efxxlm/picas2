import { Component, OnInit } from '@angular/core';

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


  constructor() { }

  ngOnInit(): void {
    this.cargarRol();
    this.verObservaciones();
    this.actasuscritaHabilitada();
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

  verObservaciones(){
    if(localStorage.getItem("conObservaciones")=="true"){
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
