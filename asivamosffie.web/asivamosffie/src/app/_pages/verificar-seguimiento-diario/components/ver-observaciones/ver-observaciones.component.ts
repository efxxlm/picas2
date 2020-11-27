import { Component, Input, OnInit } from '@angular/core';
import { SeguimientoDiarioObservaciones } from 'src/app/_interfaces/DailyFollowUp';

@Component({
  selector: 'app-ver-observaciones',
  templateUrl: './ver-observaciones.component.html',
  styleUrls: ['./ver-observaciones.component.scss']
})
export class VerObservacionesComponent implements OnInit {

  @Input() observacionObjeto: SeguimientoDiarioObservaciones;
  @Input() tieneObservaciones?: boolean;
  
  constructor() { }

  ngOnInit(): void {
  }

}
