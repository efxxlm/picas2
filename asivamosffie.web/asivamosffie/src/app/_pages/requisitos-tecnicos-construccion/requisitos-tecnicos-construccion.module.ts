import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RequisitosTecnicosConstruccionRoutingModule } from './requisitos-tecnicos-construccion-routing.module';
import { RequisitosTecnicosConstruccionComponent } from './components/requisitos-tecnicos-construccion/requisitos-tecnicos-construccion.component';
import { TablaRequisitosTecnicosComponent } from './components/tabla-requisitos-tecnicos/tabla-requisitos-tecnicos.component';
import { MaterialModule } from '../../material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { FormRequisitosTecnicosConstruccionComponent } from './components/form-requisitos-tecnicos-construccion/form-requisitos-tecnicos-construccion.component';
import { QuillModule } from 'ngx-quill';
import { DiagnosticoComponent } from './components/diagnostico/diagnostico.component';
import { PlanesProgramasComponent } from './components/planes-programas/planes-programas.component';
import { DialogObservacionesComponent } from './components/dialog-observaciones/dialog-observaciones.component';
import { ManejoAnticipoComponent } from './components/manejo-anticipo/manejo-anticipo.component';
import { HojaVidaContratistaComponent } from './components/hoja-vida-contratista/hoja-vida-contratista.component';
import { DialogCargarProgramacionComponent } from './components/dialog-cargar-programacion/dialog-cargar-programacion.component';
import { TablaProgramacionObraComponent } from './components/tabla-programacion-obra/tabla-programacion-obra.component';
import { ProgramacionObraFlujoInversionComponent } from './components/programacion-obra-flujo-inversion/programacion-obra-flujo-inversion.component';
import { DialogObservacionesProgramacionComponent } from './components/dialog-observaciones-programacion/dialog-observaciones-programacion.component';
import { TablaInversionRecursosComponent } from './components/tabla-inversion-recursos/tabla-inversion-recursos.component';
import { DialogObservacionesFlujoRecursosComponent } from './components/dialog-observaciones-flujo-recursos/dialog-observaciones-flujo-recursos.component';
import { VerDetalleReqTecConstrComponent } from './components/ver-detalle-req-tec-constr/ver-detalle-req-tec-constr.component';
import { DetalleDiagnosticoComponent } from './components/detalle-diagnostico/detalle-diagnostico.component';
import { DetallePlanesProgramasComponent } from './components/detalle-planes-programas/detalle-planes-programas.component';
import { DetalleManejoAnticipoComponent } from './components/detalle-manejo-anticipo/detalle-manejo-anticipo.component';
import { DetalleHojasVidaContratistaComponent } from './components/detalle-hojas-vida-contratista/detalle-hojas-vida-contratista.component';
import { DetalleTablaProgramacionObraComponent } from './components/detalle-tabla-programacion-obra/detalle-tabla-programacion-obra.component';
import { DetalleTablaFlujoRecursosComponent } from './components/detalle-tabla-flujo-recursos/detalle-tabla-flujo-recursos.component';

@NgModule({
  declarations: [
    RequisitosTecnicosConstruccionComponent, 
    TablaRequisitosTecnicosComponent, FormRequisitosTecnicosConstruccionComponent, DiagnosticoComponent, PlanesProgramasComponent, DialogObservacionesComponent, ManejoAnticipoComponent, HojaVidaContratistaComponent, DialogCargarProgramacionComponent, TablaProgramacionObraComponent, ProgramacionObraFlujoInversionComponent, DialogObservacionesProgramacionComponent, TablaInversionRecursosComponent, DialogObservacionesFlujoRecursosComponent, VerDetalleReqTecConstrComponent, DetalleDiagnosticoComponent, DetallePlanesProgramasComponent, DetalleManejoAnticipoComponent, DetalleHojasVidaContratistaComponent, DetalleTablaProgramacionObraComponent, DetalleTablaFlujoRecursosComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    QuillModule.forRoot(),
    FormsModule,
    RequisitosTecnicosConstruccionRoutingModule
  ]
})
export class RequisitosTecnicosConstruccionModule { }