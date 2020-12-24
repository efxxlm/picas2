import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './../../material/material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarAvanceSemanalRoutingModule } from './verificar-avance-semanal-routing.module';
import { VerificarAvanceSemanalComponent } from './components/verificar-avance-semanal/verificar-avance-semanal.component';
import { TablaVerificarAvanceSemanalComponent } from './components/tabla-verificar-avance-semanal/tabla-verificar-avance-semanal.component';
import { QuillModule } from 'ngx-quill';
import { FormVerificarSeguimientoSemanalComponent } from './components/form-verificar-seguimiento-semanal/form-verificar-seguimiento-semanal.component';
import { VerificarAvanceFisicoComponent } from './components/verificar-avance-fisico/verificar-avance-fisico.component';
import { TablaAvanceResumenAlertasComponent } from './components/tabla-avance-resumen-alertas/tabla-avance-resumen-alertas.component';
import { DialogAvanceResumenAlertasComponent } from './components/dialog-avance-resumen-alertas/dialog-avance-resumen-alertas.component';
import { TablaDisponibilidadMaterialComponent } from './components/tabla-disponibilidad-material/tabla-disponibilidad-material.component';
import { TablaAvanceFisicoComponent } from './components/tabla-avance-fisico/tabla-avance-fisico.component';
import { DialogAvanceAcumuladoComponent } from './components/dialog-avance-acumulado/dialog-avance-acumulado.component';


@NgModule({
  declarations: [VerificarAvanceSemanalComponent, TablaVerificarAvanceSemanalComponent, FormVerificarSeguimientoSemanalComponent, VerificarAvanceFisicoComponent, TablaAvanceResumenAlertasComponent, DialogAvanceResumenAlertasComponent, TablaDisponibilidadMaterialComponent, TablaAvanceFisicoComponent, DialogAvanceAcumuladoComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    VerificarAvanceSemanalRoutingModule
  ]
})
export class VerificarAvanceSemanalModule { }
