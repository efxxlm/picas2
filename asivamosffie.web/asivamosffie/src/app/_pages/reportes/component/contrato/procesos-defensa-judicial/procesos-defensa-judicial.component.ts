import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-procesos-defensa-judicial',
  templateUrl: './procesos-defensa-judicial.component.html',
  styleUrls: ['./procesos-defensa-judicial.component.scss']
})
export class ProcesosDefensaJudicialComponent implements OnInit {

  listaProcesosDefensaJudicial = [
    {
      fechaRegistroProceso: '10/12/2020',
      legitimacion: 'NOV_0001',
      tipoAccion: 'Prórroga',
      numeroProceso: 'Aprobada',
      estadoProceso: 'Aprobada',
      urlSoporte: 'http//:prórroga.onedrive'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
