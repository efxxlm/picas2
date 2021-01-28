import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { SeguimientoDiario } from 'src/app/_interfaces/DailyFollowUp';

@Component({
  selector: 'app-ver-detalle-registro',
  templateUrl: './ver-detalle-registro.component.html',
  styleUrls: ['./ver-detalle-registro.component.scss']
})
export class VerDetalleRegistroComponent implements OnInit {

  seguimientoId?: number;
  seguimiento?: SeguimientoDiario;
  proyecto: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
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
      console.log(this.seguimientoId, this.router.getCurrentNavigation());
      if (this.seguimientoId > 0){
        this.dailyFollowUpService.getDailyFollowUpById( this.seguimientoId )
          .subscribe( respuesta => {
            this.seguimiento = respuesta
          
      });
      }
    });
  }

  volver(){
    this.router.navigate(['/registroSeguimientoDiario/verBitacora', this.seguimiento.contratacionProyectoId]);
  }

  innerObservacion ( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return `<b>${ observacionHtml }</b>`;
    };
  };

}
