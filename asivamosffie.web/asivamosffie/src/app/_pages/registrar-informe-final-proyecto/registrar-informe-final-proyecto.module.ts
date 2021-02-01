import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrarInformeFinalProyectoRoutingModule } from './registrar-informe-final-proyecto-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaInformeFinalProyectoComponent } from './components/tabla-informe-final-proyecto/tabla-informe-final-proyecto.component';
import { RegistrarInformeFinalComponent } from './components/registrar-informe-final/registrar-informe-final.component';
import { DatosDelProyectoComponent } from './components/datos-del-proyecto/datos-del-proyecto.component';
import { FormReciboASatisfaccionComponent } from './components/form-recibo-a-satisfaccion/form-recibo-a-satisfaccion.component';
import { InformeFinalAnexosComponent } from './components/informe-final-anexos/informe-final-anexos.component';
import { TablaInformeFinalAnexosComponent } from './components/tabla-informe-final-anexos/tabla-informe-final-anexos.component';
import { DialogTipoDocumentoComponent } from './components/dialog-tipo-documento/dialog-tipo-documento.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { VerDetalleInformeFinalComponent } from './components/ver-detalle-informe-final/ver-detalle-informe-final.component';
import { DetalleReciboSatisfaccionComponent } from './components/detalle-recibo-satisfaccion/detalle-recibo-satisfaccion.component';
import { TablaDetalleInformeAnexosComponent } from './components/tabla-detalle-informe-anexos/tabla-detalle-informe-anexos.component';
import { DialogDetalleObservacionesComponent } from './components/dialog-detalle-observaciones/dialog-detalle-observaciones.component';


@NgModule({
  declarations: [
    HomeComponent,
    TablaInformeFinalProyectoComponent,
    RegistrarInformeFinalComponent,
    DatosDelProyectoComponent,
    FormReciboASatisfaccionComponent,
    InformeFinalAnexosComponent,
    TablaInformeFinalAnexosComponent,
    DialogTipoDocumentoComponent,
    DialogObservacionesComponent,
    VerDetalleInformeFinalComponent,
    DetalleReciboSatisfaccionComponent,
    TablaDetalleInformeAnexosComponent,
    DialogDetalleObservacionesComponent
  ],
  entryComponents: [
    DialogTipoDocumentoComponent,
    DialogObservacionesComponent
  ],
  imports: [
    CommonModule,
    RegistrarInformeFinalProyectoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarInformeFinalProyectoModule { }
