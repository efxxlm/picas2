import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-resumen',
  templateUrl: './resumen.component.html',
  styleUrls: ['./resumen.component.scss']
})
export class ResumenComponent implements OnInit {

  listaInversion = [
    {
      drp: '1',
      numeroDRP: 'IP_00090',
      valor: '$100.000.000'
    }
  ];

  listaFuentesUsos = [
    {
      aportante: 'Alcaldía de Susacón',
      valorAportante: '$45.000.000',
      fuente: 'Recursos propios',
      uso: 'Diseño',
      valorUso: '$12.000.000'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
