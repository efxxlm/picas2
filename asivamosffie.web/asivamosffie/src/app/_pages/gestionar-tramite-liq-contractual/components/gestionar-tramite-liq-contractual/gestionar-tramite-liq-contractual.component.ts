import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gestionar-tramite-liq-contractual',
  templateUrl: './gestionar-tramite-liq-contractual.component.html',
  styleUrls: ['./gestionar-tramite-liq-contractual.component.scss']
})
export class GestionarTramiteLiqContractualComponent implements OnInit {
  verAyuda = false;
  estadoAcordeonObra: string;
  estadoAcordeonInterventoria: string;
  
  constructor() { }

  ngOnInit(): void {
  }

}
