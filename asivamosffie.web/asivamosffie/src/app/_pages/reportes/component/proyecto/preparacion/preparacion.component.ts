import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';

@Component({
  selector: 'app-preparacion',
  templateUrl: './preparacion.component.html',
  styleUrls: ['./preparacion.component.scss']
})
export class PreparacionComponent implements OnInit {

  proyectoId: number;
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
  dataPreConstruccionObra: any = null;
  dataPreConstruccionInterventoria: any = null;

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
      this.proyectoId = params.id;
    });
   }

  ngOnInit(): void {
    this.fichaProyectoService.getInfoPreparacionByProyectoId(this.proyectoId)
    .subscribe(response => {
      this.dataPreparacion = response;
      if(this.dataPreparacion != null){
        if(this.dataPreparacion?.preconstruccion?.result != null){
           this.dataPreConstruccionObra =  this.dataPreparacion?.preconstruccion?.result.find(r => r.tipoContratoCodigo == '1');
           this.dataPreConstruccionInterventoria =  this.dataPreparacion?.preconstruccion?.result.find(r => r.tipoContratoCodigo == '2');
        }
        console.log(this.dataPreConstruccionObra);
        console.log(this.dataPreConstruccionInterventoria);
      }
    });
  }

}
