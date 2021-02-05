import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarInformeDelProyectoRoutingModule } from './validar-informe-del-proyecto-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaInformeFinalProyectoComponent } from './components/tabla-informe-final-proyecto/tabla-informe-final-proyecto.component';
import { ValidarInformeComponent } from './components/validar-informe/validar-informe.component';
import { DatosDelProyectoComponent } from './components/datos-del-proyecto/datos-del-proyecto.component';
import { TablaInformeFinalAnexosComponent } from './components/tabla-informe-final-anexos/tabla-informe-final-anexos.component';


@NgModule({
  declarations: [
    HomeComponent,
    TablaInformeFinalProyectoComponent,
    ValidarInformeComponent,
    DatosDelProyectoComponent,
    TablaInformeFinalAnexosComponent
  ],
  imports: [
    CommonModule,
    ValidarInformeDelProyectoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarInformeDelProyectoModule { }
