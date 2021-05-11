import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarBalanFinancTraslRecComponent } from './components/gestionar-balan-financ-trasl-rec/gestionar-balan-financ-trasl-rec.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarBalanFinancTraslRecursosRoutingModule } from './gestionar-balan-financ-trasl-recursos-routing.module';
import { ValidarBalanceGbftrecComponent } from './components/validar-balance-gbftrec/validar-balance-gbftrec.component';
import { RecursosCompromPagadosGbftrecComponent } from './components/recursos-comprom-pagados-gbftrec/recursos-comprom-pagados-gbftrec.component';
import { EjecucionFinancieraGbftrecComponent } from './components/ejecucion-financiera-gbftrec/ejecucion-financiera-gbftrec.component';
import { TrasladoRecursosGbftrecComponent } from './components/traslado-recursos-gbftrec/traslado-recursos-gbftrec.component';
import { RecursosCompromAccordGbftrecComponent } from './components/recursos-comprom-accord-gbftrec/recursos-comprom-accord-gbftrec.component';
import { FuentesUsosGbftrecComponent } from './components/fuentes-usos-gbftrec/fuentes-usos-gbftrec.component';
import { ContratistasGbftrecComponent } from './components/contratistas-gbftrec/contratistas-gbftrec.component';
import { TablaValorTotalOgGbftrecComponent } from './components/tabla-valor-total-og-gbftrec/tabla-valor-total-og-gbftrec.component';
import { DetalleOgGbftrecComponent } from './components/detalle-og-gbftrec/detalle-og-gbftrec.component';
import { TablaFacturadoOgGbftrecComponent } from './components/tabla-facturado-og-gbftrec/tabla-facturado-og-gbftrec.component';
import { TablaDescuentosOgGbftrecComponent } from './components/tabla-descuentos-og-gbftrec/tabla-descuentos-og-gbftrec.component';
import { TablaOtrosDescuentosOgGbftrecComponent } from './components/tabla-otros-descuentos-og-gbftrec/tabla-otros-descuentos-og-gbftrec.component';
import { TablaEjpresupuestalGbftrecComponent } from './components/tabla-ejpresupuestal-gbftrec/tabla-ejpresupuestal-gbftrec.component';
import { TablaEjfinancieraGbftrecComponent } from './components/tabla-ejfinanciera-gbftrec/tabla-ejfinanciera-gbftrec.component';
import { VerdetalleeditarBalanceGbftrecComponent } from './components/verdetalleeditar-balance-gbftrec/verdetalleeditar-balance-gbftrec.component';
import { DetalleBalanceGbftrecComponent } from './components/detalle-balance-gbftrec/detalle-balance-gbftrec.component';
import { RegistrarTrasladoGbftrecComponent } from './components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component';
import { ControlSolicitudesTrasladoGbftrecComponent } from './components/control-solicitudes-traslado-gbftrec/control-solicitudes-traslado-gbftrec.component';
import { DatosSolicitudGbftrecComponent } from './components/datos-solicitud-gbftrec/datos-solicitud-gbftrec.component';
import { DatosDdpDrpGbftrecComponent } from './components/datos-ddp-drp-gbftrec/datos-ddp-drp-gbftrec.component';
import { TablaDrpGbftrecComponent } from './components/tabla-drp-gbftrec/tabla-drp-gbftrec.component';
import { TablaPorcParticipacionGbftrecComponent } from './components/tabla-porc-participacion-gbftrec/tabla-porc-participacion-gbftrec.component';
import { TablaInfofrecursosGbftrecComponent } from './components/tabla-infofrecursos-gbftrec/tabla-infofrecursos-gbftrec.component';
import { FormCostoVariableGbftrecComponent } from './components/form-costo-variable-gbftrec/form-costo-variable-gbftrec.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { FormTipoPagoGbftrecComponent } from './components/form-tipo-pago-gbftrec/form-tipo-pago-gbftrec.component';
import { VerdetalleeditarTrasladoGbftrecComponent } from './components/verdetalleeditar-traslado-gbftrec/verdetalleeditar-traslado-gbftrec.component';
import { VerdetalleTrasladoGbftrecComponent } from './components/verdetalle-traslado-gbftrec/verdetalle-traslado-gbftrec.component';
import { TablaTrasladosGbftrecComponent } from './components/tabla-traslados-gbftrec/tabla-traslados-gbftrec.component';
import { TablaDetalleTrasladosGbftrecComponent } from './components/tabla-detalle-traslados-gbftrec/tabla-detalle-traslados-gbftrec.component';
import { DetalleAccordTrasladosGbftrecComponent } from './components/detalle-accord-traslados-gbftrec/detalle-accord-traslados-gbftrec.component';
import { FormOrdenGiroComponent } from './components/form-orden-giro/form-orden-giro.component';
import { FormOrdenGiroSeleccionadaComponent } from './components/form-orden-giro-seleccionada/form-orden-giro-seleccionada.component';
import { FormTerceroCausacionComponent } from './components/form-tercero-causacion/form-tercero-causacion.component';

@NgModule({
  declarations: [GestionarBalanFinancTraslRecComponent, ValidarBalanceGbftrecComponent, RecursosCompromPagadosGbftrecComponent, EjecucionFinancieraGbftrecComponent, TrasladoRecursosGbftrecComponent, RecursosCompromAccordGbftrecComponent, FuentesUsosGbftrecComponent, ContratistasGbftrecComponent, TablaValorTotalOgGbftrecComponent, DetalleOgGbftrecComponent, TablaFacturadoOgGbftrecComponent, TablaDescuentosOgGbftrecComponent, TablaOtrosDescuentosOgGbftrecComponent, TablaEjpresupuestalGbftrecComponent, TablaEjfinancieraGbftrecComponent, VerdetalleeditarBalanceGbftrecComponent, DetalleBalanceGbftrecComponent, RegistrarTrasladoGbftrecComponent, ControlSolicitudesTrasladoGbftrecComponent, DatosSolicitudGbftrecComponent, DatosDdpDrpGbftrecComponent, TablaDrpGbftrecComponent, TablaPorcParticipacionGbftrecComponent, TablaInfofrecursosGbftrecComponent, FormCostoVariableGbftrecComponent, FormTipoPagoGbftrecComponent, VerdetalleeditarTrasladoGbftrecComponent, VerdetalleTrasladoGbftrecComponent, TablaTrasladosGbftrecComponent, TablaDetalleTrasladosGbftrecComponent, DetalleAccordTrasladosGbftrecComponent, FormOrdenGiroComponent, FormOrdenGiroSeleccionadaComponent, FormTerceroCausacionComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
    GestionarBalanFinancTraslRecursosRoutingModule
  ]
})
export class GestionarBalanFinancTraslRecursosModule { }
