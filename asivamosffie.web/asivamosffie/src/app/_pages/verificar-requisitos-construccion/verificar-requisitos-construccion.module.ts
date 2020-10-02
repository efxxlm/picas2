import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { VerificarRequisitosConstruccionRoutingModule } from './verificar-requisitos-construccion-routing.module';
import { VerificarRequisitosConstruccionComponent } from './components/verificar-requisitos-construccion/verificar-requisitos-construccion.component';
import { TablaContratosObraVrtcComponent } from './components/tabla-contratos-obra-vrtc/tabla-contratos-obra-vrtc.component';
import { TablaContratosInterventoriaVrtcComponent } from './components/tabla-contratos-interventoria-vrtc/tabla-contratos-interventoria-vrtc.component';
import { FormVerificacionRequisitosComponent } from './components/form-verificacion-requisitos/form-verificacion-requisitos.component';
import { DiagnosticoVerificarRequisitosComponent } from './components/diagnostico-verificar-requisitos/diagnostico-verificar-requisitos.component';

@NgModule({
  declarations: [VerificarRequisitosConstruccionComponent, TablaContratosObraVrtcComponent, TablaContratosInterventoriaVrtcComponent, FormVerificacionRequisitosComponent, DiagnosticoVerificarRequisitosComponent],
  imports: [
    CommonModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    VerificarRequisitosConstruccionRoutingModule
  ]
})
export class VerificarRequisitosConstruccionModule { }
