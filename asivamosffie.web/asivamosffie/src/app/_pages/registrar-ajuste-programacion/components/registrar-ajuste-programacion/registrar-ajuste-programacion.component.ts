import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-registrar-ajuste-programacion',
  templateUrl: './registrar-ajuste-programacion.component.html',
  styleUrls: ['./registrar-ajuste-programacion.component.scss']
})
export class RegistrarAjusteProgramacionComponent implements OnInit {

  detalleId: string;
  ajusteProgramacionInfo: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute
  )
  {
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
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      console.log(this.detalleId);
      console.log(this.ajusteProgramacionInfo);
    });
  }

}
