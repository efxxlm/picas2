import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  verAyuda = false;
  contratoObra = false;
  contratoInterventoria = false;

  constructor() { }

  ngOnInit(): void {
  }

}
