import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CrearProyectoAdminRoutingModule } from './crear-proyecto-admin-routing.module';
import { CrearProyectoAdminComponent } from './components/crear-proyecto-admin/crear-proyecto-admin.component';
import { FormularioProyectosComponent } from './components/formulario-proyectos/formulario-proyectos.component';
import { TablaProyectosAdminComponent } from './components/tabla-proyectos-admin/tabla-proyectos-admin.component';
import { MaterialModule } from 'src/app/material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';


@NgModule({
  declarations: [CrearProyectoAdminComponent, TablaProyectosAdminComponent, FormularioProyectosComponent, TablaProyectosAdminComponent],
  imports: [
    CommonModule,
    CrearProyectoAdminRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class CrearProyectoAdminModule { }
