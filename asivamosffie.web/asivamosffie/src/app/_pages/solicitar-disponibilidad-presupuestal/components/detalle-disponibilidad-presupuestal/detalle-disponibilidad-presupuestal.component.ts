import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-detalle-disponibilidad-presupuestal',
  templateUrl: './detalle-disponibilidad-presupuestal.component.html',
  styleUrls: ['./detalle-disponibilidad-presupuestal.component.scss']
})
export class DetalleDisponibilidadPresupuestalComponent implements OnInit {
  numeroSolicitud: any;
  objeto: any;
  observaciones: any;

  constructor(private activatedRoute: ActivatedRoute, private router: Router, private budgetAvailabilityService: DisponibilidadPresupuestalService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      console.log(param);
      this.cargarServicio(param.idTipoSolicitud);
    });
  }

  cargarServicio(id){
    this.budgetAvailabilityService.GetDisponibilidadPresupuestalByID(id).subscribe(data=>{
      this.numeroSolicitud = data.numeroSolicitud;
      this.objeto = data.objeto;
      this.observaciones = data.observaciones;
    });
  }
}
