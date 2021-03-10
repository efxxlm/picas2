import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarInformeFinalProyectoRoutingModule } from './verificar-informe-final-proyecto-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaInformeFinalComponent } from './components/tabla-informe-final/tabla-informe-final.component';
import { VerificarInformeComponent } from './components/verificar-informe/verificar-informe.component';
import { DatosDelProyectoComponent } from './components/datos-del-proyecto/datos-del-proyecto.component';
import { ReciboSatisfaccionComponent } from './components/recibo-satisfaccion/recibo-satisfaccion.component';
import { TablaInformeFinalAnexosComponent } from './components/tabla-informe-final-anexos/tabla-informe-final-anexos.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { FormObservacionesReciboSatisfaccionComponent } from './components/form-observaciones-recibo-satisfaccion/form-observaciones-recibo-satisfaccion.component';
import { VerDetalleInformeFinalComponent } from './components/ver-detalle-informe-final/ver-detalle-informe-final.component';
import { TablaDetalleComponent } from './components/tabla-detalle/tabla-detalle.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaInformeFinalComponent,
    VerificarInformeComponent,
    DatosDelProyectoComponent,
    ReciboSatisfaccionComponent,
    TablaInformeFinalAnexosComponent,
    DialogObservacionesComponent,
    FormObservacionesReciboSatisfaccionComponent,
    VerDetalleInformeFinalComponent,
    TablaDetalleComponent
  ],
  imports: [
    CommonModule,
    VerificarInformeFinalProyectoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class VerificarInformeFinalProyectoModule { }
