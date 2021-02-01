import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarSolicitudNovedadesRoutingModule } from './validar-solicitud-novedades-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaNovedadContratosObraComponent } from './components/tabla-novedad-contratos-obra/tabla-novedad-contratos-obra.component';
import { TablaSolicitudNovedadContractualComponent } from './components/tabla-solicitud-novedad-contractual/tabla-solicitud-novedad-contractual.component';
import { ValidarNovedadContratoObraComponent } from './components/validar-novedad-contrato-obra/validar-novedad-contrato-obra.component';
import { DialogRechazarSolicitudComponent } from './components/dialog-rechazar-solicitud/dialog-rechazar-solicitud.component';
import { ValidarNovedadContratoInterventoriaComponent } from './components/validar-novedad-contrato-interventoria/validar-novedad-contrato-interventoria.component';
import { DialogRechazarSolicitudInterventorComponent } from './components/dialog-rechazar-solicitud-interventor/dialog-rechazar-solicitud-interventor.component';
import { DialogDevolverSolicitudInterventorComponent } from './components/dialog-devolver-solicitud-interventor/dialog-devolver-solicitud-interventor.component';


@NgModule({
  declarations: [
    HomeComponent,
    TablaNovedadContratosObraComponent,
    TablaSolicitudNovedadContractualComponent,
    ValidarNovedadContratoObraComponent,
    DialogRechazarSolicitudComponent,
    ValidarNovedadContratoInterventoriaComponent,
    DialogRechazarSolicitudInterventorComponent,
    DialogDevolverSolicitudInterventorComponent
  ],
  imports: [
    CommonModule,
    ValidarSolicitudNovedadesRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarSolicitudNovedadesModule { }
