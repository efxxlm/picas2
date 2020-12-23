import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-ver-detalle-interventoria',
  templateUrl: './ver-detalle-interventoria.component.html',
  styleUrls: ['./ver-detalle-interventoria.component.scss']
})
export class VerDetalleInterventoriaComponent implements OnInit {

  detalleId: string;

  listaObservaciones = [
    {
      fecha: '20/10/2020',
      responsable: 'Supervisor',
      historial: 'No es posible acceder a la ruta de soporte, verificar la URL.'
    }
  ]

  constructor(
    private router: Router,
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      console.log(this.detalleId);
    });
  }

}
