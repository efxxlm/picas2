import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarValidarRequisitosPagoComponent } from './components/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.component';
import { RegistrarValidarRequisitosPagoRoutingModule } from './registrar-validar-requisitos-pago-routing.module';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';



@NgModule({
  declarations: [RegistrarValidarRequisitosPagoComponent],
  imports: [
    CommonModule,
    RegistrarValidarRequisitosPagoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class RegistrarValidarRequisitosPagoModule { }
