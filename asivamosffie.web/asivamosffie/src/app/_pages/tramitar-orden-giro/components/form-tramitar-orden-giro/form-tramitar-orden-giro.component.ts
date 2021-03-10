import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';

@Component({
  selector: 'app-form-tramitar-orden-giro',
  templateUrl: './form-tramitar-orden-giro.component.html',
  styleUrls: ['./form-tramitar-orden-giro.component.scss']
})
export class FormTramitarOrdenGiroComponent implements OnInit {

    esRegistroNuevo = false;
    esVerDetalle = false;
    esExpensas = false;

    constructor( private activatedRoute: ActivatedRoute )
    {
      // Verificar si es registro nuevo o ver detalle/editar o ver detalle
      this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
        if ( urlSegment.path === 'tramitarOrdenGiro' ) {
          this.esRegistroNuevo = true;
        }
        if ( urlSegment.path === 'verDetalle' ) {
          this.esVerDetalle = true;
        }
        if ( urlSegment.path === 'tramitarOrdenGiroExpensas' || urlSegment.path === 'editarOrdenGiroExpensas' ) {
          this.esExpensas = true;
        }
        if ( urlSegment.path === 'verDetalleExpensas' ) {
            this.esExpensas = true;
            this.esVerDetalle = true;
        }
      } );
    }

    ngOnInit(): void {
    }

}
