import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ComiteFiduciario } from '../../../../_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-registrar-sesion',
  templateUrl: './registrar-sesion.component.html',
  styleUrls: ['./registrar-sesion.component.scss']
})
export class RegistrarSesionComponent implements OnInit {

  registroSesion: ComiteFiduciario;

  estadoValidacion: string = 'completo';
  estadoOtrosTemas: string = 'completo';
  estadoProposiciones: string = 'sin-diligenciar';

  constructor( private routes: Router ) {
    this.getDataRegistro();
    console.log( this.registroSesion );
  }

  ngOnInit(): void {
  };

  getDataRegistro () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl( '/comiteFiduciario/sesionesConvocadas' );
      return;
    };
    this.registroSesion = this.routes.getCurrentNavigation().extras.state.sesion;
  }

}