import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrarAvanceSemanalRoutingModule } from './registrar-avance-semanal-routing.module';
import { RegistrarAvanceSemanalComponent } from './components/registrar-avance-semanal/registrar-avance-semanal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaRegistrarAvanceSemanalComponent } from './components/tabla-registrar-avance-semanal/tabla-registrar-avance-semanal.component';


@NgModule({
  declarations: [RegistrarAvanceSemanalComponent, TablaRegistrarAvanceSemanalComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    RegistrarAvanceSemanalRoutingModule
  ]
})
export class RegistrarAvanceSemanalModule { }
