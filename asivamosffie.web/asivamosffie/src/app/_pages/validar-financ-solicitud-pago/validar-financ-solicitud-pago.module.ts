import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { ValidarFinancSolicitudPagoRoutingModule } from './validar-financ-solicitud-pago-routing.module';
import { ValidarFinancSolicitudPagoComponent } from './components/validar-financ-solicitud-pago/validar-financ-solicitud-pago.component';



@NgModule({
  declarations: [ValidarFinancSolicitudPagoComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    ValidarFinancSolicitudPagoRoutingModule
  ]
})
export class ValidarFinancSolicitudPagoModule { }
