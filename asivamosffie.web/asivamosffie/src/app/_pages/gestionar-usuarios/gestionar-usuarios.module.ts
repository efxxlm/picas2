import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarUsuariosRoutingModule } from './gestionar-usuarios-routing.module';
import { GestionarUsuariosComponent } from './components/gestionar-usuarios/gestionar-usuarios.component';
import { TablaGestionarUsuariosComponent } from './components/tabla-gestionar-usuarios/tabla-gestionar-usuarios.component';
import { FormGestionarUsuariosComponent } from './components/form-gestionar-usuarios/form-gestionar-usuarios.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';


@NgModule({
  declarations: [GestionarUsuariosComponent, TablaGestionarUsuariosComponent, FormGestionarUsuariosComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    GestionarUsuariosRoutingModule
  ]
})
export class GestionarUsuariosModule { }
