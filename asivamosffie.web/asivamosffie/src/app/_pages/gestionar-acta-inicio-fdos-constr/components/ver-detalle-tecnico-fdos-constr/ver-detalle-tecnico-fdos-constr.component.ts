import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ver-detalle-tecnico-fdos-constr',
  templateUrl: './ver-detalle-tecnico-fdos-constr.component.html',
  styleUrls: ['./ver-detalle-tecnico-fdos-constr.component.scss']
})
export class VerDetalleTecnicoFdosConstrComponent implements OnInit {
  public rolAsignado;
  public opcion;
  constructor() { }

  ngOnInit(): void {
    this.cargarRol();
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

}
