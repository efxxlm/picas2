import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AprobarSolicitudesPagoRoutingModule } from './aprobar-solicitudes-pago-routing.module';
import { AprobarSolicitudesPagoComponent } from './components/aprobar-solicitudes-pago/aprobar-solicitudes-pago.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { FormAprobarSolicitudComponent } from './components/form-aprobar-solicitud/form-aprobar-solicitud.component';
import { DialogProyectosAsociadosAprobComponent } from './components/dialog-proyectos-asociados-aprob/dialog-proyectos-asociados-aprob.component';
import { ObsCargarFormaPagoComponent } from './components/obs-cargar-forma-pago/obs-cargar-forma-pago.component';
import { ObsRegistrarSolicitudPagoComponent } from './components/obs-registrar-solicitud-pago/obs-registrar-solicitud-pago.component';
import { ObsCriterioPagosComponent } from './components/obs-criterio-pagos/obs-criterio-pagos.component';
import { ObsDetllFactProcAsociadosComponent } from './components/obs-detll-fact-proc-asociados/obs-detll-fact-proc-asociados.component';
import { ObsDatosFacturaComponent } from './components/obs-datos-factura/obs-datos-factura.component';
import { ObsDescuentosDirTecnicaComponent } from './components/obs-descuentos-dir-tecnica/obs-descuentos-dir-tecnica.component';
import { ObsSoporteSolicitudComponent } from './components/obs-soporte-solicitud/obs-soporte-solicitud.component';
import { ObsValidListachequeoComponent } from './components/obs-valid-listachequeo/obs-valid-listachequeo.component';
import { FormEditAprobarSolicitudComponent } from './components/form-edit-aprobar-solicitud/form-edit-aprobar-solicitud.component';
import { TablaHistorialObservacionesSolpagoComponent } from './components/tabla-historial-observaciones-solpago/tabla-historial-observaciones-solpago.component';
import { VerDetalleAprobarSolicitudComponent } from './components/ver-detalle-aprobar-solicitud/ver-detalle-aprobar-solicitud.component';
import { DetalleSolicitudPagoComponent } from './components/detalle-solicitud-pago/detalle-solicitud-pago.component';
import { ViewDetllFactProcAsociadosComponent } from './components/view-detll-fact-proc-asociados/view-detll-fact-proc-asociados.component';
import { DetalleValidarListchequeoComponent } from './components/detalle-validar-listchequeo/detalle-validar-listchequeo.component';
import { DialogEnvioAutorizacionComponent } from './components/dialog-envio-autorizacion/dialog-envio-autorizacion.component';



@NgModule({
  declarations: [AprobarSolicitudesPagoComponent, FormAprobarSolicitudComponent, DialogProyectosAsociadosAprobComponent, ObsCargarFormaPagoComponent, ObsRegistrarSolicitudPagoComponent, ObsCriterioPagosComponent, ObsDetllFactProcAsociadosComponent, ObsDatosFacturaComponent, ObsDescuentosDirTecnicaComponent, ObsSoporteSolicitudComponent, ObsValidListachequeoComponent, FormEditAprobarSolicitudComponent, TablaHistorialObservacionesSolpagoComponent, VerDetalleAprobarSolicitudComponent, DetalleSolicitudPagoComponent, ViewDetllFactProcAsociadosComponent, DetalleValidarListchequeoComponent, DialogEnvioAutorizacionComponent],
  imports: [
    CommonModule,
    AprobarSolicitudesPagoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot()
  ]
})
export class AprobarSolicitudesPagoModule { }
