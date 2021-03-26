import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contratacion-p',
  templateUrl: './contratacion-p.component.html',
  styleUrls: ['./contratacion-p.component.scss']
})
export class ContratacionPComponent implements OnInit {

  listaProyectosAsociados = [
    {
      numeroContrato: 'N801801',
      llaveMEN: 'LL457326',
      tipoIntervension: 'Remodelación',
      departamento: 'Boyacá',
      municipio: 'Susacón',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen',
      sede: 'Única sede'
    }
  ];

  listaNovedades = [
    {
      fechaSolicitudNovedad: '10/12/2020',
      numeroSolicitud: 'NOV_0001',
      tipoNovedad: 'Prórroga',
      estadoNovedad: 'Aprobada',
      urlSoporte: 'http//:prórroga.onedrive'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
