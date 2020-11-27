import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegistrarSolicitudNovedadContractualRoutingModule } from './registrar-solicitud-novedad-contractual-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { RegistrarSolicitudComponent } from './components/registrar-solicitud/registrar-solicitud.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { FormRegistrarNovedadComponent } from './components/form-registrar-novedad/form-registrar-novedad.component';

@NgModule({
  declarations: [
    HomeComponent,
    RegistrarSolicitudComponent,
    ExpansionPanelComponent,
    FormRegistrarNovedadComponent
  ],
  imports: [
    CommonModule,
    RegistrarSolicitudNovedadContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarSolicitudNovedadContractualModule { }
