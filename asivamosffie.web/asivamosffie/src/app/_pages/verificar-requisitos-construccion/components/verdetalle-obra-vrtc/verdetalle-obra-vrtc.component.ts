import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-verdetalle-obra-vrtc',
  templateUrl: './verdetalle-obra-vrtc.component.html',
  styleUrls: ['./verdetalle-obra-vrtc.component.scss']
})
export class VerdetalleObraVrtcComponent implements OnInit {

  contrato: Contrato;
  fechaPoliza: string;

  constructor(
              private faseUnoConstruccionService: FaseUnoConstruccionService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
            ) 
  {
    this.getContrato();

    if (this.router.getCurrentNavigation().extras.state)
      this.fechaPoliza = this.router.getCurrentNavigation().extras.state.fechaPoliza;
  }

  getContrato () {
    this.faseUnoConstruccionService.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
    .subscribe( response => {
      this.contrato = response;
      console.log( this.contrato );
    } );
  };

  ngOnInit(): void {
  }

}
