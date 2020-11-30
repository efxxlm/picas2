import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-verificar-requisitos-construccion',
  templateUrl: './verificar-requisitos-construccion.component.html',
  styleUrls: ['./verificar-requisitos-construccion.component.scss']
})
export class VerificarRequisitosConstruccionComponent implements OnInit {
  verAyuda = false;
  public selTab;
  constructor() { }

  ngOnInit(): void {
  }
  cambiarTab(opc) {
    this.selTab=opc;
  }
}
