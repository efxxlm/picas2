import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarFuentesDeFinanciacionRoutingModule } from './gestionar-fuentes-de-financiacion-routing.module';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarComponent } from './components/registrar/registrar.component';
import { TablaFuentesComponent } from './components/tabla-fuentes/tabla-fuentes.component';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { ControlDeRecursosComponent } from './components/control-de-recursos/control-de-recursos.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';


@NgModule({
  declarations: [BtnRegistrarComponent, RegistrarComponent, TablaFuentesComponent, ControlDeRecursosComponent],
  imports: [
    CommonModule,
    GestionarFuentesDeFinanciacionRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
  ]
})
export class GestionarFuentesDeFinanciacionModule { }
