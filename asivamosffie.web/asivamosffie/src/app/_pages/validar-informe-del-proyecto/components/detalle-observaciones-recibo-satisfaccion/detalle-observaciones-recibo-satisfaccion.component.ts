import { Component, Input, OnInit } from '@angular/core';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-detalle-observaciones-recibo-satisfaccion',
  templateUrl: './detalle-observaciones-recibo-satisfaccion.component.html',
  styleUrls: ['./detalle-observaciones-recibo-satisfaccion.component.scss']
})
export class DetalleObservacionesReciboSatisfaccionComponent implements OnInit {

  @Input() report: Report
  constructor() { }

  ngOnInit(): void {
  }

}
