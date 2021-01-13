import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-solicitud-novedad',
  templateUrl: './solicitud-novedad.component.html',
  styleUrls: ['./solicitud-novedad.component.scss']
})
export class SolicitudNovedadComponent implements OnInit {

  listaObservaciones = [
    {
      llaveMEN: 'LJ776554',
      departamento: 'Atlántico',
      municipio: 'Galapa',
      institucionEducativa: 'I.E. María Villa Campo',
      sede: 'Única sede'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
