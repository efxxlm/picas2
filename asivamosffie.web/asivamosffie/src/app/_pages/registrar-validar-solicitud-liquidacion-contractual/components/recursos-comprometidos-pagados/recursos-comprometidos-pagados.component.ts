import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-recursos-comprometidos-pagados',
  templateUrl: './recursos-comprometidos-pagados.component.html',
  styleUrls: ['./recursos-comprometidos-pagados.component.scss']
})
export class RecursosComprometidosPagadosComponent implements OnInit {

  listaAportantes = [
    {
      nombre: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      total: '$105.000.000'
    },
    {
      nombre: 'Gobernación de Boyacá',
      valor: '$40.000.000',
      total: '$105.000.000'
    },
    {
      nombre: 'FFIE',
      valor: '$20.000.000',
      total: '$105.000.000'
    }
  ]

  listaFuentesUsos = [
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      fuente: 'Recursos propios',
      uso: 'Diseño',
      valorUso: '$12.000.000'
    },
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      fuente: 'Contingencias',
      uso: 'Diseño',
      valorUso: '$12.000.000'
    },
    {
      aportante: 'Alcaldía de Susacón',
      valor: '$45.000.000',
      fuente: 'Contingencias',
      uso: 'Obra principal',
      valorUso: '$40.000.000'
    },
    {
      aportante: 'Gobernación de Boyacá',
      valor: '$40.000.000',
      fuente: 'Recursos propios',
      uso: 'Obra principal',
      valorUso: '$12.000.000'
    },
    {
      aportante: 'FFIE',
      valor: '$20.000.000',
      fuente: 'Contingencias',
      uso: 'Obra principal',
      valorUso: '$20.000.000'
    }
  ]

  listaValorTotalOrdenesGiro = [
    {
      id: '1',
      numeroOrdenGiro: 'ODG_Obra_001',
      contratista: 'Construir futuro',
      facturado: '$67.000.000',
      adn: '$500.000',
      reteGarantia: '$3.500.000',
      otrosDescuentos: '$1.110.000',
      pagarAntesImpuestos: '$61.000.000',
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
