import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarAcuerdoCofinanciacionRoutingModule } from './gestionar-acuerdo-cofinanciacion-routing.module';
import { BotonRegistrarAcuerdoComponent } from './components/boton-registrar-acuerdo/boton-registrar-acuerdo.component';
import { RegistrarAcuerdoComponent } from './components/registrar-acuerdo/registrar-acuerdo.component';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TablaAcuerdosComponent } from './components/tabla-acuerdos/tabla-acuerdos.component';

import { CurrencyPipe } from '@angular/common';

@NgModule({
  declarations: [BotonRegistrarAcuerdoComponent, RegistrarAcuerdoComponent,TablaAcuerdosComponent],
  imports: [
    CommonModule,
    GestionarAcuerdoCofinanciacionRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [CurrencyPipe]
})
export class GestionarAcuerdoCofinanciacionModule { }
