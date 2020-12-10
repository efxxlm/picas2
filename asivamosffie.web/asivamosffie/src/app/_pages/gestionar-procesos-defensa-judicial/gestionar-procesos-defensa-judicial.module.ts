import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarProcesosDefensaJudicialComponent } from './components/gestionar-procesos-defensa-judicial/gestionar-procesos-defensa-judicial.component';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { GestionarProcesosDefensaJudicialRoutingModule } from './gestionar-procesos-defensa-judicial-routing.module';



@NgModule({
  declarations: [GestionarProcesosDefensaJudicialComponent],
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
