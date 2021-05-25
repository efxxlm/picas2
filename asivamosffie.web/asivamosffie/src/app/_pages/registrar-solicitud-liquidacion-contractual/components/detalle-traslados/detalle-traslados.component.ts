import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle-traslados',
  templateUrl: './detalle-traslados.component.html',
  styleUrls: ['./detalle-traslados.component.scss']
})
export class DetalleTrasladosComponent implements OnInit {

    solicitudPago: any;
    esVerDetalle: any;
    lista = [
        {
          drp: '1',
          numeroDRP: 'IP_00090',
          valor: '$100.000.000',
          saldo: '$100.000.000'
        }
    ];

  listaSolicitud = [
    {
      tipoSolicitud: 'Obra',
      fechaAprobacionFiduciaria: 'I15/11/2020',
      fechaPagoFiduciaria: '22/11/2020',
      numeroOrdenGiro: 'ODG_Obra_222',
      modalidadContrato: 'Modalidad 1',
      numeroContrato: 'N801801'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
