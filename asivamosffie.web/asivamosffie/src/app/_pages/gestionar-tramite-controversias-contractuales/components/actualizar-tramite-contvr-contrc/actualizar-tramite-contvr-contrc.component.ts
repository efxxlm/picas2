import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-actualizar-tramite-contvr-contrc',
  templateUrl: './actualizar-tramite-contvr-contrc.component.html',
  styleUrls: ['./actualizar-tramite-contvr-contrc.component.scss']
})
export class ActualizarTramiteContvrContrcComponent implements OnInit {
  public controversiaID = localStorage.getItem("controversiaID");
  public selTab;
  constructor() { }

  ngOnInit(): void {
  }
  cambiarTab(opc) {
    this.selTab=opc;
  }
}
