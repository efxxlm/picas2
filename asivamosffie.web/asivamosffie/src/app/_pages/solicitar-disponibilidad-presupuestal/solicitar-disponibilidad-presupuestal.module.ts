import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { SolicitarDisponibilidadPresupuestalRoutingModule } from './solicitar-disponibilidad-presupuestal-routing.module';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaCrearSolicitudTradicionalComponent } from './components/tabla-crear-solicitud-tradicional/tabla-crear-solicitud-tradicional.component';
import { RegistrarInformacionAdicionalComponent } from './components/registrar-informacion-adicional/registrar-informacion-adicional.component';


@NgModule({
  declarations: [TituloComponent, TablaCrearSolicitudTradicionalComponent, RegistrarInformacionAdicionalComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    SolicitarDisponibilidadPresupuestalRoutingModule
  ]
})
export class SolicitarDisponibilidadPresupuestalModule { }
