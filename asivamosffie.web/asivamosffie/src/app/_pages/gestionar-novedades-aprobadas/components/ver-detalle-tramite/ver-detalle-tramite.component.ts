import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ver-detalle-tramite',
  templateUrl: './ver-detalle-tramite.component.html',
  styleUrls: ['./ver-detalle-tramite.component.scss']
})
export class VerDetalleTramiteComponent implements OnInit {

  detallarSolicitud = [
    {
      aportante: 'Fundación Pies Descalzos',
      valorAportante: '$ 60.000.000',
      componente: 'Obra',
      fase: 'Pre-Construcción ',
      uso: 'Diseño Obra Complementaria',
      valorUso: '$ 20.000.000'
    },
    {
      aportante: 'Fundación Pies Descalzos',
      valorAportante: '$ 60.000.000',
      componente: 'Interventoría',
      fase: 'Pre-Construcción ',
      uso: 'Interventoría Diseño Obra Complementaria',
      valorUso: '$ 40.000.000'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
