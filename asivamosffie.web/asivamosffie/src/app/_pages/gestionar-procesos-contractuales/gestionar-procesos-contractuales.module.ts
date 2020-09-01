import { NgModule } from '@angular/core';
import { QuillModule } from 'ngx-quill';
import { CommonModule } from '@angular/common';

import { GestionarProcesosContractualesRoutingModule } from './gestionar-procesos-contractuales-routing.module';
import { ProcesosContractualesComponent } from './components/procesos-contractuales/procesos-contractuales.component';
import { MaterialModule } from '../../material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TablaSolicitudesSinTramitarComponent } from './components/tabla-solicitudes-sin-tramitar/tabla-solicitudes-sin-tramitar.component';
import { TablaSolicitudesEnviadasComponent } from './components/tabla-solicitudes-enviadas/tabla-solicitudes-enviadas.component';
import { FormContratacionComponent } from './components/form-contratacion/form-contratacion.component';
import { FormAportantesComponent } from './components/form-aportantes/form-aportantes.component';


@NgModule({
  declarations: [ProcesosContractualesComponent, TablaSolicitudesSinTramitarComponent, TablaSolicitudesEnviadasComponent, FormContratacionComponent, FormAportantesComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot(),
    GestionarProcesosContractualesRoutingModule
  ]
})
export class GestionarProcesosContractualesModule { }
