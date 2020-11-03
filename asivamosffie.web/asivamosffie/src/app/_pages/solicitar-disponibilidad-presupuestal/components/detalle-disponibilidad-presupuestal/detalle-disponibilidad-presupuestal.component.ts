import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';

import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ProjectService } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-detalle-disponibilidad-presupuestal',
  templateUrl: './detalle-disponibilidad-presupuestal.component.html',
  styleUrls: ['./detalle-disponibilidad-presupuestal.component.scss']
})
export class DetalleDisponibilidadPresupuestalComponent implements OnInit {
  numeroSolicitud: any;
  objeto: any;
  observaciones: any;
  aportantesList: any;
  tipoSolicitudColdigo: string;
  public listaProyectos;
  fechaComite: any;
  llaveMen: any;
  nameColegio: any;
  sede: string;
  departamento: string;
  municipio: string;
  plazoDias: number;
  plazoMeses: number;

  constructor(private activatedRoute: ActivatedRoute, private router: Router, private budgetAvailabilityService: BudgetAvailabilityService,
    private projectService: ProjectService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      console.log(param);
      this.cargarServicio1(param.idTipoSolicitud);
    });
  }

  cargarServicio1(id){
    this.budgetAvailabilityService.getDisponibilidadPresupuestalById(id).subscribe(data0=>{
      this.numeroSolicitud = data0.numeroSolicitud;
      this.tipoSolicitudColdigo = data0.tipoSolicitudCodigo;
      this.objeto = data0.objeto;
      this.plazoDias = data0.plazoDias;
      this.plazoMeses = data0.plazoMeses;
      this.fechaComite =data0.contratacionId&&data0.fechaComiteTecnicoNotMapped!='0001-01-01T00:00:00'?data0.fechaComiteTecnicoNotMapped:"";
      //this.fechaComite = data0.disponibilidadPresupuestalProyecto[0].proyecto['fechaComite'];
      this.cargarServicio2(data0.disponibilidadPresupuestalProyecto[0].proyectoId);
    });
  }

  cargarServicio2(id){
    this.projectService.getProjectById(id).subscribe(data1=>{
      this.llaveMen = data1.llaveMen;
      this.nameColegio = data1.institucionEducativa.nombre;
      this.sede = data1.sede.nombre;
      this.departamento = data1.departamento;
      this.municipio = data1.municipio;
      this.aportantesList = data1.proyectoAportante;
      console.log(this.aportantesList);
    });
  }

  cargarServicio3(id){
    
  }
}
