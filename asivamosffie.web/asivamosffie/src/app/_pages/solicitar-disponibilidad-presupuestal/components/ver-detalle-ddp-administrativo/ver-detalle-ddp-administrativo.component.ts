import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';

@Component({
  selector: 'app-ver-detalle-ddp-administrativo',
  templateUrl: './ver-detalle-ddp-administrativo.component.html',
  styleUrls: ['./ver-detalle-ddp-administrativo.component.scss']
})
export class VerDetalleDdpAdministrativoComponent implements OnInit {
  numeroSolicitud: string;
  objeto: string;
  idProyectoAdmin: number;

  constructor(private activatedRoute: ActivatedRoute, private budgetAvailabilityService: BudgetAvailabilityService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.cargarRegistro(param.id);
    });
  }

  cargarRegistro(id) {
    this.budgetAvailabilityService.getDisponibilidadPresupuestalById(id).subscribe(resp=>{
      this.numeroSolicitud = resp.numeroSolicitud;
      this.objeto = resp.objeto;
      this.idProyectoAdmin = resp.disponibilidadPresupuestalProyecto[0].proyectoAdministrativoId;
      this.budgetAvailabilityService.getAportantesByProyectoAdminId(resp.disponibilidadPresupuestalProyecto[0].proyectoAdministrativoId).subscribe(resp1=>{

      });
    });
  }
}
