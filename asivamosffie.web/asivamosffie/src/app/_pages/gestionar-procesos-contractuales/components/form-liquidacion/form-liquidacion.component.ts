import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-liquidacion',
  templateUrl: './form-liquidacion.component.html',
  styleUrls: ['./form-liquidacion.component.scss']
})
export class FormLiquidacionComponent implements OnInit {

  dataForm: any[] = [
    {
      nombre: 'LL000208 - I.E Andrés Bello',
      tipoIntervencion: 'Remodelación',
      departamento: 'Valle del Cauca',
      municipio: 'Jamundi',
      valorTotalProyecto: '80.000.000',
      aportantes: [
        {
          tipoAportante: 'ET',
          nombreAportante: 'Gobernación Del Valle del Cauca',
          valorAportanteProyecto: '30.000.000',
          fuente: 'Recursos propios',
          valorSolicitado: '30.000.000'
        },
        {
          tipoAportante: 'FFIE',
          nombreAportante: 'FFIE',
          valorAportanteProyecto: '50.000.000',
          fuente: 'Contingencias',
          valorSolicitado: '50.000.000'
        }
      ]
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
