import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-preparacion',
  templateUrl: './preparacion.component.html',
  styleUrls: ['./preparacion.component.scss']
})
export class PreparacionComponent implements OnInit {

  contratacionProyectoId: number;
  dataPreparacion: any = null;
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

  constructor(
    private fichaProyectoService: FichaProyectoService,
    private route: ActivatedRoute
  ) {
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
    });
   }

  ngOnInit(): void {
    this.fichaProyectoService.getInfoPreparacionByContratacionProyectoId(this.contratacionProyectoId)
    .subscribe(response => {
      this.dataPreparacion = response;
    });
  }

}
