import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-recursos-pagados',
  templateUrl: './tabla-recursos-pagados.component.html',
  styleUrls: ['./tabla-recursos-pagados.component.scss']
})
export class TablaRecursosPagadosComponent implements OnInit {

  dataAportantes = new MatTableDataSource();
  displayedColumns: string[] = [ 'nombre', 'valorFacturado', 'uso', 'tipoPago', 'conceptoPago', 'valorConceptoPago' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Nombre del aportante', name: 'nombre'},
    { titulo: 'Valor facturado', name: 'valorFacturado' },
    { titulo: 'Uso', name: 'uso' },
    { titulo: 'Tipo de pago', name: 'tipoPago' },
    { titulo: 'Concepto de pago', name: 'conceptoPago' },
    { titulo: 'Valor concepto de pago', name: 'valorConceptoPago' }
  ];
  dataAportantes1 = new MatTableDataSource();
  displayedColumns1: string[] = [ 'nombre', 'conceptoPago', 'ansAplicado', 'garantiaPagar', 'otrosDescuentos', 'totalDescuentos' ];
  ELEMENT_DATA1: any[] = [
    {titulo: 'Nombre del aportante', name: 'nombre'},
    { titulo: 'Concepto de pago', name: 'conceptoPago' },
    { titulo: 'ANS aplicado', name: 'ansAplicado' },
    { titulo: 'Rete garantía a pagar', name: 'garantiaPagar' },
    { titulo: 'Otros descuentos', name: 'otrosDescuentos' },
    { titulo: 'Valor total descuentos', name: 'totalDescuentos' },
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
