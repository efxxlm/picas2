import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-aprobar-requisitos-construccion',
  templateUrl: './aprobar-requisitos-construccion.component.html',
  styleUrls: ['./aprobar-requisitos-construccion.component.scss']
})
export class AprobarRequisitosConstruccionComponent implements OnInit {

  verAyuda = false;
  contratoObra = false;
  contratoInterventoria = false;

  constructor() { }

  ngOnInit(): void {
  }

}
