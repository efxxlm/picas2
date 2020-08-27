import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { GenerarDisponibilidadPresupuestalRoutingModule } from './generar-disponibilidad-presupuestal-routing.module';

@NgModule({
  declarations: [],
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
