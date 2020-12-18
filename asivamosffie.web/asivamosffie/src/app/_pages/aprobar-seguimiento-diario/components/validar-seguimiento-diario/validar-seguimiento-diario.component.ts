import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { SeguimientoDiario } from 'src/app/_interfaces/DailyFollowUp';

@Component({
  selector: 'app-validar-seguimiento-diario',
  templateUrl: './validar-seguimiento-diario.component.html',
  styleUrls: ['./validar-seguimiento-diario.component.scss']
})
export class ValidarSeguimientoDiarioComponent implements OnInit {

  seguimientoId?: number;
  seguimiento?: SeguimientoDiario;
  proyecto: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private dailyFollowUpService: FollowUpDailyService,
  ) 
  {
    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/aprobarSeguimientoDiario');
      return;
    };

    if (this.router.getCurrentNavigation().extras.state)
      this.proyecto = this.router.getCurrentNavigation().extras.state.proyecto;
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId);
      if (this.seguimientoId > 0){
        this.dailyFollowUpService.getDailyFollowUpById( this.seguimientoId )
          .subscribe( respuesta => {
            this.seguimiento = respuesta
          
      });
      }
    });
  }

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

}
