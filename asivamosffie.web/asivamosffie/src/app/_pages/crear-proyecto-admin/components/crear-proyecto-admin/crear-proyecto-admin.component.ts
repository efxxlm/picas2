import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-crear-proyecto-admin',
  templateUrl: './crear-proyecto-admin.component.html',
  styleUrls: ['./crear-proyecto-admin.component.scss']
})
export class CrearProyectoAdminComponent {
  verAyuda = false; 

  constructor(private fb: FormBuilder) {}

}
