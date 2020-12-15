import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarValidarRequisitosPagoComponent } from './components/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.component';
import { RegistrarValidarRequisitosPagoRoutingModule } from './registrar-validar-requisitos-pago-routing.module';
import { MaterialModule } from 'src/app/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill';
import { RegistrarNuevaSolicitudPagoComponent } from './components/registrar-nueva-solicitud-pago/registrar-nueva-solicitud-pago.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { DialogProyectosAsociadosComponent } from './components/dialog-proyectos-asociados/dialog-proyectos-asociados.component';
import { FormCargarFormaDePagoComponent } from './components/form-cargar-forma-de-pago/form-cargar-forma-de-pago.component';
import { FormRegistrarSolicitudDePagoComponent } from './components/form-registrar-solicitud-de-pago/form-registrar-solicitud-de-pago.component';
import { FormCriteriosPagoComponent } from './components/form-criterios-pago/form-criterios-pago.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';



@NgModule({
  declarations: [RegistrarValidarRequisitosPagoComponent, RegistrarNuevaSolicitudPagoComponent, DialogProyectosAsociadosComponent, FormCargarFormaDePagoComponent, FormRegistrarSolicitudDePagoComponent, FormCriteriosPagoComponent],
  imports: [
    CommonModule,
    RegistrarValidarRequisitosPagoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    QuillModule.forRoot(),
    MatAutocompleteModule,
    CurrencyMaskModule
  ]
})
export class RegistrarValidarRequisitosPagoModule { }
