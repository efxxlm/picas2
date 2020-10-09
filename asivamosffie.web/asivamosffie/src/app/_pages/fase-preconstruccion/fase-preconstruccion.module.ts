import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { FasePreconstruccionRoutingModule } from './fase-preconstruccion-routing.module';
import { FormPerfilComponent } from './components/form-perfil/form-perfil.component';
import { TablaRegistrarRequisitosComponent } from './components/tabla-registrar-requisitos/tabla-registrar-requisitos.component';
import { ExpansionGestionarRequisitosComponent } from './components/expansion-gestionar-requisitos/expansion-gestionar-requisitos.component';

@NgModule({
  declarations: [FormPerfilComponent, TablaRegistrarRequisitosComponent, ExpansionGestionarRequisitosComponent],
  imports: [
    CommonModule,
    FasePreconstruccionRoutingModule,
    MaterialModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
    ReactiveFormsModule
  ],
  exports: [
    FormPerfilComponent
  ]
})
export class FasePreconstruccionModule { }
