import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-cargar-masivamente-proyectos-viabilizados',
  templateUrl: './cargar-masivamente-proyectos-viabilizados.component.html',
  styleUrls: ['./cargar-masivamente-proyectos-viabilizados.component.scss']
})
export class CargarMasivamenteProyectosViabilizadosComponent implements OnInit {

  verAyuda = false;

  constructor() { }

  ngOnInit(): void {
  }
  descargaPlantilla()
  {
    location.href ="./assets/files/Formato_Cargue masivo de proyectos_FFIE.xlsx";
  }
}
