import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { VerificarRequisitosConstruccionRoutingModule } from './verificar-requisitos-construccion-routing.module';
import { VerificarRequisitosConstruccionComponent } from './components/verificar-requisitos-construccion/verificar-requisitos-construccion.component';

@NgModule({
  declarations: [VerificarRequisitosConstruccionComponent],
  imports: [
    CommonModule,
    MaterialModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
    VerificarRequisitosConstruccionRoutingModule
  ]
})
export class VerificarRequisitosConstruccionModule { }
