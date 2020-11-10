import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { DisponibilidadPresupuestal } from 'src/app/_interfaces/budgetAvailability';

@Component({
  selector: 'app-ver-detalle-ddp-administrativo',
  templateUrl: './ver-detalle-ddp-administrativo.component.html',
  styleUrls: ['./ver-detalle-ddp-administrativo.component.scss']
})
export class VerDetalleDdpAdministrativoComponent implements OnInit {
  numeroSolicitud: string;
  objeto: string;
  idProyectoAdmin: number;
  objetoDisponibilidad: DisponibilidadPresupuestal = {};
  aportantes: any;
  constructor(private activatedRoute: ActivatedRoute, private budgetAvailabilityService: DisponibilidadPresupuestalService,
    private projectContractingService: ProjectContractingService,
    private projectService: ProjectService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.cargarRegistro(param.id);
    });
  }

  cargarRegistro(id) {
    this.budgetAvailabilityService.GetDetailAvailabilityBudgetProyect(id).subscribe(resp=>{
      this.numeroSolicitud = resp[0].numeroSolicitud;
      this.objeto = resp[0].objeto;
      this.idProyectoAdmin = resp[0].proyectos[0].llaveMen;
      this.aportantes=resp[0].aportantes;
      /*this.budgetAvailabilityService.getAportantesByProyectoAdminId(resp.disponibilidadPresupuestalProyecto[0].proyectoAdministrativoId).subscribe(resp1=>{

      });*/
    });
  }
}
