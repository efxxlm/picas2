import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-preparacion',
  templateUrl: './preparacion.component.html',
  styleUrls: ['./preparacion.component.scss']
})
export class PreparacionComponent implements OnInit {

  listaPerfil = [
    {
      perfil: 'Ingeniero de obra',
      hvRequeridas: '1',
      hvRecibidas: '10',
      hvAprobadas: '3',
      urlSoporte: 'http//H.VIngeniero.onedrive'
    }
  ];

  listaPlanes = [
    {
      planesProgramas: 'Licencia vigente',
      recibioRequisito: 'Sí',
      fechaRadicado: '20/07/2020',
      fechaAprobacion: '19/07/2020',
      observacionesRequisito: 'No'
    }
  ];

  listacontratoInterventoria = [
    {
      perfil: 'Ingeniero de obra',
      hvRequeridas: '1',
      hvRecibidas: '10',
      hvAprobadas: '3',
      urlSoporte: 'http//H.VIngeniero.onedrive'
    }
  ];

  constructor() { }

  ngOnInit(): void {
  }

}
