import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarParametricasRoutingModule } from './gestionar-parametricas-routing.module';
import { GestionarParametricasComponent } from './components/gestionar-parametricas/gestionar-parametricas.component';
import { TablaGestionarParametricasComponent } from './components/tabla-gestionar-parametricas/tabla-gestionar-parametricas.component';
import { FormGestionarParametricasComponent } from './components/form-gestionar-parametricas/form-gestionar-parametricas.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { ConsultarEditarParametricasComponent } from './components/consultar-editar-parametricas/consultar-editar-parametricas.component';


@NgModule({
  declarations: [GestionarParametricasComponent, TablaGestionarParametricasComponent, FormGestionarParametricasComponent, ConsultarEditarParametricasComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    GestionarParametricasRoutingModule
  ]
})
export class GestionarParametricasModule { }
