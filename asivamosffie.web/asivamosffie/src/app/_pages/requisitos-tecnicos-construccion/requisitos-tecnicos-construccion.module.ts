import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RequisitosTecnicosConstruccionRoutingModule } from './requisitos-tecnicos-construccion-routing.module';
import { RequisitosTecnicosConstruccionComponent } from './components/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.component';
import { TablaRequisitosTecnicosComponent } from './components/tabla-requisitos-tecnicos/tabla-requisitos-tecnicos.component';
import { MaterialModule } from '../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FormRequisitosTecnicosConstruccionComponent } from './components/form-requisitos-tecnicos-construccion/form-requisitos-tecnicos-construccion.component';
import { QuillModule } from 'ngx-quill';
import { DiagnosticoComponent } from './components/diagnostico/diagnostico.component';

@NgModule({
  declarations: [
    RequisitosTecnicosConstruccionComponent, 
    TablaRequisitosTecnicosComponent, FormRequisitosTecnicosConstruccionComponent, DiagnosticoComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    FormsModule,
    RequisitosTecnicosConstruccionRoutingModule
  ]
})
export class RequisitosTecnicosConstruccionModule { }