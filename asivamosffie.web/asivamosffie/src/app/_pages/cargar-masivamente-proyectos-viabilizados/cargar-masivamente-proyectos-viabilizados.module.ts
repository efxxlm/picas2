import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CargarMasivamenteProyectosViabilizadosRoutingModule } from './cargar-masivamente-proyectos-viabilizados-routing.module';
import { CargarMasivamenteProyectosViabilizadosComponent } from './components/cargar-masivamente-proyectos-viabilizados/cargar-masivamente-proyectos-viabilizados.component';
import { CargarListadoDeProyectosComponent } from './components/cargar-listado-de-proyectos/cargar-listado-de-proyectos.component';
import { TablaProyectosComponent } from './components/tabla-proyectos/tabla-proyectos.component';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [CargarMasivamenteProyectosViabilizadosComponent, CargarListadoDeProyectosComponent, TablaProyectosComponent],
  imports: [
    CommonModule,
    CargarMasivamenteProyectosViabilizadosRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
  ]
})
export class CargarMasivamenteProyectosViabilizadosModule { }
