import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrarAjusteProgramacionRoutingModule } from './registrar-ajuste-programacion-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { HomeComponent } from './components/home/home.component';
import { TablaAjusteProgramacionComponent } from './components/tabla-ajuste-programacion/tabla-ajuste-programacion.component';
import { RegistrarAjusteProgramacionComponent } from './components/registrar-ajuste-programacion/registrar-ajuste-programacion.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { ProgramacionDeObraComponent } from './components/programacion-de-obra/programacion-de-obra.component';
import { FlujoIntervencionRecursosComponent } from './components/flujo-intervencion-recursos/flujo-intervencion-recursos.component';
import { CargarProgramacionComponent } from './components/cargar-programacion/cargar-programacion.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { DialogValidacionRegistroComponent } from './components/dialog-validacion-registro/dialog-validacion-registro.component';
import { CargarFlujoComponent } from './components/cargar-flujo/cargar-flujo.component';


@NgModule({
  declarations: [
    HomeComponent,
    TablaAjusteProgramacionComponent,
    RegistrarAjusteProgramacionComponent,
    ExpansionPanelComponent,
    ProgramacionDeObraComponent,
    FlujoIntervencionRecursosComponent,
    CargarProgramacionComponent,
    DialogObservacionesComponent,
    DialogValidacionRegistroComponent,
    CargarFlujoComponent
  ],
  imports: [
    CommonModule,
    RegistrarAjusteProgramacionRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule
  ]
})
export class RegistrarAjusteProgramacionModule { }
