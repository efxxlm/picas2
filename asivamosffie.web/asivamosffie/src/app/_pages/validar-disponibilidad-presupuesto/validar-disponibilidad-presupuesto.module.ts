import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { ValidarDisponibilidadPresupuestoRoutingModule } from './validar-disponibilidad-presupuesto-routing.module';
import { ValidarComponent } from './components/validar/validar.component';
import { TablaEnValidacionComponent } from './components/tabla-en-validacion/tabla-en-validacion.component';
import { ValidacionPresupuestalComponent } from './components/validacion-presupuestal/validacion-presupuestal.component';
import { DevolverPorValidacionComponent } from './components/devolver-por-validacion/devolver-por-validacion.component';
import { TablaConValidacionComponent } from './components/tabla-con-validacion/tabla-con-validacion.component';
import { ConValidacionPresupuestalComponent } from './components/con-validacion-presupuestal/con-validacion-presupuestal.component';
import { TablaDevueltaPorValidacionComponent } from './components/tabla-devuelta-por-validacion/tabla-devuelta-por-validacion.component';
import { DevueltaPorValidacionComponent } from './components/devuelta-por-validacion/devuelta-por-validacion.component';
import { TablaRechasadaPorValidacionComponent } from './components/tabla-rechasada-por-validacion/tabla-rechasada-por-validacion.component';
import { RechasadaPorValidacionComponent } from './components/rechasada-por-validacion/rechasada-por-validacion.component';
import { TablaGestionarValidacionComponent } from './components/tabla-gestionar-validacion/tabla-gestionar-validacion.component';
import { FormGestionarFuentesComponent } from './components/form-gestionar-fuentes/form-gestionar-fuentes.component';

@NgModule({
  declarations: [
    ValidarComponent,
    TablaEnValidacionComponent,
    ValidacionPresupuestalComponent,
    DevolverPorValidacionComponent,
    TablaConValidacionComponent,
    ConValidacionPresupuestalComponent,
    TablaDevueltaPorValidacionComponent,
    DevueltaPorValidacionComponent,
    TablaRechasadaPorValidacionComponent,
    RechasadaPorValidacionComponent,
    TablaGestionarValidacionComponent,
    FormGestionarFuentesComponent
  ],
  imports: [
    CommonModule,
    ValidarDisponibilidadPresupuestoRoutingModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    CurrencyMaskModule
  ]
})
export class ValidarDisponibilidadPresupuestoModule { }
