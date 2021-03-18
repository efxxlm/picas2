import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-actualizacion-poliza',
  templateUrl: './actualizacion-poliza.component.html',
  styleUrls: ['./actualizacion-poliza.component.scss']
})
export class ActualizacionPolizaComponent implements OnInit {

  listapolizas = [
    {
      poliza: 'Buen manejo y correcta inversión del anticipo',
      responsable: 'Andres Nikolai Montealegre Rojas'
    },
    {
      poliza: 'Garantía de estabilidad y calidad de la obra',
      responsable: 'Andres Nikolai Montealegre Rojas'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
