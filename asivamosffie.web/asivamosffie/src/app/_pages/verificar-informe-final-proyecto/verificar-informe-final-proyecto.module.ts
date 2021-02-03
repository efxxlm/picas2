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


@NgModule({
  declarations: [
    HomeComponent,
    TablaInformeFinalComponent,
    VerificarInformeComponent,
    DatosDelProyectoComponent,
    ReciboSatisfaccionComponent,
    TablaInformeFinalAnexosComponent,
    DialogObservacionesComponent
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
