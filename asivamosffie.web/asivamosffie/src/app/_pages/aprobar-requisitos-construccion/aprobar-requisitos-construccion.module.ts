import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { AprobarRequisitosConstruccionRoutingModule } from './aprobar-requisitos-construccion-routing.module';
import { AprobarRequisitosConstruccionComponent } from './components/aprobar-requisitos-construccion/aprobar-requisitos-construccion.component';
import { TablaContratoObraArtcComponent } from './components/tabla-contrato-obra-artc/tabla-contrato-obra-artc.component';
import { TablaContratoInterventoriaArtcComponent } from './components/tabla-contrato-interventoria-artc/tabla-contrato-interventoria-artc.component';
import { FormValidacionRequisitosObraArtcComponent } from './components/form-validacion-requisitos-obra-artc/form-validacion-requisitos-obra-artc.component';
import { DiagnosticoArtcComponent } from './components/diagnostico-artc/diagnostico-artc.component';
import { PlanesProgramasArtcComponent } from './components/planes-programas-artc/planes-programas-artc.component';
import { ManejoAnticipoArtcComponent } from './components/manejo-anticipo-artc/manejo-anticipo-artc.component';
import { HojasVidaContratistaArtcComponent } from './components/hojas-vida-contratista-artc/hojas-vida-contratista-artc.component';
import { ProgramacionObraArtcComponent } from './components/programacion-obra-artc/programacion-obra-artc.component';
import { FlujoInversionRecursosArtcComponent } from './components/flujo-inversion-recursos-artc/flujo-inversion-recursos-artc.component';
import { VerDetalleContratoObraArtcComponent } from './components/ver-detalle-contrato-obra-artc/ver-detalle-contrato-obra-artc.component';
import { FormValidacionRequisitosInterventoriaArtcComponent } from './components/form-validacion-requisitos-interventoria-artc/form-validacion-requisitos-interventoria-artc.component';
import { VerDetalleContratoInterventoriaArtcComponent } from './components/ver-detalle-contrato-interventoria-artc/ver-detalle-contrato-interventoria-artc.component';
import { HojasVidaInterventoriaArtcComponent } from './components/hojas-vida-interventoria-artc/hojas-vida-interventoria-artc.component';



@NgModule({
  declarations: [AprobarRequisitosConstruccionComponent, TablaContratoObraArtcComponent, TablaContratoInterventoriaArtcComponent, FormValidacionRequisitosObraArtcComponent, DiagnosticoArtcComponent, PlanesProgramasArtcComponent, ManejoAnticipoArtcComponent, HojasVidaContratistaArtcComponent, ProgramacionObraArtcComponent, FlujoInversionRecursosArtcComponent, VerDetalleContratoObraArtcComponent, FormValidacionRequisitosInterventoriaArtcComponent, VerDetalleContratoInterventoriaArtcComponent, HojasVidaInterventoriaArtcComponent],
  imports: [
    CommonModule,
    MaterialModule,
    AprobarRequisitosConstruccionRoutingModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
  ]
})
export class AprobarRequisitosConstruccionModule { }
