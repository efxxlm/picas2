import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProgramacionPersonalObraRoutingModule } from './programacion-personal-obra-routing.module';
import { ProgramacionPersonalObraComponent } from './components/programacion-personal-obra/programacion-personal-obra.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TablaRegistroProgramacionComponent } from './components/tabla-registro-programacion/tabla-registro-programacion.component';
import { DialogRegistroProgramacionComponent } from './components/dialog-registro-programacion/dialog-registro-programacion.component';
import { TablaRegistroSemanasComponent } from './components/tabla-registro-semanas/tabla-registro-semanas.component';


@NgModule({
  declarations: [ProgramacionPersonalObraComponent, TablaRegistroProgramacionComponent, DialogRegistroProgramacionComponent, TablaRegistroSemanasComponent],
  imports: [
    CommonModule,
    ProgramacionPersonalObraRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class ProgramacionPersonalObraModule { }
