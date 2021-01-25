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
import { DetalleFacturaProyectosAsociadosComponent } from './components/detalle-factura-proyectos-asociados/detalle-factura-proyectos-asociados.component';
import { FormDatosFacturaComponent } from './components/form-datos-factura/form-datos-factura.component';
import { FormDescuentosDireccionTecnicaComponent } from './components/form-descuentos-direccion-tecnica/form-descuentos-direccion-tecnica.component';
import { FormSoporteSolicitudUrlComponent } from './components/form-soporte-solicitud-url/form-soporte-solicitud-url.component';
import { ValidarListaChequeoComponent } from './components/validar-lista-chequeo/validar-lista-chequeo.component';
import { DialogObservacionesItemListchequeoComponent } from './components/dialog-observaciones-item-listchequeo/dialog-observaciones-item-listchequeo.component';
import { DialogDevolverSolicitudComponent } from './components/dialog-devolver-solicitud/dialog-devolver-solicitud.component';
import { VerdetalleEditarSolicitudPagoComponent } from './components/verdetalle-editar-solicitud-pago/verdetalle-editar-solicitud-pago.component';
import { VerdetalleSolicitudPagoComponent } from './components/verdetalle-solicitud-pago/verdetalle-solicitud-pago.component';
import { TablaDetalleSolicitudPagoComponent } from './components/tabla-detalle-solicitud-pago/tabla-detalle-solicitud-pago.component';
import { TablaDetalleValidarListachequeoComponent } from './components/tabla-detalle-validar-listachequeo/tabla-detalle-validar-listachequeo.component';
import { FormAmortizacionAnticipoComponent } from './components/form-amortizacion-anticipo/form-amortizacion-anticipo.component';
import { DialogSubsanacionComponent } from './components/dialog-subsanacion/dialog-subsanacion.component';
import { FormSolicitudExpensasComponent } from './components/form-solicitud-expensas/form-solicitud-expensas.component';
import { FormSolicitudOtrosCostosserviciosComponent } from './components/form-solicitud-otros-costosservicios/form-solicitud-otros-costosservicios.component';
import { FormListachequeoExpensasComponent } from './components/form-listachequeo-expensas/form-listachequeo-expensas.component';
import { DatosFacturaConstruccionRvrpComponent } from './components/datos-factura-construccion-rvrp/datos-factura-construccion-rvrp.component';
import { VerDetalleEditarExpensasComponent } from './components/ver-detalle-editar-expensas/ver-detalle-editar-expensas.component';



@NgModule({
  declarations: [RegistrarValidarRequisitosPagoComponent, RegistrarNuevaSolicitudPagoComponent, DialogProyectosAsociadosComponent, FormCargarFormaDePagoComponent, FormRegistrarSolicitudDePagoComponent, FormCriteriosPagoComponent, DetalleFacturaProyectosAsociadosComponent, FormDatosFacturaComponent, FormDescuentosDireccionTecnicaComponent, FormSoporteSolicitudUrlComponent, ValidarListaChequeoComponent, DialogObservacionesItemListchequeoComponent, DialogDevolverSolicitudComponent, VerdetalleEditarSolicitudPagoComponent, VerdetalleSolicitudPagoComponent, TablaDetalleSolicitudPagoComponent, TablaDetalleValidarListachequeoComponent, FormAmortizacionAnticipoComponent, DialogSubsanacionComponent, FormSolicitudExpensasComponent, FormSolicitudOtrosCostosserviciosComponent, FormListachequeoExpensasComponent, DatosFacturaConstruccionRvrpComponent, VerDetalleEditarExpensasComponent],
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
