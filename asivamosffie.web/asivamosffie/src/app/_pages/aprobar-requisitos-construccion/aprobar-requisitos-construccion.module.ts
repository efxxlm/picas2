import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'src/app/material/material.module';
import { QuillModule } from 'ngx-quill';
import { ReactiveFormsModule } from '@angular/forms';
import { AprobarRequisitosConstruccionRoutingModule } from './aprobar-requisitos-construccion-routing.module';
import { AprobarRequisitosConstruccionComponent } from './components/aprobar-requisitos-construccion/aprobar-requisitos-construccion.component';



@NgModule({
  declarations: [AprobarRequisitosConstruccionComponent],
  imports: [
    CommonModule,
    MaterialModule,
    AprobarRequisitosConstruccionRoutingModule,
    QuillModule.forRoot(),
    ReactiveFormsModule,
  ]
})
export class AprobarRequisitosConstruccionModule { }
