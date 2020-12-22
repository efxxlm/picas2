import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ValidarSolicitudNovedadesRoutingModule } from './validar-solicitud-novedades-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { HomeComponent } from './components/home/home.component';
import { TablaNovedadContratosObraComponent } from './components/tabla-novedad-contratos-obra/tabla-novedad-contratos-obra.component';
import { VerificarSolicitudNovedadComponent } from './components/verificar-solicitud-novedad/verificar-solicitud-novedad.component';
import { VerDetalleSolicitudComponent } from './components/ver-detalle-solicitud/ver-detalle-solicitud.component';
import { RegistrarSolicitudComponent } from './components/registrar-solicitud/registrar-solicitud.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { FormRegistrarNovedadComponent } from './components/form-registrar-novedad/form-registrar-novedad.component';
import { FormSoporteSolicitudComponent } from './components/form-soporte-solicitud/form-soporte-solicitud.component';
import { TablaSolicitudNovedadContractualComponent } from './components/tabla-solicitud-novedad-contractual/tabla-solicitud-novedad-contractual.component';
import { VerDetalleInterventoriaComponent } from './components/ver-detalle-interventoria/ver-detalle-interventoria.component';
import { RegistrarSolicitudInterventoriaComponent } from './components/registrar-solicitud-interventoria/registrar-solicitud-interventoria.component';
import { ExpansionPanelInterventoriaComponent } from './components/expansion-panel-interventoria/expansion-panel-interventoria.component';


@NgModule({
  declarations: [HomeComponent, TablaNovedadContratosObraComponent, VerificarSolicitudNovedadComponent, VerDetalleSolicitudComponent, RegistrarSolicitudComponent, ExpansionPanelComponent, FormRegistrarNovedadComponent, FormSoporteSolicitudComponent, TablaSolicitudNovedadContractualComponent, VerDetalleInterventoriaComponent, RegistrarSolicitudInterventoriaComponent, ExpansionPanelInterventoriaComponent],
  imports: [
    CommonModule,
    ValidarSolicitudNovedadesRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot()
  ]
})
export class ValidarSolicitudNovedadesModule { }
