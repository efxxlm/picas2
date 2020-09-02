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
import { FormModificacionContractualComponent } from './components/form-modificacion-contractual/form-modificacion-contractual.component';
import { TablaProyectosAsociadosComponent } from './components/tabla-proyectos-asociados/tabla-proyectos-asociados.component';
import { FormLiquidacionComponent } from './components/form-liquidacion/form-liquidacion.component';
import { TablaRecursosCompartidosComponent } from './components/tabla-recursos-compartidos/tabla-recursos-compartidos.component';
import { FormRecursosCompartidosComponent } from './components/form-recursos-compartidos/form-recursos-compartidos.component';
import { TablaRecursosAportantesComponent } from './components/tabla-recursos-aportantes/tabla-recursos-aportantes.component';


@NgModule({
  declarations: [ProcesosContractualesComponent, TablaSolicitudesSinTramitarComponent, TablaSolicitudesEnviadasComponent, FormContratacionComponent, FormAportantesComponent, FormModificacionContractualComponent, TablaProyectosAsociadosComponent, FormLiquidacionComponent, TablaRecursosCompartidosComponent, FormRecursosCompartidosComponent, TablaRecursosAportantesComponent],
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
