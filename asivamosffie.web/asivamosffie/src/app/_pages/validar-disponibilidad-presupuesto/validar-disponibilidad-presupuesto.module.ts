import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { ValidarDisponibilidadPresupuestoRoutingModule } from './validar-disponibilidad-presupuesto-routing.module';
import { ValidarComponent } from './components/validar/validar.component';
import { TablaEnValidacionComponent } from './components/tabla-en-validacion/tabla-en-validacion.component';
import { ValidacionPresupuestalComponent } from './components/validacion-presupuestal/validacion-presupuestal.component';



@NgModule({
  declarations: [ValidarComponent, TablaEnValidacionComponent, ValidacionPresupuestalComponent],
  imports: [
    CommonModule,
    ValidarDisponibilidadPresupuestoRoutingModule,
    MaterialModule,
    ReactiveFormsModule
  ]
})
export class ValidarDisponibilidadPresupuestoModule { }
