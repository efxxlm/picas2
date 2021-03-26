import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-controversias',
  templateUrl: './controversias.component.html',
  styleUrls: ['./controversias.component.scss']
})
export class ControversiasComponent implements OnInit {

  listaControversias = [
    {
      fechaSolicitudControversia: '10/12/2020',
      numeroSolicitud: 'NOV_0001',
      tipoControversia: 'Prórroga',
      estadoControversia: 'Aprobada',
      urlSoporte: 'http//:prórroga.onedrive'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
