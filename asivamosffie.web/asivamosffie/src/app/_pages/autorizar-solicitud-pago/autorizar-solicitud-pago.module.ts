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
import { ObsDatosFacturaAutorizComponent } from './components/obs-datos-factura-autoriz/obs-datos-factura-autoriz.component';
import { ObsDescDirTecnicaAutorizComponent } from './components/obs-desc-dir-tecnica-autoriz/obs-desc-dir-tecnica-autoriz.component';
import { ObsSoporteurlAutorizComponent } from './components/obs-soporteurl-autoriz/obs-soporteurl-autoriz.component';
import { ObsValidListchqeoAutorizComponent } from './components/obs-valid-listchqeo-autoriz/obs-valid-listchqeo-autoriz.component';
import { ObsCertAutorizacionAutorizComponent } from './components/obs-cert-autorizacion-autoriz/obs-cert-autorizacion-autoriz.component';
import { FormEditAutorizarSolicitudComponent } from './components/form-edit-autorizar-solicitud/form-edit-autorizar-solicitud.component';
import { DialogProyectosAsociadosAutorizComponent } from './components/dialog-proyectos-asociados-autoriz/dialog-proyectos-asociados-autoriz.component';
import { TablaObservacionesAutorizComponent } from './components/tabla-observaciones-autoriz/tabla-observaciones-autoriz.component';
import { VerDetalleAutorizarSolicitudComponent } from './components/ver-detalle-autorizar-solicitud/ver-detalle-autorizar-solicitud.component';
import { DetalleSoliPagoAutorizComponent } from './components/detalle-soli-pago-autoriz/detalle-soli-pago-autoriz.component';
import { DetalleValidListchqAutorizComponent } from './components/detalle-valid-listchq-autoriz/detalle-valid-listchq-autoriz.component';



@NgModule({
  declarations: [AutorizarSolicitudPagoComponent, FormAutorizarSolicitudComponent, ObsCargarFormpagoAutorizComponent, ObsRegistrarSolPagoAutorizComponent, ObsCriterioPagosAutorizComponent, ObsDetllfactProcasocAutorizComponent, ObsDatosFacturaAutorizComponent, ObsDescDirTecnicaAutorizComponent, ObsSoporteurlAutorizComponent, ObsValidListchqeoAutorizComponent, ObsCertAutorizacionAutorizComponent, FormEditAutorizarSolicitudComponent, DialogProyectosAsociadosAutorizComponent, TablaObservacionesAutorizComponent, VerDetalleAutorizarSolicitudComponent, DetalleSoliPagoAutorizComponent, DetalleValidListchqAutorizComponent],
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
