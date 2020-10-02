import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestionar-acta-inicio-fdos-constr',
  templateUrl: './gestionar-acta-inicio-fdos-constr.component.html',
  styleUrls: ['./gestionar-acta-inicio-fdos-constr.component.scss']
})
export class GestionarActaInicioFdosConstrComponent implements OnInit {

  verAyuda = false;
  public rolAsignado;
  public ocpion;
  public selTab;
  constructor() { }

  ngOnInit(): void {
    this.cargarRol();
  }
  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 2) {
      this.ocpion = 1;
    }
    else {
      this.ocpion = 2;
    }
  }
  cambiarTab(opc) {
    this.selTab=opc;
  }
}
