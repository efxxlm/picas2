import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ver-traslados-recursos',
  templateUrl: './ver-traslados-recursos.component.html',
  styleUrls: ['./ver-traslados-recursos.component.scss']
})
export class VerTrasladosRecursosComponent implements OnInit {

  listaTraslados = [
    {
      id: '1',
      fechaTraslado: 'Alcaldía de Susacón',
      numeroTraslado: '$45.000.000',
      numeroContrato: '$105.000.000',
      numeroOrdenGiro: 'Alcaldía de Susacón',
      valorTraslado: '$45.000.000',
      estadoTraslado: '$105.000.000',
    },
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
