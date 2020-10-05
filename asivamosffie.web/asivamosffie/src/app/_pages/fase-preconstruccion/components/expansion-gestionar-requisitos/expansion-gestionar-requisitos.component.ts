import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';

@Component({
  selector: 'app-expansion-gestionar-requisitos',
  templateUrl: './expansion-gestionar-requisitos.component.html',
  styleUrls: ['./expansion-gestionar-requisitos.component.scss']
})
export class ExpansionGestionarRequisitosComponent implements OnInit {

  constructor ( private activatedRoute: ActivatedRoute,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id )
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId ( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( console.log );
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

}
