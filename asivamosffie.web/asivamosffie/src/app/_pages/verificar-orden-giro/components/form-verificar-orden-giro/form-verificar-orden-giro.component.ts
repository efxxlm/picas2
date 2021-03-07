import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-verificar-orden-giro',
  templateUrl: './form-verificar-orden-giro.component.html',
  styleUrls: ['./form-verificar-orden-giro.component.scss']
})
export class FormVerificarOrdenGiroComponent implements OnInit {

  esRegistroNuevo = false;
  esVerDetalle = false;

  constructor( private activatedRoute: ActivatedRoute )
  {
    // Verificar si es registro nuevo o un ver detalle/editar
    this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'verificarOrdenGiro' ) {
        this.esRegistroNuevo = true;
      }
      if ( urlSegment.path === 'verDetalle' ) {
        this.esVerDetalle = true;
      }
    } );
  }

  ngOnInit(): void {
  }

}
