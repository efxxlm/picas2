import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarAcuerdoCofinanciacionRoutingModule } from './gestionar-acuerdo-cofinanciacion-routing.module';
import { BotonRegistrarAcuerdoComponent } from './components/boton-registrar-acuerdo/boton-registrar-acuerdo.component';
import { RegistrarAcuerdoComponent } from './components/registrar-acuerdo/registrar-acuerdo.component';
import { FormDocumentoDeApropiacionComponent } from './components/form-documento-de-apropiacion/form-documento-de-apropiacion.component';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [BotonRegistrarAcuerdoComponent, RegistrarAcuerdoComponent, FormDocumentoDeApropiacionComponent],
  imports: [
    CommonModule,
    GestionarAcuerdoCofinanciacionRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
  ]
})
export class GestionarAcuerdoCofinanciacionModule { }
