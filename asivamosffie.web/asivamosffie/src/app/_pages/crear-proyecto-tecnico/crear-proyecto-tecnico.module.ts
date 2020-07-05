import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CrearProyectoTecnicoRoutingModule } from './crear-proyecto-tecnico-routing.module';
import { CrearProyectoTenicoComponent } from './components/crear-proyecto-tenico/crear-proyecto-tenico.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
import { ReactiveFormsModule } from '@angular/forms';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';



@NgModule({
  declarations: [CrearProyectoTenicoComponent, FormularioProyectosComponent],
  imports: [
    CommonModule,
    CrearProyectoTecnicoRoutingModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    ReactiveFormsModule
  ]
})
export class CrearProyectoTecnicoModule { }
