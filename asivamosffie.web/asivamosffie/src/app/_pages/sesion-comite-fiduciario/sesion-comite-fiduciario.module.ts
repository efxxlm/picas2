import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SesionComiteFiduciarioRoutingModule } from './sesion-comite-fiduciario-routing.module';
import { SesionComiteFiduciarioComponent } from './components/sesion-comite-fiduciario/sesion-comite-fiduciario.component';
import { MaterialModule } from '../../material/material.module';
import { OrdenesDiaComponent } from './components/ordenes-dia/ordenes-dia.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CrearOrdenComponent } from './components/crear-orden/crear-orden.component';
import { TablaSesionesComponent } from './components/tabla-sesiones/tabla.component';
import { TablaOrdenesComponent } from './components/tabla-ordenes/tabla-ordenes.component';
import { SesionesConvocadasComponent } from './components/sesiones-convocadas/sesiones-convocadas.component';
import { EditarOrdenComponent } from './components/editar-orden/editar-orden.component';
import { TablaEditOrdenComponent } from './components/tabla-edit-orden/tabla-edit-orden.component';
import { RegistrarSesionComponent } from './components/registrar-sesion/registrar-sesion.component';
import { TablaValidacionesContractualesComponent } from './components/tabla-validaciones-contractuales/tabla-validaciones-contractuales.component';
import { FormNuevoTemaComponent } from './components/form-nuevo-tema/form-nuevo-tema.component';

@NgModule({
  declarations: [
    SesionComiteFiduciarioComponent,
    OrdenesDiaComponent,
    CrearOrdenComponent,
    TablaSesionesComponent,
    TablaOrdenesComponent,
    SesionesConvocadasComponent,
    EditarOrdenComponent,
    TablaEditOrdenComponent,
    RegistrarSesionComponent,
    TablaValidacionesContractualesComponent,
    FormNuevoTemaComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    SesionComiteFiduciarioRoutingModule
  ]
})
export class SesionComiteFiduciarioModule { }
