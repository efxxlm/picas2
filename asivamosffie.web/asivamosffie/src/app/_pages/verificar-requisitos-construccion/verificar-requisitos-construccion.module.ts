import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { VerificarRequisitosConstruccionRoutingModule } from './verificar-requisitos-construccion-routing.module';
import { VerificarRequisitosConstruccionComponent } from './components/verificar-requisitos-construccion/verificar-requisitos-construccion.component';
import { TablaContratosObraVrtcComponent } from './components/tabla-contratos-obra-vrtc/tabla-contratos-obra-vrtc.component';
import { TablaContratosInterventoriaVrtcComponent } from './components/tabla-contratos-interventoria-vrtc/tabla-contratos-interventoria-vrtc.component';
import { FormVerificacionRequisitosComponent } from './components/form-verificacion-requisitos/form-verificacion-requisitos.component';
import { DiagnosticoVerificarRequisitosComponent } from './components/diagnostico-verificar-requisitos/diagnostico-verificar-requisitos.component';
import { PlanesProgramasVerificarRequisitosComponent } from './components/planes-programas-verificar-requisitos/planes-programas-verificar-requisitos.component';
import { ManejoAnticipoVerificarRequisitosComponent } from './components/manejo-anticipo-verificar-requisitos/manejo-anticipo-verificar-requisitos.component';
import { HojasVidaVerificarRequisitosComponent } from './components/hojas-vida-verificar-requisitos/hojas-vida-verificar-requisitos.component';
import { ProgramacionObraVerificarRequisitosComponent } from './components/programacion-obra-verificar-requisitos/programacion-obra-verificar-requisitos.component';
import { InversionFljrecursosVerificarRequisitosComponent } from './components/inversion-fljrecursos-verificar-requisitos/inversion-fljrecursos-verificar-requisitos.component';
import { VerdetalleObraVrtcComponent } from './components/verdetalle-obra-vrtc/verdetalle-obra-vrtc.component';
import { VerdetalleInterventoriaVrtcComponent } from './components/verdetalle-interventoria-vrtc/verdetalle-interventoria-vrtc.component';
import { FormInterventoriaVerificacionRequisitosComponent } from './components/form-interventoria-verificacion-requisitos/form-interventoria-verificacion-requisitos.component';
import { RegistroHojasVidaVrtcComponent } from './components/registro-hojas-vida-vrtc/registro-hojas-vida-vrtc.component';

@NgModule({
  declarations: [VerificarRequisitosConstruccionComponent, TablaContratosObraVrtcComponent, TablaContratosInterventoriaVrtcComponent, FormVerificacionRequisitosComponent, DiagnosticoVerificarRequisitosComponent, PlanesProgramasVerificarRequisitosComponent, ManejoAnticipoVerificarRequisitosComponent, HojasVidaVerificarRequisitosComponent, ProgramacionObraVerificarRequisitosComponent, InversionFljrecursosVerificarRequisitosComponent, VerdetalleObraVrtcComponent, VerdetalleInterventoriaVrtcComponent, FormInterventoriaVerificacionRequisitosComponent, RegistroHojasVidaVrtcComponent],
  imports: [
    CommonModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    FormsModule,
    VerificarRequisitosConstruccionRoutingModule
  ]
})
export class VerificarRequisitosConstruccionModule { }
