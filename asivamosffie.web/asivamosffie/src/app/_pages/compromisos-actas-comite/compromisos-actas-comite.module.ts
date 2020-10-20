import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';

import { CompromisosActasComiteRoutingModule } from './compromisos-actas-comite-routing.module';
import { CompromisosActasComponent } from './components/compromisos-actas/compromisos-actas.component';
import { MaterialModule } from '../../material/material.module';
import { TablaGestionCompromisosComponent } from './components/tabla-gestion-compromisos/tabla-gestion-compromisos.component';
import { TablaGestionActasComponent } from './components/tabla-gestion-actas/tabla-gestion-actas.component';
import { ReporteAvanceCompromisoComponent } from './components/reporte-avance-compromiso/reporte-avance-compromiso.component';
import { TablaDetalleCompromisoComponent } from './components/tabla-detalle-compromiso/tabla-detalle-compromiso.component';
import { RevisionActaComponent } from './components/revision-acta/revision-acta.component';
import { FormSolicitudComponent } from './components/form-solicitud/form-solicitud.component';
import { TablaDecisionesActaComponent } from './components/tabla-decisiones-acta/tabla-decisiones-acta.component';
import { MAT_RADIO_DEFAULT_OPTIONS } from '@angular/material/radio';
import { TablaDetalleActaComponent } from './components/tabla-detalle-acta/tabla-detalle-acta.component';
import { ObservacionDialogComponent } from './components/observacion-dialog/observacion-dialog.component';


@NgModule({
  declarations: [
    CompromisosActasComponent,
    TablaGestionCompromisosComponent,
    TablaGestionActasComponent,
    ReporteAvanceCompromisoComponent,
    TablaDetalleCompromisoComponent,
    RevisionActaComponent,
    FormSolicitudComponent,
    TablaDecisionesActaComponent,
    TablaDetalleActaComponent,
    ObservacionDialogComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    CompromisosActasComiteRoutingModule
  ]
})
export class CompromisosActasComiteModule { }
