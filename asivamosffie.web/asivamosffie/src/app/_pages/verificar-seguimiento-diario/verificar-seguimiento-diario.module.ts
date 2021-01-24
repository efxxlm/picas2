import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarSeguimientoDiarioRoutingModule } from './verificar-seguimiento-diario-routing.module';
import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaVerificarSeguimientoDiarioComponent } from './components/tabla-verificar-seguimiento-diario/tabla-verificar-seguimiento-diario.component';
import { VerificarSeguimientoComponent } from './components/verificar-seguimiento/verificar-seguimiento.component';
import { FormObservacionesComponent } from './components/form-observaciones/form-observaciones.component';
import { VerObservacionesComponent } from './components/ver-observaciones/ver-observaciones.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaVerificarSeguimientoDiarioComponent,
    VerificarSeguimientoComponent,
    FormObservacionesComponent,
    VerObservacionesComponent
  ],
  imports: [
    CommonModule,
    VerificarSeguimientoDiarioRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class VerificarSeguimientoDiarioModule { }
