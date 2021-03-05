import { Component, Input } from '@angular/core';

import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-detalle-recibo-satisfaccion',
  templateUrl: './detalle-recibo-satisfaccion.component.html',
  styleUrls: ['./detalle-recibo-satisfaccion.component.scss']
})
export class DetalleReciboSatisfaccionComponent {

  @Input() report: Report

}
