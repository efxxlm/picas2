import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-validar-requerimientos-liquidaciones',
  templateUrl: './validar-requerimientos-liquidaciones.component.html',
  styleUrls: ['./validar-requerimientos-liquidaciones.component.scss']
})
export class ValidarRequerimientosLiquidacionesComponent implements OnInit {

  semaforoInformeFinal = 'Incompleto'
  contratacionProyectoId: number;
  constructor(
    private route: ActivatedRoute,
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.contratacionProyectoId = params.id;
    });
  }


  ngOnInit(): void {
  }

  valueSemaforoInformeFinal(event: any){
    this.semaforoInformeFinal = event;
  }
}
