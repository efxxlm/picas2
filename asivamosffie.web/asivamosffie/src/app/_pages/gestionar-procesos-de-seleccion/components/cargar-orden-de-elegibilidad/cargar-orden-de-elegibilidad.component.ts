import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-cargar-orden-de-elegibilidad',
  templateUrl: './cargar-orden-de-elegibilidad.component.html',
  styleUrls: ['./cargar-orden-de-elegibilidad.component.scss']
})
export class CargarOrdenDeElegibilidadComponent {

  fileElegibilidad: FormControl;
  archivo: string;

  constructor() {
      this.declararInputFile();
    }

    private declararInputFile() {
      this.fileElegibilidad = new FormControl('', [Validators.required]);
    }

    fileName() {
      const inputNode: any = document.getElementById('file');
      this.archivo = inputNode.files[0].name;
    }

    onSubmit(): void {
      console.log(this.fileElegibilidad.value)
  }
}
