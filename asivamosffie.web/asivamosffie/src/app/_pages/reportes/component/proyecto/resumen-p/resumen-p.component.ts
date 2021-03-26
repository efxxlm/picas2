import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-resumen-p',
  templateUrl: './resumen-p.component.html',
  styleUrls: ['./resumen-p.component.scss']
})
export class ResumenPComponent implements OnInit {

  listaAlcance = [
    {
      infraestructura: 'Laboratorio de quimíca',
      cantidad: '4'
    }
  ];

  listaFuentesUsos = [
    {
      aportante: 'ACT-DDP 0001',
      valorAportante: 'Adición',
      fuente: '$5.000.000',
      uso: 'CT_0060',
      valorOso: '20/11/2020'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
