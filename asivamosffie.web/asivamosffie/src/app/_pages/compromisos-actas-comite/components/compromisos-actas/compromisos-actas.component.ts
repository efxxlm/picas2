import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-compromisos-actas',
  templateUrl: './compromisos-actas.component.html',
  styleUrls: ['./compromisos-actas.component.scss']
})
export class CompromisosActasComponent implements OnInit {

  verAyuda = false;
  gestionCompromisos = false;
  gestionActas = false;

  constructor() { }

  ngOnInit(): void {
  }

}
