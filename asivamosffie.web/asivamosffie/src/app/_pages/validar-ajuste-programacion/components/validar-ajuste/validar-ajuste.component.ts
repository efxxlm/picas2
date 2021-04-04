import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-validar-ajuste',
  templateUrl: './validar-ajuste.component.html',
  styleUrls: ['./validar-ajuste.component.scss']
})
export class ValidarAjusteComponent implements OnInit {

  detalleId: string;
  ajusteProgramacion: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private faseUnoConstruccionService: FaseUnoConstruccionService,
    
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      this.faseUnoConstruccionService.GetAjusteProgramacionById( this.detalleId )
        .subscribe( ajuste => {
          this.ajusteProgramacion = ajuste;
        });
    });
  }

}
