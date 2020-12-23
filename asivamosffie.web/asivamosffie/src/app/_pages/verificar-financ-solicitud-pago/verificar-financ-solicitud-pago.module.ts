import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { VerificarFinancSolicitudPagoComponent } from './components/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.component';
import { VerificarFinancSolicitudPagoRoutingModule } from './verificar-financ-solicitud-pago-routing.module';
import { FormVerificarSolicitudVfspComponent } from './components/form-verificar-solicitud-vfsp/form-verificar-solicitud-vfsp.component';
import { DetalleRegSolPagoVfspComponent } from './components/detalle-reg-sol-pago-vfsp/detalle-reg-sol-pago-vfsp.component';
import { FormValidListchequeoVfspComponent } from './components/form-valid-listchequeo-vfsp/form-valid-listchequeo-vfsp.component';
import { DetalleFactProcasVfspComponent } from './components/detalle-fact-procas-vfsp/detalle-fact-procas-vfsp.component';
import { DialogObservacionesVfspComponent } from './components/dialog-observaciones-vfsp/dialog-observaciones-vfsp.component';
import { DialogRechazarSolicitudVfspComponent } from './components/dialog-rechazar-solicitud-vfsp/dialog-rechazar-solicitud-vfsp.component';
import { FormEditVerificarSolicitudVfspComponent } from './components/form-edit-verificar-solicitud-vfsp/form-edit-verificar-solicitud-vfsp.component';
import { VerdetalleVfspComponent } from './components/verdetalle-vfsp/verdetalle-vfsp.component';
import { DialogProyAsociadosVfspComponent } from './components/dialog-proy-asociados-vfsp/dialog-proy-asociados-vfsp.component';

@NgModule({
  declarations: [VerificarFinancSolicitudPagoComponent,FormVerificarSolicitudVfspComponent, DetalleRegSolPagoVfspComponent, FormValidListchequeoVfspComponent, DetalleFactProcasVfspComponent, DialogObservacionesVfspComponent, DialogRechazarSolicitudVfspComponent, FormEditVerificarSolicitudVfspComponent, VerdetalleVfspComponent, DialogProyAsociadosVfspComponent],
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
