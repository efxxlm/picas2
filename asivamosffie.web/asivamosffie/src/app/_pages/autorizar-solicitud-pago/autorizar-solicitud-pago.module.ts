import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { AutorizarSolicitudPagoRoutingModule } from './autorizar-solicitud-pago-routing.module';
import { AutorizarSolicitudPagoComponent } from './components/autorizar-solicitud-pago/autorizar-solicitud-pago.component';
import { FormAutorizarSolicitudComponent } from './components/form-autorizar-solicitud/form-autorizar-solicitud.component';
import { ObsCargarFormpagoAutorizComponent } from './components/obs-cargar-formpago-autoriz/obs-cargar-formpago-autoriz.component';
import { ObsRegistrarSolPagoAutorizComponent } from './components/obs-registrar-sol-pago-autoriz/obs-registrar-sol-pago-autoriz.component';
import { ObsCriterioPagosAutorizComponent } from './components/obs-criterio-pagos-autoriz/obs-criterio-pagos-autoriz.component';
import { ObsDetllfactProcasocAutorizComponent } from './components/obs-detllfact-procasoc-autoriz/obs-detllfact-procasoc-autoriz.component';



@NgModule({
  declarations: [AutorizarSolicitudPagoComponent, FormAutorizarSolicitudComponent, ObsCargarFormpagoAutorizComponent, ObsRegistrarSolPagoAutorizComponent, ObsCriterioPagosAutorizComponent, ObsDetllfactProcasocAutorizComponent],
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
