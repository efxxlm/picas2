import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contratos-modificaciones-contractuales',
  templateUrl: './contratos-modificaciones-contractuales.component.html',
  styleUrls: ['./contratos-modificaciones-contractuales.component.scss']
})
export class ContratosModificacionesContractualesComponent implements OnInit {

  verAyuda = false;
  
  dataTable: any[] = [
    {
      fechaSolicitud: '19/06/2020',
      numeroSolicitud: 'PI_007',
      tipoSolicitud: 'Contratación',
      estadoRegistro: false,
      estadoDocumento: 'En revisión',
      id: 0
    },
    {
      fechaSolicitud: '20/05/2020',
      numeroSolicitud: '000003',
      tipoSolicitud: 'Modificación contractual',
      estadoRegistro: false,
      estadoDocumento: 'En revisión',
      id: 1
    },
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
