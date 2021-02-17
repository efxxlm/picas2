import { Component, Input, OnInit } from '@angular/core';
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-verificacion-novedades',
  templateUrl: './verificacion-novedades.component.html',
  styleUrls: ['./verificacion-novedades.component.scss']
})
export class VerificacionNovedadesComponent implements OnInit {

  @Input() report: Report
  @Input() existeObservacionCumplimiento: boolean
  
  constructor() { }

  ngOnInit(): void {
  }

}
