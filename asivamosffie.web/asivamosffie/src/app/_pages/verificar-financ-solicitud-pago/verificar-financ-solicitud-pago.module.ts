import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { VerificarFinancSolicitudPagoComponent } from './components/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.component';
import { VerificarFinancSolicitudPagoRoutingModule } from './verificar-financ-solicitud-pago-routing.module';

@NgModule({
  declarations: [VerificarFinancSolicitudPagoComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    VerificarFinancSolicitudPagoRoutingModule
  ]
})
export class VerificarFinancSolicitudPagoModule { }
