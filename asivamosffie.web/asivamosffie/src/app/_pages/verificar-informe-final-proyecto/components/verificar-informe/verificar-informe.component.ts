import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { VerificarInformeFinalService } from 'src/app/core/_services/verificarInformeFinal/verificar-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-verificar-informe',
  templateUrl: './verificar-informe.component.html',
  styleUrls: ['./verificar-informe.component.scss']
})
export class VerificarInformeComponent implements OnInit {

  id: string;
  report: Report;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private validarInformeFinalProyectoService: VerificarInformeFinalService
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
    });
  }

}
