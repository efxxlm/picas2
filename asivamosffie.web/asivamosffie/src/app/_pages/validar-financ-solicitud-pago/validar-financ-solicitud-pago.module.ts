import { DescripcionFacturaComponent } from './components/descripcion-factura/descripcion-factura.component';
import { FormAmortizacionComponent } from './components/form-amortizacion/form-amortizacion.component';
import { FormProyectoComponent } from './components/form-proyecto/form-proyecto.component';
import { ObsRegistrarSolicitudPagoComponent } from './components/obs-registrar-solicitud-pago/obs-registrar-solicitud-pago.component';
import { ObsDatosFacturaComponent } from './components/obs-datos-factura/obs-datos-factura.component';
import { ObsCriterioPagosComponent } from './components/obs-criterio-pagos/obs-criterio-pagos.component';
import { DialogObservacionesVfspComponent } from './components/dialog-observaciones-vfsp/dialog-observaciones-vfsp.component';
import { FormValidListchequeoVfspComponent } from './components/form-valid-listchequeo-vfsp/form-valid-listchequeo-vfsp.component';
import { VerDetalleExpensasComponent } from './components/ver-detalle-expensas/ver-detalle-expensas.component';
import { VerDetalleEditarExpensasComponent } from './components/ver-detalle-editar-expensas/ver-detalle-editar-expensas.component';
import { RegistrarSolicitudPagoComponent } from './components/registrar-solicitud-pago/registrar-solicitud-pago.component';
import { FormSolicitudExpensasComponent } from './components/form-solicitud-expensas/form-solicitud-expensas.component';
import { DetalleFacturaProyectosComponent } from './components/detalle-factura-proyectos/detalle-factura-proyectos.component';
import { DescuentosDireccionTecnicaComponent } from './components/descuentos-direccion-tecnica/descuentos-direccion-tecnica.component';
import { DatosFacturaComponent } from './components/datos-factura/datos-factura.component';
import { CriteriosPagoComponent } from './components/criterios-pago/criterios-pago.component';
import { AmortizacionPagoComponent } from './components/amortizacion-pago/amortizacion-pago.component';
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
import { DetalleFactProcasValidfspComponent } from './components/detalle-fact-procas-validfsp/detalle-fact-procas-validfsp.component';
import { FormEditValidarSolicitudValidfspComponent } from './components/form-edit-validar-solicitud-validfsp/form-edit-validar-solicitud-validfsp.component';
import { DialogRechazarSolicitudValidfspComponent } from './components/dialog-rechazar-solicitud-validfsp/dialog-rechazar-solicitud-validfsp.component';
import { VerdetalleValidfspComponent } from './components/verdetalle-validfsp/verdetalle-validfsp.component';
import { DialogObservacionesValidfspComponent } from './components/dialog-observaciones-validfsp/dialog-observaciones-validfsp.component';
import { DetalleValidListchqValidfspComponent } from './components/detalle-valid-listchq-validfsp/detalle-valid-listchq-validfsp.component';



@NgModule({
  declarations: [ ObsCriterioPagosComponent, ObsDatosFacturaComponent, ObsRegistrarSolicitudPagoComponent, FormProyectoComponent, FormAmortizacionComponent, DescripcionFacturaComponent, DialogObservacionesVfspComponent, FormValidListchequeoVfspComponent, VerDetalleExpensasComponent, VerDetalleEditarExpensasComponent, RegistrarSolicitudPagoComponent, FormSolicitudExpensasComponent, DetalleFacturaProyectosComponent, DescuentosDireccionTecnicaComponent, DatosFacturaComponent, CriteriosPagoComponent, AmortizacionPagoComponent, ValidarFinancSolicitudPagoComponent, FormValidarSolicitudValidfspComponent, DialogProyectosAsociadosValidfspComponent, DetalleRegSolPagoValidfspComponent, FormValidListchequeoValidfspComponent, DetalleFactProcasValidfspComponent, FormEditValidarSolicitudValidfspComponent, DialogRechazarSolicitudValidfspComponent, VerdetalleValidfspComponent, DialogObservacionesValidfspComponent, DetalleValidListchqValidfspComponent],
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
