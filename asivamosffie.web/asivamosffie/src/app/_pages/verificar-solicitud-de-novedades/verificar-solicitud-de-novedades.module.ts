import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VerificarSolicitudDeNovedadesRoutingModule } from './verificar-solicitud-de-novedades-routing.module';

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
import { AccordionNovedadesComponent } from './components/accordion-novedades/accordion-novedades.component';
import { FormRegistrarNovedadAccordComponent } from './components/form-registrar-novedad-accord/form-registrar-novedad-accord.component';
import { TablaProyectosRegistrarNovedadComponent } from './components/tabla-proyectos-registrar-novedad/tabla-proyectos-registrar-novedad.component'
import { CurrencyMaskModule } from 'ng2-currency-mask';



@NgModule({
  declarations: [
    HomeComponent,
    TablaNovedadContratosObraComponent,
    VerificarSolicitudNovedadComponent,
    VerDetalleSolicitudComponent,
    RegistrarSolicitudComponent,
    ExpansionPanelComponent,
    FormRegistrarNovedadComponent,
    FormSoporteSolicitudComponent,
    TablaSolicitudNovedadContractualComponent,
    VerDetalleInterventoriaComponent,
    RegistrarSolicitudInterventoriaComponent,
    ExpansionPanelInterventoriaComponent,
    AccordionNovedadesComponent,
    FormRegistrarNovedadAccordComponent,
    TablaProyectosRegistrarNovedadComponent],
  imports: [
    CommonModule,
    VerificarSolicitudDeNovedadesRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule
  ]
})
export class VerificarSolicitudDeNovedadesModule { }
