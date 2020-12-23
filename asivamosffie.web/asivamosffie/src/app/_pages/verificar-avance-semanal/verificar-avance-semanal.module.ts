import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './../../material/material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarAvanceSemanalRoutingModule } from './verificar-avance-semanal-routing.module';
import { VerificarAvanceSemanalComponent } from './components/verificar-avance-semanal/verificar-avance-semanal.component';
import { TablaVerificarAvanceSemanalComponent } from './components/tabla-verificar-avance-semanal/tabla-verificar-avance-semanal.component';
import { QuillModule } from 'ngx-quill';
import { FormVerificarSeguimientoSemanalComponent } from './components/form-verificar-seguimiento-semanal/form-verificar-seguimiento-semanal.component';


@NgModule({
  declarations: [VerificarAvanceSemanalComponent, TablaVerificarAvanceSemanalComponent, FormVerificarSeguimientoSemanalComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    VerificarAvanceSemanalRoutingModule
  ]
})
export class VerificarAvanceSemanalModule { }
