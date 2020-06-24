import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { CambiarContrasenaRoutingModule } from './cambiar-contrasena-routing.module';
import { CambiarContrasenaComponent } from './components/cambiar-contrasena/cambiar-contrasena.component';

import { MaterialModule } from './../../material/material.module';

@NgModule({
  declarations: [CambiarContrasenaComponent],
  imports: [
    CommonModule,
    CambiarContrasenaRoutingModule,
    MaterialModule,
    ReactiveFormsModule
  ]
})
export class CambiarContrasenaModule { }
