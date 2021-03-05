import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarListaChequeoRoutingModule } from './gestionar-lista-chequeo-routing.module';
import { GestionarListaChequeoComponent } from './components/gestionar-lista-chequeo/gestionar-lista-chequeo.component';
import { CrearBancoComponent } from './components/crear-banco/crear-banco.component';
import { CrearListaComponent } from './components/crear-lista/crear-lista.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { FormCrearBancoComponent } from './components/form-crear-banco/form-crear-banco.component';
import { TablaBancosComponent } from './components/tabla-bancos/tabla-bancos.component';
import { TablaListaChequeoComponent } from './components/tabla-lista-chequeo/tabla-lista-chequeo.component';
import { FormListaChequeoComponent } from './components/form-lista-chequeo/form-lista-chequeo.component';

@NgModule({
  declarations: [GestionarListaChequeoComponent, CrearBancoComponent, CrearListaComponent, FormCrearBancoComponent, TablaBancosComponent, TablaListaChequeoComponent, FormListaChequeoComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    QuillModule.forRoot(),
    GestionarListaChequeoRoutingModule
  ]
})
export class GestionarListaChequeoModule { }
