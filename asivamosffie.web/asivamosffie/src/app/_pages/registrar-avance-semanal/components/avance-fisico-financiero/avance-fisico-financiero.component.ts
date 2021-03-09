import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-avance-fisico-financiero',
  templateUrl: './avance-fisico-financiero.component.html',
  styleUrls: ['./avance-fisico-financiero.component.scss']
})
export class AvanceFisicoFinancieroComponent implements OnInit {

  @Input() esVerDetalle = false;
  @Input() seguimientoSemanal: any;
  @Input() avanceFisicoObs: string;
  semaforoAvanceFisico = 'sin-diligenciar';
  sinRegistros = false;

  constructor() { }

  ngOnInit(): void {
    if ( this.seguimientoSemanal !== undefined ) {
      if ( this.seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 ) {
        const avanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];
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
