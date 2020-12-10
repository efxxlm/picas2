import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { GestionarProcesosDefensaJudicialRoutingModule } from './gestionar-procesos-defensa-judicial-routing.module';
import { ControlTablaProcesoDefensaJudicialComponent } from './components/control-tabla-proceso-defensa-judicial/control-tabla-proceso-defensa-judicial.component';
import { RegistroNuevoProcesoJudicialComponent } from './components/registro-nuevo-proceso-judicial/registro-nuevo-proceso-judicial.component';
import { FormContratosAsociadosDjComponent } from './components/form-contratos-asociados-dj/form-contratos-asociados-dj.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';



@NgModule({
  declarations: [GestionarProcesosDefensaJudicialComponent, ControlTablaProcesoDefensaJudicialComponent, RegistroNuevoProcesoJudicialComponent, FormContratosAsociadosDjComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    GestionarProcesosDefensaJudicialRoutingModule,
    QuillModule.forRoot()
  ]
})
export class GestionarProcesosDefensaJudicialModule { }
