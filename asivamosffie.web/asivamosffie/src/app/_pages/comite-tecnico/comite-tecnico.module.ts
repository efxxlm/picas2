import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComiteTecnicoRoutingModule } from './comite-tecnico-routing.module';
import { ComiteTecnicoComponent } from './components/comite-tecnico/comite-tecnico.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { QuillModule } from 'ngx-quill';

import { MaterialModule } from './../../material/material.module';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { TablaSolicitudesContractualesComponent } from './components/tabla-solicitudes-contractuales/tabla-solicitudes-contractuales.component';
import { TablaOrdenDelDiaComponent } from './components/tabla-orden-del-dia/tabla-orden-del-dia.component';
import { TablaSesionComiteTecnicoComponent } from './components/tabla-sesion-comite-tecnico/tabla-sesion-comite-tecnico.component';
import { TablaGestionActasComponent } from './components/tabla-gestion-actas/tabla-gestion-actas.component';
import { TablaMonitoreoCompromisosComponent } from './components/tabla-monitoreo-compromisos/tabla-monitoreo-compromisos.component';
import { RegistrarSesionComiteTecnicoComponent } from './components/registrar-sesion-comite-tecnico/registrar-sesion-comite-tecnico.component';
import { TablaValidacionSolicitudesContractualesComponent } from './components/tabla-validacion-solicitudes-contractuales/tabla-validacion-solicitudes-contractuales.component';
import { TablaOtrosTemasComponent } from './components/tabla-otros-temas/tabla-otros-temas.component';
import { FormProposicionesVariosComponent } from './components/form-proposiciones-varios/form-proposiciones-varios.component';
import { FormRegistrarParticipantesComponent } from './components/form-registrar-participantes/form-registrar-participantes.component';
import { TablaRegistrarValidacionSolicitudesContractialesComponent } from './components/tabla-registrar-validacion-solicitudes-contractiales/tabla-registrar-validacion-solicitudes-contractiales.component';
import { TablaRegistrarOtrosTemasComponent } from './components/tabla-registrar-otros-temas/tabla-registrar-otros-temas.component';
import { VotacionSolicitudComponent } from './components/votacion-solicitud/votacion-solicitud.component';
import { AplazarSesionComponent } from './components/aplazar-sesion/aplazar-sesion.component';
import { CrearActaComponent } from './components/crear-acta/crear-acta.component';
import { FormSolicitudComponent } from './components/form-solicitud/form-solicitud.component';
import { VerificarCumplimientoComponent } from './components/verificar-cumplimiento/verificar-cumplimiento.component';
import { VotacionSolicitudMultipleComponent } from './components/votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import { TablaProposicionesYVariosComponent } from './components/tabla-proposiciones-y-varios/tabla-proposiciones-y-varios.component';

@NgModule({
  declarations: [
    ComiteTecnicoComponent,
    CrearOrdenDelDiaComponent,
    TablaSolicitudesContractualesComponent,
    TablaOrdenDelDiaComponent,
    TablaSesionComiteTecnicoComponent,
    TablaGestionActasComponent,
    TablaMonitoreoCompromisosComponent,
    RegistrarSesionComiteTecnicoComponent,
    TablaValidacionSolicitudesContractualesComponent,
    TablaOtrosTemasComponent,
    FormProposicionesVariosComponent,
    FormRegistrarParticipantesComponent,
    TablaRegistrarValidacionSolicitudesContractialesComponent,
    TablaRegistrarOtrosTemasComponent,
    VotacionSolicitudComponent,
    AplazarSesionComponent,
    CrearActaComponent,
    FormSolicitudComponent,
    VerificarCumplimientoComponent,
    VotacionSolicitudMultipleComponent,
    TablaProposicionesYVariosComponent
  ],
  imports: [
    CommonModule,
    ComiteTecnicoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class ComiteTecnicoModule { }
