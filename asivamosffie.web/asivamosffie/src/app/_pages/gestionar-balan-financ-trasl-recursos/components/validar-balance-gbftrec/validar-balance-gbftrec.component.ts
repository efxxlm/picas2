import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, UrlSegment } from '@angular/router';

@Component({
  selector: 'app-validar-balance-gbftrec',
  templateUrl: './validar-balance-gbftrec.component.html',
  styleUrls: ['./validar-balance-gbftrec.component.scss']
})
export class ValidarBalanceGbftrecComponent implements OnInit {
  id: string;
  opcion1 = false;
  opcion2 = false;
  opcion3 = false;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;
  constructor(
    private route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'validarBalance' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditarBalance' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalleBalance' ) {
          this.esVerDetalle = true;
          return;
      }
    });
  }

}
