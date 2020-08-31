import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { GenerarDisponibilidadPresupuestalRoutingModule } from './generar-disponibilidad-presupuestal-routing.module';
import { MenuGenerarDisponibilidadComponent } from './components/menu-generar-disponibilidad/menu-generar-disponibilidad.component';
import { TablaConValidacionPresupuestalComponent } from './components/tabla-con-validacion-presupuestal/tabla-con-validacion-presupuestal.component';

@NgModule({
  declarations: [MenuGenerarDisponibilidadComponent, TablaConValidacionPresupuestalComponent],
  imports: [
    CommonModule,
    GenerarDisponibilidadPresupuestalRoutingModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    CurrencyMaskModule
  ]
})
export class GenerarDisponibilidadPresupuestalModule { }
