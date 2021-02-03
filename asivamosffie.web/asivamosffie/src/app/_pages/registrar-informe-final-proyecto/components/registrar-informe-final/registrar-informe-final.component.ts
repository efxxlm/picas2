import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
@Component({
  selector: 'app-registrar-informe-final',
  templateUrl: './registrar-informe-final.component.html',
  styleUrls: ['./registrar-informe-final.component.scss']
})
export class RegistrarInformeFinalComponent implements OnInit {

  id: string;
  report: Report;
  estadoInforme: string;
  mostrarInforme: boolean;

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
  getInformeFinalByProyecto(id:string) {
    this.registrarInformeFinalProyectoService.getInformeFinalByProyecto(id)
    .subscribe(report => {
      this.report = report[0];
      if(report[0].proyecto.informeFinal.length>0){
        this.estadoInforme = report[0].proyecto.informeFinal[0].estadoInforme;
      }else{
        this.estadoInforme = '0';
      }
    });
  }
  
  updateForm(data) {
    if(data) this.mostrarInforme = data;
  }
}
