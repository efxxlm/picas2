import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CrearProyectoTecnicoRoutingModule } from './crear-proyecto-tecnico-routing.module';
import { CrearProyectoTenicoComponent } from './components/crear-proyecto-tenico/crear-proyecto-tenico.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';
import { TablaProyectosTecnicoComponent } from './components/tabla-proyectos-tecnico/tabla-proyectos-tecnico.component';
import { MaterialModule } from 'src/app/material/material.module';



@NgModule({
  declarations: [CrearProyectoTenicoComponent, FormularioProyectosComponent, TablaProyectosTecnicoComponent],
  imports: [
    CommonModule,
    CrearProyectoTecnicoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule    
  ]
})
export class CrearProyectoTecnicoModule { }
