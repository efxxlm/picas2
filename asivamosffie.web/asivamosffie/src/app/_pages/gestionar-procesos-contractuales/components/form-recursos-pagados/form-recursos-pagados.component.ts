import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-recursos-pagados',
  templateUrl: './form-recursos-pagados.component.html',
  styleUrls: ['./form-recursos-pagados.component.scss']
})
export class FormRecursosPagadosComponent implements OnInit {

  displayedColumns: string[] = [ 'nombre', 'conceptoPago', 'descuentoCuatroPorMil', 'retencion', 'totalOtrosDescuentos' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Nombre del aportante', name: 'nombre' },
    { titulo: 'Concepto de pago', name: 'conceptoPago' },
    { titulo: 'Descuento 4*1000', name: 'descuentoCuatroPorMil' },
    { titulo: 'Retención', name: 'retencion' },
    { titulo: 'Total otros descuentos', name: 'totalOtrosDescuentos' }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
