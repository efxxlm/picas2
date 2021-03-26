import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-polizas-seguros',
  templateUrl: './polizas-seguros.component.html',
  styleUrls: ['./polizas-seguros.component.scss']
})
export class PolizasSegurosComponent implements OnInit {

  listaPolizasSeguros = [
    {
      polizasSeguros: 'N801801',
      vigenciaPoliza: 'LL457326',
      vigenciaAmparo: 'Remodelación',
      valorAmparo: 'Boyacá'
    }
  ];

  listaActualizacion = [
    'Buen manejo y correcta inversión del anticipo',
    'Garantía de estabilidad y calidad de la obra'
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
