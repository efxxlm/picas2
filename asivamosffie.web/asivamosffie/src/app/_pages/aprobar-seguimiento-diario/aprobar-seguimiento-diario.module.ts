import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AprobarSeguimientoDiarioRoutingModule } from './aprobar-seguimiento-diario-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaValidarSeguimientoDiarioComponent } from './components/tabla-validar-seguimiento-diario/tabla-validar-seguimiento-diario.component';
import { ValidarSeguimientoDiarioComponent } from './components/validar-seguimiento-diario/validar-seguimiento-diario.component';
import { FormObservacionesComponent } from './components/form-observaciones/form-observaciones.component';
import { VerObservacionesComponent } from './components/ver-observaciones/ver-observaciones.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaValidarSeguimientoDiarioComponent,
    ValidarSeguimientoDiarioComponent,
    FormObservacionesComponent,
    VerObservacionesComponent
  ],
  imports: [
    CommonModule,
    AprobarSeguimientoDiarioRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class AprobarSeguimientoDiarioModule { }
