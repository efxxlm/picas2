import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-ver-detalle-informe-final',
  templateUrl: './ver-detalle-informe-final.component.html',
  styleUrls: ['./ver-detalle-informe-final.component.scss']
})
export class VerDetalleInformeFinalComponent implements OnInit {

  id: string;
  report: Report;
  existeObservacionApoyo = false;
  existeObservacionSupervision = false;
  existeObservacionCumplimiento = false;

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
        this.existeObservacionApoyo = true;
      }
      if(report[0].proyecto.informeFinal[0].informeFinalObservacionesSupervisor.length > 0){
        this.existeObservacionSupervision = true;
      }
      if(report[0].proyecto.informeFinal[0].informeFinalObservacionesCumplimiento.length > 0){
        this.existeObservacionCumplimiento = true;
      }
    });
  }
  
}