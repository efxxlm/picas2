import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AprobarPreconstruccionRoutingModule } from './aprobar-preconstruccion-routing.module';
import { FormPerfilComponent } from './components/form-perfil/form-perfil.component';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaContratoDeObraComponent } from './components/tabla-contrato-de-obra/tabla-contrato-de-obra.component';
import { TablaContratoDeInterventoriaComponent } from './components/tabla-contrato-de-interventoria/tabla-contrato-de-interventoria.component';
import { ExpansionValidarRequisitosComponent } from './components/expansion-validar-requisitos/expansion-validar-requisitos.component';
import { ExpansionInterValidarRequisitosComponent } from './components/expansion-inter-validar-requisitos/expansion-inter-validar-requisitos.component';
import { VerDetalleAprobarPreconstruccionComponent } from './components/ver-detalle-aprobar-preconstruccion/ver-detalle-aprobar-preconstruccion.component';


@NgModule({
  declarations: [
    FormPerfilComponent,
    TituloComponent,
    TablaContratoDeObraComponent,
    TablaContratoDeInterventoriaComponent,
    ExpansionValidarRequisitosComponent,
    ExpansionInterValidarRequisitosComponent,
    VerDetalleAprobarPreconstruccionComponent
  ],
  imports: [
    CommonModule,
    AprobarPreconstruccionRoutingModule,
    MaterialModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
    ReactiveFormsModule
  ]
})
export class AprobarPreconstruccionModule { }
