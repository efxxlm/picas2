import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { SolicitarDisponibilidadPresupuestalRoutingModule } from './solicitar-disponibilidad-presupuestal-routing.module';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaCrearSolicitudTradicionalComponent } from './components/tabla-crear-solicitud-tradicional/tabla-crear-solicitud-tradicional.component';
import { RegistrarInformacionAdicionalComponent } from './components/registrar-informacion-adicional/registrar-informacion-adicional.component';
import { CrearSolicitudEspecialComponent } from './components/crear-solicitud-especial/crear-solicitud-especial.component';
import { NuevaSolicitudEspecialComponent } from './components/nueva-solicitud-especial/nueva-solicitud-especial.component';
import { TablaCrearSolicitudEspecialComponent } from './components/tabla-crear-solicitud-especial/tabla-crear-solicitud-especial.component';


@NgModule({
  declarations: [TituloComponent, TablaCrearSolicitudTradicionalComponent, RegistrarInformacionAdicionalComponent, CrearSolicitudEspecialComponent, NuevaSolicitudEspecialComponent, TablaCrearSolicitudEspecialComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    SolicitarDisponibilidadPresupuestalRoutingModule
  ]
})
export class SolicitarDisponibilidadPresupuestalModule { }
