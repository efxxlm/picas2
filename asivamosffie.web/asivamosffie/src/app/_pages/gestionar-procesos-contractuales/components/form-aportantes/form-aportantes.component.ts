import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-aportantes',
  templateUrl: './form-aportantes.component.html',
  styleUrls: ['./form-aportantes.component.scss']
})
export class FormAportantesComponent implements OnInit {

  data: any[] = [
    {
      nombre: 'LL000007 - I.E De Desarrollo Rural Miguel Valencia - Única sede',
      tipoIntervencion: 'Remodelación',
      departamento: 'Antioquia',
      municipio: 'Bello',
      valorTotalProyecto: '230.000.000',
      aportantes: [
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación de Antioquia',
          valorAportanteProyecto: '150.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '150.000.000'
        },
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '80.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '80.000.000'
        },
      ]
    },
    {
      nombre: 'LL000117 - I.E Miguel Suarez - Única sede',
      tipoIntervencion: 'Remodelación',
      departamento: 'Boyacá',
      municipio: 'Paipa',
      valorTotalProyecto: '370.000.000',
      aportantes: [
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación de Boyacá',
          valorAportanteProyecto: '200.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '200.000.000'
        },
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '170.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '170.000.000'
        },
      ]
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
