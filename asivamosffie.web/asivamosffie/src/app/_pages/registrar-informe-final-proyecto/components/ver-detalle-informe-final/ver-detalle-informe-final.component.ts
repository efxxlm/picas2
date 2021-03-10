import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrarInformeFinal/registrar-informe-final-proyecto.service'
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-ver-detalle-informe-final',
  templateUrl: './ver-detalle-informe-final.component.html',
  styleUrls: ['./ver-detalle-informe-final.component.scss']
})
export class VerDetalleInformeFinalComponent implements OnInit {

  id: string;
  report: Report;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getInformeFinalByProyecto(this.id);
    })
  }
  getInformeFinalByProyecto (id:string) {
    this.registrarInformeFinalProyectoService.getInformeFinalByProyecto(id)
    .subscribe(report => {
      this.report = report[0];
      console.log(this.report);
      
    });
  }

}
