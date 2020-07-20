import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarFuentesDeFinanciacionRoutingModule } from './gestionar-fuentes-de-financiacion-routing.module';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarComponent } from './components/registrar/registrar.component';
import { TablaFuentesComponent } from './components/tabla-fuentes/tabla-fuentes.component';
import { ControlDeRecursosComponent } from './components/control-de-recursos/control-de-recursos.component'

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [BtnRegistrarComponent, RegistrarComponent, TablaFuentesComponent, ControlDeRecursosComponent],
  imports: [
    CommonModule,
    GestionarFuentesDeFinanciacionRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
  ]
})
export class GestionarFuentesDeFinanciacionModule { }
