import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-avance-fisico-financiero',
  templateUrl: './avance-fisico-financiero.component.html',
  styleUrls: ['./avance-fisico-financiero.component.scss']
})
export class AvanceFisicoFinancieroComponent implements OnInit {

  @Input() esVerDetalle = false;
  @Input() seguimientoDiario: any;
  semaforoAvanceFisico = 'sin-diligenciar';

  constructor() { }

  ngOnInit(): void {
    if ( this.seguimientoDiario !== undefined ) {
      if ( this.seguimientoDiario.seguimientoSemanalAvanceFisico.length > 0 ) {
        const avanceFisico = this.seguimientoDiario.seguimientoSemanalAvanceFisico[0];
        if ( avanceFisico.registroCompleto === false ) {
          this.semaforoAvanceFisico = 'en-proceso';
        }
        if ( avanceFisico.registroCompleto === true ) {
          this.semaforoAvanceFisico = 'completo';
        }
      }
    }
  }

}
