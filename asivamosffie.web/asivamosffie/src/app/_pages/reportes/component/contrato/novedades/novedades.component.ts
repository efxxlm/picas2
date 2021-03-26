import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-novedades',
  templateUrl: './novedades.component.html',
  styleUrls: ['./novedades.component.scss']
})
export class NovedadesComponent implements OnInit {

  listaNovedades = [
    {
      fechaSolicitudNovedad: '10/12/2020',
      numeroSolicitud: 'NOV_0001',
      tipoNovedad: 'Prórroga',
      estadoNovedad: 'Aprobada',
      urlSoporte: 'http//:prórroga.onedrive'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
