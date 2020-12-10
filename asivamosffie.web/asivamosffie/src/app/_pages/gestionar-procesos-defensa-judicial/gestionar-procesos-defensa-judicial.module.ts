import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { GestionarProcesosDefensaJudicialRoutingModule } from './gestionar-procesos-defensa-judicial-routing.module';
import { ControlTablaProcesoDefensaJudicialComponent } from './components/control-tabla-proceso-defensa-judicial/control-tabla-proceso-defensa-judicial.component';
import { RegistroNuevoProcesoJudicialComponent } from './components/registro-nuevo-proceso-judicial/registro-nuevo-proceso-judicial.component';



@NgModule({
  declarations: [GestionarProcesosDefensaJudicialComponent, ControlTablaProcesoDefensaJudicialComponent, RegistroNuevoProcesoJudicialComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    GestionarProcesosDefensaJudicialRoutingModule,
    QuillModule.forRoot()
  ]
})
export class GestionarProcesosDefensaJudicialModule { }
