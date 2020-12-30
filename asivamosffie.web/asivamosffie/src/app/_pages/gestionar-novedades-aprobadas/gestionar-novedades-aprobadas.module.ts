import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GestionarNovedadesAprobadasRoutingModule } from './gestionar-novedades-aprobadas-routing.module';

import { MaterialModule } from './../../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { CurrencyMaskModule } from 'ng2-currency-mask';

import { HomeComponent } from './components/home/home.component';
import { TablaNovedadesAprobadasComponent } from './components/tabla-novedades-aprobadas/tabla-novedades-aprobadas.component';
import { RegistrarRevisionJuridicaComponent } from './components/registrar-revision-juridica/registrar-revision-juridica.component';
import { ExpansionPanelComponent } from './components/expansion-panel/expansion-panel.component';
import { SolicitudNovedadComponent } from './components/solicitud-novedad/solicitud-novedad.component';
import { RegistrarRevisionNovedadComponent } from './components/registrar-revision-novedad/registrar-revision-novedad.component';
import { RegistrarFirmasComponent } from './components/registrar-firmas/registrar-firmas.component';
import { VerDetalleTramiteComponent } from './components/ver-detalle-tramite/ver-detalle-tramite.component';
import { FormDetallarSolicitudNovedadComponent } from './components/form-detallar-solicitud-novedad/form-detallar-solicitud-novedad.component';

@NgModule({
  declarations: [
    HomeComponent,
    TablaNovedadesAprobadasComponent,
    RegistrarRevisionJuridicaComponent,
    ExpansionPanelComponent,
    SolicitudNovedadComponent,
    RegistrarRevisionNovedadComponent,
    RegistrarFirmasComponent,
    VerDetalleTramiteComponent,
    FormDetallarSolicitudNovedadComponent
  ],
  imports: [
    CommonModule,
    GestionarNovedadesAprobadasRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule
  ]
})
export class GestionarNovedadesAprobadasModule { }
