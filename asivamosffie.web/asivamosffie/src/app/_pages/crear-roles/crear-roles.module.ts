import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CrearRolesRoutingModule } from './crear-roles-routing.module';
import { CrearRolesComponent } from './components/crear-roles/crear-roles.component';
import { TablaCrearRolesComponent } from './components/tabla-crear-roles/tabla-crear-roles.component';
import { FormCrearRolesComponent } from './components/form-crear-roles/form-crear-roles.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaFaseInicioComponent } from './components/tabla-fase-inicio/tabla-fase-inicio.component';
import { TablaFaseSeguimientoComponent } from './components/tabla-fase-seguimiento/tabla-fase-seguimiento.component';
import { TablaFaseCierreComponent } from './components/tabla-fase-cierre/tabla-fase-cierre.component';


@NgModule({
  declarations: [CrearRolesComponent, TablaCrearRolesComponent, FormCrearRolesComponent, TablaFaseInicioComponent, TablaFaseSeguimientoComponent, TablaFaseCierreComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    CrearRolesRoutingModule
  ]
})
export class CrearRolesModule { }
