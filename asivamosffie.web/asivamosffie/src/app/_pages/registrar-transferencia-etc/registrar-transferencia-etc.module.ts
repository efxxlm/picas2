import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { RegistrarTransferenciaEtcRoutingModule } from './registrar-transferencia-etc-routing.module';
import { HomeComponent } from './components/home/home.component';
import { TablaProyectosComponent } from './components/tabla-proyectos/tabla-proyectos.component';
import { RegistrarEntregaComponent } from './components/registrar-entrega/registrar-entrega.component';
import { FormRecorridoObraComponent } from './components/form-recorrido-obra/form-recorrido-obra.component';
import { FormRemisionComponent } from './components/form-remision/form-remision.component';
import { FormActaEntregaBienesYServiciosComponent } from './components/form-acta-entrega-bienes-y-servicios/form-acta-entrega-bienes-y-servicios.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaProyectosComponent,
    RegistrarEntregaComponent,
    FormRecorridoObraComponent,
    FormRemisionComponent,
    FormActaEntregaBienesYServiciosComponent
  ],
  imports: [
    CommonModule,
    RegistrarTransferenciaEtcRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarTransferenciaEtcModule {}
