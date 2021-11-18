import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-ver-historial',
  templateUrl: './ver-historial.component.html',
  styleUrls: ['./ver-historial.component.scss']
})
export class VerHistorialComponent implements OnInit {
  detalleId: string;
  ajusteProgramacionInfo: any;
  constructor(
    private router: Router,
    private route: ActivatedRoute
  ) {
    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/registrarAjusteProgramacion');
      return;
    };

    if (this.router.getCurrentNavigation().extras.state){
      console.log( this.router.getCurrentNavigation().extras.state.ajusteProgramacion )
      this.ajusteProgramacionInfo = this.router.getCurrentNavigation().extras.state.ajusteProgramacion
    }
   }

  ngOnInit(): void {
  }

}
