import { Component, OnInit } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
@Component({
  selector: 'app-generar-acta-inicio-f-i-prc',
  templateUrl: './generar-acta-inicio-f-i-prc.component.html',
  styleUrls: ['./generar-acta-inicio-f-i-prc.component.scss']
})
export class GenerarActaInicioFaseunoPreconstruccionComponent implements OnInit {
  verAyuda = false;
  public rolAsignado;
  public ocpion;
  public selTab;
  contratoObra = false;
  contratoInterventoria = false;
  constructor() { }

  ngOnInit(): void {
    this.cargarRol();
  }
  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 11) {
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
