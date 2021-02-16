import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';


@Component({
  selector: 'app-validar-informe',
  templateUrl: './validar-informe.component.html',
  styleUrls: ['./validar-informe.component.scss']
})
export class ValidarInformeComponent implements OnInit {

  id: string;
  report: Report;
  existeObservacion = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private validarInformeFinalProyectoService: ValidarInformeFinalService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getInformeFinalByProyecto(this.id);
    })
  }

  getInformeFinalByProyecto(id:string) {
    this.validarInformeFinalProyectoService.getInformeFinalByProyecto(id)
    .subscribe(report => {
      this.report = report[0];
      if(report[0].proyecto.informeFinal[0].informeFinalObservaciones.length > 0){
        this.existeObservacion = true;
      }
    });
  }
  
}