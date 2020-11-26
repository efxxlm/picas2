import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistroSeguimientoDiarioRoutingModule } from './registro-seguimiento-diario-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { TablaRegistroSeguimientoDiarioComponent } from './components/tabla-registro-seguimiento-diario/tabla-registro-seguimiento-diario.component';
import { HomeComponent } from './components/home/home.component';
import { FormRegistrarSeguimientoComponent } from './components/form-registrar-seguimiento/form-registrar-seguimiento.component';
import { VerDetalleRegistroComponent } from './components/ver-detalle-registro/ver-detalle-registro.component';
import { VerBitacoraComponent } from './components/ver-bitacora/ver-bitacora.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaRegistroSeguimientoDiarioComponent,
    FormRegistrarSeguimientoComponent,
    VerDetalleRegistroComponent,
    VerBitacoraComponent,
  ],
  imports: [
    CommonModule,
    RegistroSeguimientoDiarioRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistroSeguimientoDiarioModule { }
