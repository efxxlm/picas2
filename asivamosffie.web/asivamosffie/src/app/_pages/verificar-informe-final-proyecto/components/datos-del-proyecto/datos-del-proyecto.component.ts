import { Component, Input } from '@angular/core';

import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-datos-del-proyecto',
  templateUrl: './datos-del-proyecto.component.html',
  styleUrls: ['./datos-del-proyecto.component.scss']
})
export class DatosDelProyectoComponent {

  @Input() report: Report

}
