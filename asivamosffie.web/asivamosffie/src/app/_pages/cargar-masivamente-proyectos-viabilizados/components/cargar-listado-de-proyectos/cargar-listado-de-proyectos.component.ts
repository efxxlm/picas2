import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';


@Component({
  selector: 'app-cargar-listado-de-proyectos',
  templateUrl: './cargar-listado-de-proyectos.component.html',
  styleUrls: ['./cargar-listado-de-proyectos.component.scss']
})
export class CargarListadoDeProyectosComponent implements OnInit {

  fileListaProyectos: FormControl;
  archivo: string;

  constructor() {
    this.declararInputFile();
  }

  ngOnInit(): void {
  }

  private declararInputFile() {
    this.fileListaProyectos = new FormControl('', [Validators.required]);
  }

  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  }

  enviarListaProyectos() {
    console.log(this.fileListaProyectos.value);
  }
}
