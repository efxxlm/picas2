import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrarAvanceSemanalRoutingModule } from './registrar-avance-semanal-routing.module';
import { RegistrarAvanceSemanalComponent } from './components/registrar-avance-semanal/registrar-avance-semanal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaRegistrarAvanceSemanalComponent } from './components/tabla-registrar-avance-semanal/tabla-registrar-avance-semanal.component';
import { FormRegistrarSeguimientoSemanalComponent } from './components/form-registrar-seguimiento-semanal/form-registrar-seguimiento-semanal.component';
import { AvanceFisicoFinancieroComponent } from './components/avance-fisico-financiero/avance-fisico-financiero.component';
import { TablaAvanceResumenAlertasComponent } from './components/tabla-avance-resumen-alertas/tabla-avance-resumen-alertas.component';
import { TablaAvanceFisicoComponent } from './components/tabla-avance-fisico/tabla-avance-fisico.component';
import { DialogTablaAvanceResumenComponent } from './components/dialog-tabla-avance-resumen/dialog-tabla-avance-resumen.component';


@NgModule({
  declarations: [RegistrarAvanceSemanalComponent, TablaRegistrarAvanceSemanalComponent, FormRegistrarSeguimientoSemanalComponent, AvanceFisicoFinancieroComponent, TablaAvanceResumenAlertasComponent, TablaAvanceFisicoComponent, DialogTablaAvanceResumenComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    RegistrarAvanceSemanalRoutingModule
  ]
})
export class RegistrarAvanceSemanalModule { }
