import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-aplazar-sesion',
  templateUrl: './aplazar-sesion.component.html',
  styleUrls: ['./aplazar-sesion.component.scss']
})
export class AplazarSesionComponent implements OnInit {

  fechaAplazamiento: FormControl;
  minDate: Date;

  constructor() {
    this.declararFechaAplazamiento();
    this.minDate = new Date();
  }

  ngOnInit(): void {
  }

  private declararFechaAplazamiento() {
    this.fechaAplazamiento = new FormControl(null, [Validators.required]);
  }

}
