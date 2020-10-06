import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { AprobarRequisitosConstruccionRoutingModule } from './aprobar-requisitos-construccion-routing.module';
import { AprobarRequisitosConstruccionComponent } from './components/aprobar-requisitos-construccion/aprobar-requisitos-construccion.component';
import { TablaContratoObraArtcComponent } from './components/tabla-contrato-obra-artc/tabla-contrato-obra-artc.component';
import { TablaContratoInterventoriaArtcComponent } from './components/tabla-contrato-interventoria-artc/tabla-contrato-interventoria-artc.component';



@NgModule({
  declarations: [AprobarRequisitosConstruccionComponent, TablaContratoObraArtcComponent, TablaContratoInterventoriaArtcComponent],
  imports: [
    CommonModule,
    MaterialModule,
    AprobarRequisitosConstruccionRoutingModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
  ]
})
export class AprobarRequisitosConstruccionModule { }
