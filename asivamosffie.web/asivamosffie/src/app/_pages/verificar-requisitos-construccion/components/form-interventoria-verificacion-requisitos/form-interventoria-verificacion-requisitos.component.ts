import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-interventoria-verificacion-requisitos',
  templateUrl: './form-interventoria-verificacion-requisitos.component.html',
  styleUrls: ['./form-interventoria-verificacion-requisitos.component.scss']
})
export class FormInterventoriaVerificacionRequisitosComponent implements OnInit {

  contrato: Contrato;

  constructor(
                private faseUnoConstruccionService: FaseUnoConstruccionService,
                private activatedRoute: ActivatedRoute,
             ) 
  { 
    this.getContrato();
  }

  ngOnInit(): void {
  }

  getContrato () {
    this.faseUnoConstruccionService.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
    .subscribe( response => {
      this.contrato = response;
      console.log( this.contrato );
    } );
  };

}
