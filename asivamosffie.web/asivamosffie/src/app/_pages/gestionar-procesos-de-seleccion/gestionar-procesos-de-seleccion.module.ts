import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { QuillModule } from 'ngx-quill';

import { GestionarProcesosDeSeleccionRoutingModule } from './gestionar-procesos-de-seleccion-routing.module';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarNuevoComponent } from './components/registrar-nuevo/registrar-nuevo.component';
import { SeccionPrivadaComponent } from './components/seccion-privada/seccion-privada.component';
import { FormDescripcionDelProcesoDeSeleccionComponent } from './components/form-descripcion-del-proceso-de-seleccion/form-descripcion-del-proceso-de-seleccion.component';
import { FormEstudioDeMercadoComponent } from './components/form-estudio-de-mercado/form-estudio-de-mercado.component';
import { FormDatosProponentesSeleccionadosComponent } from './components/form-datos-proponentes-seleccionados/form-datos-proponentes-seleccionados.component';
import { TablaProcesosComponent } from './components/tabla-procesos/tabla-procesos.component';


@NgModule({
  declarations: [
    BtnRegistrarComponent,
    RegistrarNuevoComponent,
    SeccionPrivadaComponent,
    FormDescripcionDelProcesoDeSeleccionComponent,
    FormEstudioDeMercadoComponent,
    FormDatosProponentesSeleccionadosComponent,
    TablaProcesosComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    GestionarProcesosDeSeleccionRoutingModule,
    QuillModule.forRoot()
  ]
})
export class GestionarProcesosDeSeleccionModule { }
