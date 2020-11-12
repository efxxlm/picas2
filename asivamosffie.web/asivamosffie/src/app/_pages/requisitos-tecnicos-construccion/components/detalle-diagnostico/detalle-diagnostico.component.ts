import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle-diagnostico',
  templateUrl: './detalle-diagnostico.component.html',
  styleUrls: ['./detalle-diagnostico.component.scss']
})
export class DetalleDiagnosticoComponent implements OnInit {

  @Input() construccion: any;

  constructor() { }


  ngOnInit(): void {
  }

}
