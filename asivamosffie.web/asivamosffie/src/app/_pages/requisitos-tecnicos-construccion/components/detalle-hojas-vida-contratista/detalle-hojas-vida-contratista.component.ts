import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle-hojas-vida-contratista',
  templateUrl: './detalle-hojas-vida-contratista.component.html',
  styleUrls: ['./detalle-hojas-vida-contratista.component.scss']
})
export class DetalleHojasVidaContratistaComponent implements OnInit {

  @Input() perfil: any;
  
  constructor() { }

  ngOnInit(): void {
  }

  innerObservacion ( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    };
  };

}
