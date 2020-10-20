import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ComiteFiduciarioRoutingModule } from './comite-fiduciario-routing.module';
import { ComiteFiduciarioComponent } from './components/comite-fiduciario/comite-fiduciario.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { QuillModule } from 'ngx-quill';

import { MaterialModule } from './../../material/material.module';
import { CrearOrdenDelDiaComponent } from './components/crear-orden-del-dia/crear-orden-del-dia.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatCardModule } from '@angular/material/card';
import { TablaSolicitudesContractualesComponent } from './components/tabla-solicitudes-contractuales/tabla-solicitudes-contractuales.component';
import { TablaOrdenDelDiaComponent } from './components/tabla-orden-del-dia/tabla-orden-del-dia.component';
import { TablaGestionActasComponent } from './components/tabla-gestion-actas/tabla-gestion-actas.component';
import { TablaMonitoreoCompromisosComponent } from './components/tabla-monitoreo-compromisos/tabla-monitoreo-compromisos.component';
import { TablaValidacionSolicitudesContractualesComponent } from './components/tabla-validacion-solicitudes-contractuales/tabla-validacion-solicitudes-contractuales.component';
import { TablaOtrosTemasComponent } from './components/tabla-otros-temas/tabla-otros-temas.component';
import { FormProposicionesVariosComponent } from './components/form-proposiciones-varios/form-proposiciones-varios.component';
import { FormRegistrarParticipantesComponent } from './components/form-registrar-participantes/form-registrar-participantes.component';
import { TablaRegistrarValidacionSolicitudesContractialesComponent } from './components/tabla-registrar-validacion-solicitudes-contractiales/tabla-validacion-solicitudes-contractiales.component';
import { TablaRegistrarOtrosTemasComponent } from './components/tabla-registrar-otros-temas/tabla-registrar-otros-temas.component';
import { VotacionSolicitudComponent } from './components/votacion-solicitud/votacion-solicitud.component';
import { AplazarSesionComponent } from './components/aplazar-sesion/aplazar-sesion.component';
import { CrearActaComponent } from './components/crear-acta/crear-acta.component';
import { FormSolicitudComponent } from './components/form-solicitud/form-solicitud.component';
import { VerificarCumplimientoComponent } from './components/verificar-cumplimiento/verificar-cumplimiento.component';
import { TablaSesionComiteFiduciarioComponent } from './components/tabla-sesion-comite-fiduciario/tabla-sesion-comite-fiduciario.component';
import { RegistrarSesionComiteFiduciarioComponent } from './components/registrar-sesion-comite-fiduciario/registrar-sesion-comite-fiduciario.component';
import { TablaSesionesComponent } from './components/tabla-sesiones/tabla-sesiones.component';
import { TablaVerificarCumplimientosComponent } from './components/tabla-verificar-cumplimientos/tabla-verificar-cumplimientos.component';
import { VerDetallesComponent } from './components/ver-detalles/ver-detalles.component';
import { FormOtrosTemasComponent } from './components/form-otros-temas/form-otros-temas.component';
import { TablaFormSolicitudMultipleComponent } from './components/tabla-form-solicitud-multiple/tabla-form-solicitud-multiple.component';
import { VotacionSolicitudMultipleComponent } from './components/votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import { tablaComentariosActaComponent } from './components/tabla-comentarios-acta/tabla-comentarios-acta.component'

@NgModule({
  declarations: [
    ComiteFiduciarioComponent,
    CrearOrdenDelDiaComponent,
    TablaSesionesComponent,
    TablaSolicitudesContractualesComponent,
    TablaOrdenDelDiaComponent,
    TablaSesionComiteFiduciarioComponent,
    TablaGestionActasComponent,
    TablaMonitoreoCompromisosComponent,
    RegistrarSesionComiteFiduciarioComponent,
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
    TablaVerificarCumplimientosComponent,
    VerDetallesComponent,
    FormOtrosTemasComponent,
    TablaFormSolicitudMultipleComponent,
    VotacionSolicitudMultipleComponent,
    tablaComentariosActaComponent
  ],
  imports: [
    CommonModule,
    ComiteFiduciarioRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    QuillModule.forRoot()
  ]
})
export class ComiteFiduciarioModule { }