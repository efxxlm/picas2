import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarSolicitudNovedadesRoutingModule } from './validar-solicitud-novedades-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';


@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    ValidarSolicitudNovedadesRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarSolicitudNovedadesModule { }
