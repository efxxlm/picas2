import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { SeguimientoDiario } from 'src/app/_interfaces/DailyFollowUp';

@Component({
  selector: 'app-verificar-seguimiento',
  templateUrl: './verificar-seguimiento.component.html',
  styleUrls: ['./verificar-seguimiento.component.scss']
})
export class VerificarSeguimientoComponent implements OnInit {

  seguimientoId?: number;
  seguimiento?: SeguimientoDiario;
  proyecto: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private dailyFollowUpService: FollowUpDailyService,
  ) 
  {
    if (this.router.getCurrentNavigation().extras.state)
      this.proyecto = this.router.getCurrentNavigation().extras.state.proyecto;
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.seguimientoId = params.id;
      console.log(this.seguimientoId);
    });
  }

}
