import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComiteTecnicoRoutingModule } from './comite-tecnico-routing.module';
import { ComiteTecnicoComponent } from './components/comite-tecnico/comite-tecnico.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { MaterialModule } from './../../material/material.module';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
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

@NgModule({
  declarations: [ComiteTecnicoComponent, CrearOrdenDelDiaComponent, TablaSolicitudesContractualesComponent, TablaOrdenDelDiaComponent, TablaSesionComiteTecnicoComponent, TablaGestionActasComponent, TablaMonitoreoCompromisosComponent, RegistrarSesionComiteTecnicoComponent, TablaValidacionSolicitudesContractualesComponent, TablaOtrosTemasComponent, FormProposicionesVariosComponent, FormRegistrarParticipantesComponent, TablaRegistrarValidacionSolicitudesContractialesComponent, TablaRegistrarOtrosTemasComponent],
  imports: [
    CommonModule,
    ComiteTecnicoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule
  ]
})
export class ComiteTecnicoModule { }
