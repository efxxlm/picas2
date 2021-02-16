import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { ValidarCumplimientoInformeFinalProyectoRoutingModule } from './validar-cumplimiento-informe-final-proyecto-routing.module';
import { HomeComponent } from './components/home/home.component';
import { TablaInformeFinalComponent } from './components/tabla-informe-final/tabla-informe-final.component';
import { RevisarInformeComponent } from './components/revisar-informe/revisar-informe.component';
import { DatosDelProyectoComponent } from './components/datos-del-proyecto/datos-del-proyecto.component';
import { ReciboSatisfaccionComponent } from './components/recibo-satisfaccion/recibo-satisfaccion.component';
import { TablaInformeFinalAnexosComponent } from './components/tabla-informe-final-anexos/tabla-informe-final-anexos.component';
import { FormObservacionesReciboSatisfaccionComponent } from './components/form-observaciones-recibo-satisfaccion/form-observaciones-recibo-satisfaccion.component';
import { FormObservacionesInformeFinalAnexosComponent } from './components/form-observaciones-informe-final-anexos/form-observaciones-informe-final-anexos.component';
import { TablaHistorialObservacionesComponent } from './components/tabla-historial-observaciones/tabla-historial-observaciones.component';
import { DetalleInformeComponent } from './components/detalle-informe/detalle-informe.component';
import { VerificacionNovedadesComponent } from './components/verificacion-novedades/verificacion-novedades.component';
import { TablaDetalleInformeFinalAnexosComponent } from './components/tabla-detalle-informe-final-anexos/tabla-detalle-informe-final-anexos.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaInformeFinalComponent,
    RevisarInformeComponent,
    DatosDelProyectoComponent,
    ReciboSatisfaccionComponent,
    TablaInformeFinalAnexosComponent,
    FormObservacionesReciboSatisfaccionComponent,
    FormObservacionesInformeFinalAnexosComponent,
    TablaHistorialObservacionesComponent,
    DetalleInformeComponent,
    VerificacionNovedadesComponent,
    TablaDetalleInformeFinalAnexosComponent
  ],
  imports: [
    CommonModule,
    ValidarCumplimientoInformeFinalProyectoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarCumplimientoInformeFinalProyectoModule { }
