import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-crear-proyecto-tenico',
  templateUrl: './crear-proyecto-tenico.component.html',
  styleUrls: ['./crear-proyecto-tenico.component.scss']
})
export class CrearProyectoTenicoComponent {
  verAyuda = false; 

  constructor(private fb: FormBuilder) {}
}