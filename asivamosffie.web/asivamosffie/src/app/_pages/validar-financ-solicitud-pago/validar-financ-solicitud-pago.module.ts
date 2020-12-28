import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { ValidarFinancSolicitudPagoRoutingModule } from './validar-financ-solicitud-pago-routing.module';
import { ValidarFinancSolicitudPagoComponent } from './components/validar-financ-solicitud-pago/validar-financ-solicitud-pago.component';
import { FormValidarSolicitudValidfspComponent } from './components/form-validar-solicitud-validfsp/form-validar-solicitud-validfsp.component';
import { DialogProyectosAsociadosValidfspComponent } from './components/dialog-proyectos-asociados-validfsp/dialog-proyectos-asociados-validfsp.component';
import { DetalleRegSolPagoValidfspComponent } from './components/detalle-reg-sol-pago-validfsp/detalle-reg-sol-pago-validfsp.component';
import { FormValidListchequeoValidfspComponent } from './components/form-valid-listchequeo-validfsp/form-valid-listchequeo-validfsp.component';



@NgModule({
  declarations: [ValidarFinancSolicitudPagoComponent, FormValidarSolicitudValidfspComponent, DialogProyectosAsociadosValidfspComponent, DetalleRegSolPagoValidfspComponent, FormValidListchequeoValidfspComponent],
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
