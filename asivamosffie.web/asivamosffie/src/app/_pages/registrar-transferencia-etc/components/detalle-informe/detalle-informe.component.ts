import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ProyectoEntregaETC } from 'src/app/_interfaces/proyecto-entrega-etc';

@Component({
  selector: 'app-detalle-informe',
  templateUrl: './detalle-informe.component.html',
  styleUrls: ['./detalle-informe.component.scss']
})
export class DetalleInformeComponent implements OnInit {

  id: number;
  numeroContratoObra: string;
  numeroContratoInterventoria: string; 
  llaveMen: string;
  proyectoEntregaEtc: ProyectoEntregaETC;

  constructor(
    private route: ActivatedRoute,
    private registerProjectETCService: RegisterProjectEtcService
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getProyectoEntregaETCByInformeFinalId(this.id);
    });
  }

  ngOnInit(): void {
    //this.getProyectoEntregaETCByInformeFinalId(this.id);
  }
  

  getProyectoEntregaETCByInformeFinalId(id: number){
    this.registerProjectETCService.getProyectoEntregaETCByInformeFinalId(id)
    .subscribe(report => {
      this.llaveMen = report[0].llaveMen;
      this.numeroContratoObra = report[0].numeroContratoObra;
      this.numeroContratoInterventoria = report[0].numeroContratoInterventoria;
      if ( report[0].proyectoEntregaEtc != null ) {
        this.proyectoEntregaEtc = report[0].proyectoEntregaEtc as ProyectoEntregaETC;
      }
    });
  }

  arrayOne(n: number): any[] {
    return Array(n);
  }

  descargarActa(url: string){
     window.open(url, "_blank");
  }
}
