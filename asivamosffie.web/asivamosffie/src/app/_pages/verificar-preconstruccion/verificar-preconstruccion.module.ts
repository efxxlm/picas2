import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './../../material/material.module';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { ReactiveFormsModule } from '@angular/forms';

import { VerificarPreconstruccionRoutingModule } from './verificar-preconstruccion-routing.module';
import { FormPerfilComponent } from './components/form-perfil/form-perfil.component';
import { TituloComponent } from './components/titulo/titulo.component';
import { TablaContratoDeObraComponent } from './components/tabla-contrato-de-obra/tabla-contrato-de-obra.component';
import { TablaContratoDeInterventoriaComponent } from './components/tabla-contrato-de-interventoria/tabla-contrato-de-interventoria.component';
import { ExpansionVerificarRequisitosComponent } from './components/expansion-verificar-requisitos/expansion-verificar-requisitos.component';
import { VerDetalleComponent } from './components/ver-detalle/ver-detalle.component';
import { ExpansionGestionarInterventoriaComponent } from './components/expansion-gestionar-interventoria/expansion-gestionar-interventoria.component';


@NgModule({
  declarations: [FormPerfilComponent, TituloComponent, TablaContratoDeObraComponent, TablaContratoDeInterventoriaComponent, ExpansionVerificarRequisitosComponent, VerDetalleComponent, ExpansionGestionarInterventoriaComponent],
  imports: [
    CommonModule,
    VerificarPreconstruccionRoutingModule,
    MaterialModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
    ReactiveFormsModule
  ]
})
export class VerificarPreconstruccionModule { }
