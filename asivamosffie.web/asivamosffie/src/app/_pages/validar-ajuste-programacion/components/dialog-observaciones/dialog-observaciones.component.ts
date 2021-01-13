import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dialog-observaciones',
  templateUrl: './dialog-observaciones.component.html',
  styleUrls: ['./dialog-observaciones.component.scss']
})
export class DialogObservacionesComponent implements OnInit {

  listaObservaciones = [
    {
      fecha: '25/10/2020',
      historial: 'EN las actividades de la semana 1 no concuerda con la actividad de seguimiento de semana 2'
    },
    {
      fecha: '20/10/2020',
      historial: 'El archivo presenta un error, cargue de nuevo para validarlo'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
