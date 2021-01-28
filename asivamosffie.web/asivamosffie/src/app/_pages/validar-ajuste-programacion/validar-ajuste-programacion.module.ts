import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarAjusteProgramacionRoutingModule } from './validar-ajuste-programacion-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaAjusteProgramacionComponent } from './components/tabla-ajuste-programacion/tabla-ajuste-programacion.component';
import { ValidarAjusteComponent } from './components/validar-ajuste/validar-ajuste.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { ProgramacionDeObraComponent } from './components/programacion-de-obra/programacion-de-obra.component';
import { FlujoIntervencionRecursosComponent } from './components/flujo-intervencion-recursos/flujo-intervencion-recursos.component';
import { HistorialComponent } from './components/historial/historial.component';
import { ExpansionPanelHisorialComponent } from './components/expansion-panel-hisorial/expansion-panel-hisorial.component';
import { TablaProgramacionDeObraComponent } from './components/tabla-programacion-de-obra/tabla-programacion-de-obra.component';
import { TablaFlujoIntervencionRecursosComponent } from './components/tabla-flujo-intervencion-recursos/tabla-flujo-intervencion-recursos.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';


@NgModule({
  declarations: [
    HomeComponent,
    TablaAjusteProgramacionComponent,
    ValidarAjusteComponent,
    ExpansionPanelComponent,
    ProgramacionDeObraComponent,
    FlujoIntervencionRecursosComponent,
    HistorialComponent,
    ExpansionPanelHisorialComponent,
    TablaProgramacionDeObraComponent,
    TablaFlujoIntervencionRecursosComponent,
    DialogObservacionesComponent,
  ],
  imports: [
    CommonModule,
    ValidarAjusteProgramacionRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarAjusteProgramacionModule { }
