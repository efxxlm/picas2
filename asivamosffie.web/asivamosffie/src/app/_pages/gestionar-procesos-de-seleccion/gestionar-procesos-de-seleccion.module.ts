import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

import { GestionarProcesosDeSeleccionRoutingModule } from './gestionar-procesos-de-seleccion-routing.module';
import { BtnRegistrarComponent } from './components/btn-registrar/btn-registrar.component';
import { RegistrarNuevoComponent } from './components/registrar-nuevo/registrar-nuevo.component';
import { SeccionPrivadaComponent } from './components/seccion-privada/seccion-privada.component';
import { FormDescripcionDelProcesoDeSeleccionComponent } from './components/form-descripcion-del-proceso-de-seleccion/form-descripcion-del-proceso-de-seleccion.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';


@NgModule({
  declarations: [BtnRegistrarComponent, RegistrarNuevoComponent, SeccionPrivadaComponent, FormDescripcionDelProcesoDeSeleccionComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    GestionarProcesosDeSeleccionRoutingModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule
  ]
})
export class GestionarProcesosDeSeleccionModule { }
