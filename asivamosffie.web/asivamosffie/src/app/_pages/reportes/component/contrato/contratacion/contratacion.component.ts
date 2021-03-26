import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contratacion',
  templateUrl: './contratacion.component.html',
  styleUrls: ['./contratacion.component.scss']
})
export class ContratacionComponent implements OnInit {

  listaProyectosAsociados = [
    {
      numeroContrato: 'N801801',
      llaveMEN: 'LL457326',
      tipoIntervension: 'Remodelación',
      departamento: 'Boyacá',
      municipio: 'Susacón',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen',
      sede: 'Única sede'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
