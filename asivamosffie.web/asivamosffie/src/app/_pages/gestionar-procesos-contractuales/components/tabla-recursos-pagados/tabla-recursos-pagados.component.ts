import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-recursos-pagados',
  templateUrl: './tabla-recursos-pagados.component.html',
  styleUrls: ['./tabla-recursos-pagados.component.scss']
})
export class TablaRecursosPagadosComponent implements OnInit {

  dataAportantes              = new MatTableDataSource();
  displayedColumns: string[]  = [ 'nombre', 'valorFacturado', 'uso', 'tipoPago', 'conceptoPago', 'valorConceptoPago' ];
  dataTable : any[] = [
    {
      nombre: 'FFIE',
      valorFacturado: '$126.000.000',
      uso: [ 'Obra complementaria', 'Obra' ],
      tipoPago: [ '100% fase 2', '100% fase 2' ],
      conceptoPago: [ 'Demolición', 'Renovación' ],
      valorConceptoPago: [ '$26.000.000', '$100.000.000' ]
    },
    {
      nombre: 'Gobernación del Valle Del Cauca',
      valorFacturado: '$100.000.000',
      uso: [ 'Obra complementaria', 'Obra' ],
      tipoPago: [ '100% fase 2', '100% fase 2' ],
      conceptoPago: [ 'Demolición', 'Renovación' ],
      valorConceptoPago: [ '$30.000.000', '$70.000.000' ]
    },
    {
      nombre: 'Fundación Pies Descalzos',
      valorFacturado: '$100.000.000',
      uso: [ 'Obra' ],
      tipoPago: [ '100% fase 2' ],
      conceptoPago: [ 'Renovación' ],
      valorConceptoPago: [ '$100.000.000' ]
    }
  ]
  dataAportantes1             = new MatTableDataSource();
  displayedColumns1: string[] = [ 'nombre', 'conceptoPago', 'ansAplicado', 'garantiaPagar', 'otrosDescuentos', 'totalDescuentos' ];
  dataTable1: any[] = [
    {
      nombre: 'FFIE',
      conceptoPago: [ 'Demolición', 'Renovación' ],
      ansAplicado: [ '$3.000.000', '$7.000.000' ],
      garantiaPagar: [ '0', '$500.000' ],
      otrosDescuentos: [ '0', '0' ],
      totalDescuentos: [ '$3.000.000', '$7.500.000' ]
    },
    {
      nombre: 'Gobernación del Valle Del Cauca',
      conceptoPago: [ 'Demolición', 'Renovación' ],
      ansAplicado: [ '$2.000.000', '$5.000.000' ],
      garantiaPagar: [ '0', '$8.000.000' ],
      otrosDescuentos: [ '0', '0' ],
      totalDescuentos: [ '$2.000.000', '$13.000.000' ]
    },
    {
      nombre: 'Fundación Pies Descalzos',
      conceptoPago: [ 'Renovación' ],
      ansAplicado: [ '$6.000.000' ],
      garantiaPagar: [ '$5.000.000' ],
      otrosDescuentos: [ '$10.000.000' ],
      totalDescuentos: [ '$21.000.000' ]
    }
  ]

  constructor() { }

  ngOnInit(): void {
    this.dataAportantes = new MatTableDataSource( this.dataTable );
    this.dataAportantes1 = new MatTableDataSource( this.dataTable1 );
  }

}
