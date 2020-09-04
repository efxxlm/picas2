import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-ejecucion-financiera',
  templateUrl: './form-ejecucion-financiera.component.html',
  styleUrls: ['./form-ejecucion-financiera.component.scss']
})
export class FormEjecucionFinancieraComponent implements OnInit {

  displayedColumns: string[] = [ 'componente', 'totalComprometido', 'ordenadoGiroImpuestos', 'saldo', 'porcentajeEjecucionFinanciera' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Componente', name: 'componente' },
    { titulo: 'Total comprometido', name: 'totalComprometido' },
    { titulo: 'Ordenado a girar antes de impuestos', name: 'ordenadoGiroImpuestos' },
    { titulo: 'Saldo', name: 'saldo' },
    { titulo: '% de ejecución Componente financiera', name: 'porcentajeEjecucionFinanciera' }
  ];

  displayedColumns1: string[] = [ 'componente', 'totalComprometido', 'facturadoAntesImpuestos', 'saldo', 'porcentajeEjecucionPresupuestal' ];
  ELEMENT_DATA1    : any[]    = [
    { titulo: 'Componente', name: 'componente' },
    { titulo: 'Total comprometido', name: 'totalComprometido' },
    { titulo: 'Facturado antes de impuestos', name: 'facturadoAntesImpuestos' },
    { titulo: 'Saldo', name: 'saldo' },
    { titulo: '% de ejecución componente presupuestal', name: 'porcentajeEjecucionPresupuestal' }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
