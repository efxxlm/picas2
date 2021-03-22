import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-ver-detalle',
  templateUrl: './ver-detalle.component.html',
  styleUrls: ['./ver-detalle.component.scss']
})
export class VerDetalleComponent implements OnInit {

  listaFacturado = [
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      uso: 'Diseño',
      tipoPago: '$12.000.000',
      conceptoPago: 'Recursos propios',
      valorConceptoPago: 'Recursos propios'
    },
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      uso: 'Diseño',
      tipoPago: '$12.000.000',
      conceptoPago: 'Recursos propios',
      valorConceptoPago: 'Recursos propios'
    }
  ]

  listaDescuentos = [
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      uso: 'Diseño',
      tipoPago: '$12.000.000',
      conceptoPago: 'Recursos propios',
      valorConceptoPago: 'Recursos propios'
    },
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      uso: 'Diseño',
      tipoPago: '$12.000.000',
      conceptoPago: 'Recursos propios',
      valorConceptoPago: 'Recursos propios'
    }
  ]

  listaDetalleOtrosDescuentos = [
    {
      concepto: 'Demolición',
      descuento: '$45.000.000',
      valor: 'Diseño'
    }
  ]
  
  constructor() { }

  ngOnInit(): void {
  }

}
