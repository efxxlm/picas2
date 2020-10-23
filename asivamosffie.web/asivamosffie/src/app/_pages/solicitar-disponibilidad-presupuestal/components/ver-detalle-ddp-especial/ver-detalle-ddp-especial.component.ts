import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-ver-detalle-ddp-especial',
  templateUrl: './ver-detalle-ddp-especial.component.html',
  styleUrls: ['./ver-detalle-ddp-especial.component.scss']
})
export class VerDetalleDdpEspecialComponent implements OnInit {
  aportantesList: any;
  numeroSolicitud: any;
  objeto: any;

  constructor(private activatedRoute: ActivatedRoute, private budgetAvailabilityService: DisponibilidadPresupuestalService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      console.log(param);
      this.cargarServicio(param.id);
    });
  }
  cargarServicio(id){
    this.budgetAvailabilityService.GetDetailAvailabilityBudgetProyect(id).subscribe(data=>{
      this.aportantesList = data[0].aportantes;
      this.numeroSolicitud = data[0].numeroSolicitud;
      this.objeto = data[0].objeto;
    });
  }
}
