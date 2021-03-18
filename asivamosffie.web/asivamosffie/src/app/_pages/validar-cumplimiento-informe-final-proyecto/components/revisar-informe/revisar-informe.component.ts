import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-revisar-informe',
  templateUrl: './revisar-informe.component.html',
  styleUrls: ['./revisar-informe.component.scss']
})
export class RevisarInformeComponent implements OnInit {

  id: string;
  report: Report;
  existeObservacionCumplimiento= false;

  constructor(
    private route: ActivatedRoute,
    private validarCumplimientoInformeFinalProyectoService: ValidarCumplimientoInformeFinalService,
    public dialog: MatDialog,
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
      if(report[0].proyecto.informeFinal[0].historialObsInformeFinalNovedades.length >0){
        this.existeObservacionCumplimiento = true;
      }
    });
  }

}
