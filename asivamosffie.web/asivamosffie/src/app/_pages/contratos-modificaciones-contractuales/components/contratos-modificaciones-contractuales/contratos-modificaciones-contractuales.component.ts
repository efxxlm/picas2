import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contratos-modificaciones-contractuales',
  templateUrl: './contratos-modificaciones-contractuales.component.html',
  styleUrls: ['./contratos-modificaciones-contractuales.component.scss']
})
export class ContratosModificacionesContractualesComponent implements OnInit {

  verAyuda = false;
  sinDataSinregistro: boolean = true;
  sinDataProcesoFirmas: boolean = true;
  sinDataRegistrados: boolean = true;

  constructor() { }

  ngOnInit(): void {
  }

}
