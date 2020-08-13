import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ContratacionProyecto, Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';

@Component({
  selector: 'app-expansion-panel-detallar-solicitud',
  templateUrl: './expansion-panel-detallar-solicitud.component.html',
  styleUrls: ['./expansion-panel-detallar-solicitud.component.scss']
})
export class ExpansionPanelDetallarSolicitudComponent implements OnInit {

  contratacion: Contratacion = {};

  constructor(
        private route: ActivatedRoute,
        private projectContractingService: ProjectContractingService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.contratacion.contratacionId = params.id;

      this.projectContractingService.getListContratacionProyectoByContratacionId( this.contratacion.contratacionId )
        .subscribe( response => {
            this.contratacion.contratacionProyecto = [];
            this.contratacion.contratacionProyecto = response;
            console.log( response );
      })

    });
  }

}
