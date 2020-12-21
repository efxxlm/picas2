import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { AutorizarSolicitudPagoRoutingModule } from './autorizar-solicitud-pago-routing.module';
import { AutorizarSolicitudPagoComponent } from './components/autorizar-solicitud-pago/autorizar-solicitud-pago.component';



@NgModule({
  declarations: [AutorizarSolicitudPagoComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    AutorizarSolicitudPagoRoutingModule
  ]
})
export class AutorizarSolicitudPagoModule { }
