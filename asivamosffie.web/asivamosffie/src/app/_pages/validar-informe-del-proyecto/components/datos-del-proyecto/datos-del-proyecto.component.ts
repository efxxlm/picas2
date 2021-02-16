import { Component, Input, OnInit } from '@angular/core';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-datos-del-proyecto',
  templateUrl: './datos-del-proyecto.component.html',
  styleUrls: ['./datos-del-proyecto.component.scss']
})
export class DatosDelProyectoComponent implements OnInit {

  @Input() report: Report
  @Input() existeObservacion: boolean

  constructor() { }

  ngOnInit(): void {
  }

}
