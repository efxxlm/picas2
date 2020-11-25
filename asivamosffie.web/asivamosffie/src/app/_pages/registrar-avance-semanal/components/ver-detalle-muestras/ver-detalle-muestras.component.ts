import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ver-detalle-muestras',
  templateUrl: './ver-detalle-muestras.component.html',
  styleUrls: ['./ver-detalle-muestras.component.scss']
})
export class VerDetalleMuestrasComponent implements OnInit {

    constructor(
      private location: Location )
    { }

    ngOnInit(): void {
    }

    getRutaAnterior() {
      this.location.back();
    }

}
