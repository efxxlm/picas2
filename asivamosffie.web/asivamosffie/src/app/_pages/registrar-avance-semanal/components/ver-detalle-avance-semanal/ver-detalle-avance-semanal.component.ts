import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-ver-detalle-avance-semanal',
  templateUrl: './ver-detalle-avance-semanal.component.html',
  styleUrls: ['./ver-detalle-avance-semanal.component.scss']
})
export class VerDetalleAvanceSemanalComponent implements OnInit {

    constructor(
      private location: Location )
    { }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
    }

}
