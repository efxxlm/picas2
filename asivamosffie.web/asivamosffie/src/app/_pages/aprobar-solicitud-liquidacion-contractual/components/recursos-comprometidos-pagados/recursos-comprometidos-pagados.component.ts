import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-recursos-comprometidos-pagados',
  templateUrl: './recursos-comprometidos-pagados.component.html',
  styleUrls: ['./recursos-comprometidos-pagados.component.scss']
})
export class RecursosComprometidosPagadosComponent implements OnInit {

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
