import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarInformeDelProyectoRoutingModule } from './validar-informe-del-proyecto-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaInformeFinalProyectoComponent } from './components/tabla-informe-final-proyecto/tabla-informe-final-proyecto.component';
import { ValidarInformeComponent } from './components/validar-informe/validar-informe.component';
import { DatosDelProyectoComponent } from './components/datos-del-proyecto/datos-del-proyecto.component';
import { TablaInformeFinalAnexosComponent } from './components/tabla-informe-final-anexos/tabla-informe-final-anexos.component';
import { FormObservacionesReciboSatisfaccionComponent } from './components/form-observaciones-recibo-satisfaccion/form-observaciones-recibo-satisfaccion.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { VerDetalleInformeFinalComponent } from './components/ver-detalle-informe-final/ver-detalle-informe-final.component';
import { DetalleObservacionesReciboSatisfaccionComponent } from './components/detalle-observaciones-recibo-satisfaccion/detalle-observaciones-recibo-satisfaccion.component';
import { TablaDetalleInformeAnexosComponent } from './components/tabla-detalle-informe-anexos/tabla-detalle-informe-anexos.component';
import { TablaObservacionesCumplimientoInterventoriaComponent } from './components/tabla-observaciones-cumplimiento-interventoria/tabla-observaciones-cumplimiento-interventoria.component';
import { TablaHistorialObservacionesComponent } from './components/tabla-historial-observaciones/tabla-historial-observaciones.component';


@NgModule({
  declarations: [
    HomeComponent,
    TablaInformeFinalProyectoComponent,
    ValidarInformeComponent,
    DatosDelProyectoComponent,
    TablaInformeFinalAnexosComponent,
    FormObservacionesReciboSatisfaccionComponent,
    DialogObservacionesComponent,
    VerDetalleInformeFinalComponent,
    DetalleObservacionesReciboSatisfaccionComponent,
    TablaDetalleInformeAnexosComponent,
    TablaObservacionesCumplimientoInterventoriaComponent,
    TablaHistorialObservacionesComponent
  ],
  imports: [
    CommonModule,
    ValidarInformeDelProyectoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarInformeDelProyectoModule { }
