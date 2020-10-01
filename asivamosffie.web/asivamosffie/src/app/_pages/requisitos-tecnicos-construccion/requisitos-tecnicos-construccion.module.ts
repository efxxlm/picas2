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
import { PlanesProgramasComponent } from './components/planes-programas/planes-programas.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { ManejoAnticipoComponent } from './components/manejo-anticipo/manejo-anticipo.component';
import { HojaVidaContratistaComponent } from './components/hoja-vida-contratista/hoja-vida-contratista.component';
import { DialogCargarProgramacionComponent } from './components/dialog-cargar-programacion/dialog-cargar-programacion.component';
import { TablaProgramacionObraComponent } from './components/tabla-programacion-obra/tabla-programacion-obra.component';
import { ProgramacionObraFlujoInversionComponent } from './components/programacion-obra-flujo-inversion/programacion-obra-flujo-inversion.component';

@NgModule({
  declarations: [
    RequisitosTecnicosConstruccionComponent, 
    TablaRequisitosTecnicosComponent, FormRequisitosTecnicosConstruccionComponent, DiagnosticoComponent, PlanesProgramasComponent, DialogObservacionesComponent, ManejoAnticipoComponent, HojaVidaContratistaComponent, DialogCargarProgramacionComponent, TablaProgramacionObraComponent, ProgramacionObraFlujoInversionComponent
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