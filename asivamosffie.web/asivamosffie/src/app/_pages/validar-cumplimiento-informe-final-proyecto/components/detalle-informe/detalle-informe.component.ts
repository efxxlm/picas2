import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-detalle-informe',
  templateUrl: './detalle-informe.component.html',
  styleUrls: ['./detalle-informe.component.scss']
})
export class DetalleInformeComponent implements OnInit {

  id: string;
  report: Report;
  existeObservacionCumplimiento = false;
  existeObservacionInterventoria = false;
  
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private validarCumplimientoInformeFinalProyectoService: ValidarCumplimientoInformeFinalService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getInformeFinalByProyecto(this.id);
    })
  }

  getInformeFinalByProyecto(id:string) {
    this.validarCumplimientoInformeFinalProyectoService.getInformeFinalByProyecto(id)
    .subscribe(report => {
      this.report = report[0];
      if(report[0].proyecto.informeFinal[0].informeFinalObservaciones.length > 0){
        this.existeObservacionCumplimiento = true;
      }
      if(report[0].proyecto.informeFinal[0].observacionVigenteInformeFinalInterventoriaNovedades != null){
        this.existeObservacionInterventoria = true;
      }
    });
  }

}
