import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GestionarTramiteLiqContractualRoutingModule } from './gestionar-tramite-liq-contractual-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { GestionarTramiteLiqContractualComponent } from './components/gestionar-tramite-liq-contractual/gestionar-tramite-liq-contractual.component';
import { TablaLiquidacionObraGtlcComponent } from './components/tabla-liquidacion-obra-gtlc/tabla-liquidacion-obra-gtlc.component';
import { TablaLiquidacionIntervnGtlcComponent } from './components/tabla-liquidacion-intervn-gtlc/tabla-liquidacion-intervn-gtlc.component';
import { VerificarRequisitosGtlcComponent } from './components/verificar-requisitos-gtlc/verificar-requisitos-gtlc.component';
import { ActualizacionPolizaGtlcComponent } from './components/actualizacion-poliza-gtlc/actualizacion-poliza-gtlc.component';
import { TablaBalanceFinancieroGtlcComponent } from './components/tabla-balance-financiero-gtlc/tabla-balance-financiero-gtlc.component';
import { VerificarBalanceGtlcComponent } from './components/verificar-balance-gtlc/verificar-balance-gtlc.component';
import { RecursosComproPagadosGtlcComponent } from './components/recursos-compro-pagados-gtlc/recursos-compro-pagados-gtlc.component';
import { AcordionRecursosComproPagadosGtlcComponent } from './components/acordion-recursos-compro-pagados-gtlc/acordion-recursos-compro-pagados-gtlc.component';
import { ListaContratistasGtlcComponent } from './components/lista-contratistas-gtlc/lista-contratistas-gtlc.component';
import { TablaValtotalOgGtlcComponent } from './components/tabla-valtotal-og-gtlc/tabla-valtotal-og-gtlc.component';
import { TablaFuentesUsosGtlcComponent } from './components/tabla-fuentes-usos-gtlc/tabla-fuentes-usos-gtlc.component';
import { DetalleOgGtlcComponent } from './components/detalle-og-gtlc/detalle-og-gtlc.component';
import { TablaFacturadoOgGtlcComponent } from './components/tabla-facturado-og-gtlc/tabla-facturado-og-gtlc.component';
import { TablaDescuentosOgGtlcComponent } from './components/tabla-descuentos-og-gtlc/tabla-descuentos-og-gtlc.component';
import { TablaOtrosDescuentosOgGtlcComponent } from './components/tabla-otros-descuentos-og-gtlc/tabla-otros-descuentos-og-gtlc.component';
import { EjecucionFinancieraGtlcComponent } from './components/ejecucion-financiera-gtlc/ejecucion-financiera-gtlc.component';
import { TablaEjpresupuestalGtlcComponent } from './components/tabla-ejpresupuestal-gtlc/tabla-ejpresupuestal-gtlc.component';
import { TablaEjfinancieraGtlcComponent } from './components/tabla-ejfinanciera-gtlc/tabla-ejfinanciera-gtlc.component';
import { TrasladoRecursosGtlcComponent } from './components/traslado-recursos-gtlc/traslado-recursos-gtlc.component';
import { DetalleTrasladoGtlcComponent } from './components/detalle-traslado-gtlc/detalle-traslado-gtlc.component';
import { TablaTrasladoGtlcComponent } from './components/tabla-traslado-gtlc/tabla-traslado-gtlc.component';
import { DatosSolicitudGtlcComponent } from './components/datos-solicitud-gtlc/datos-solicitud-gtlc.component';
import { DatosDdpDrpGtlcComponent } from './components/datos-ddp-drp-gtlc/datos-ddp-drp-gtlc.component';
import { TablaPorcParticipacionGtlcComponent } from './components/tabla-porc-participacion-gtlc/tabla-porc-participacion-gtlc.component';
import { TablaInforecursosGtlcComponent } from './components/tabla-inforecursos-gtlc/tabla-inforecursos-gtlc.component';
import { TablaDrpGtlcComponent } from './components/tabla-drp-gtlc/tabla-drp-gtlc.component';
import { TablaInformeFinalGtlcComponent } from './components/tabla-informe-final-gtlc/tabla-informe-final-gtlc.component';
import { VerificarInformeGtlcComponent } from './components/verificar-informe-gtlc/verificar-informe-gtlc.component';
import { VerDetalleVerificacionGtlcComponent } from './components/ver-detalle-verificacion-gtlc/ver-detalle-verificacion-gtlc.component';
import { DetalleActuPolizaGtlcComponent } from './components/detalle-actu-poliza-gtlc/detalle-actu-poliza-gtlc.component';
import { DetalleBalanceFinancGtlcComponent } from './components/detalle-balance-financ-gtlc/detalle-balance-financ-gtlc.component';
import { DetalleInformeFinalGtlcComponent } from './components/detalle-informe-final-gtlc/detalle-informe-final-gtlc.component';
import { VerDetalleEditarVerificacionComponent } from './components/ver-detalle-editar-verificacion/ver-detalle-editar-verificacion.component';
import { TablaInformeAnexosComponent } from './components/tabla-informe-anexos/tabla-informe-anexos.component';
import { ObservacionesSupervisorComponent } from './components/observaciones-supervisor/observaciones-supervisorcomponent';
import { ObservacionesInformeFinalComponent } from './components/observaciones-informe-final/observaciones-informe-final.component';
import { FormObservacionActualizacionPolizaComponent } from './components/form-observacion-actualizacion-poliza/form-observacion-actualizacion-poliza.component';
import { FormObservacionBalanceComponent } from './components/form-observacion-balance/form-observacion-balance.component';
import { RegistrarTrasladoGbftrecComponent } from './components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component';
import { FormOrdenGiroComponent } from './components/form-orden-giro/form-orden-giro.component';
import { DatosDdpDrpGbftrecComponent } from './components/datos-ddp-drp-gbftrec/datos-ddp-drp-gbftrec.component';
import { DatosSolicitudGbftrecComponent } from './components/datos-solicitud-gbftrec/datos-solicitud-gbftrec.component';
import { TablaInfofrecursosGbftrecComponent } from './components/tabla-infofrecursos-gbftrec/tabla-infofrecursos-gbftrec.component';
import { TablaPorcParticipacionGbftrecComponent } from './components/tabla-porc-participacion-gbftrec/tabla-porc-participacion-gbftrec.component';
import { FormTerceroCausacionComponent } from './components/form-tercero-causacion/form-tercero-causacion.component';
import { FormDescuentosDireccionTecnicaComponent } from './components/form-descuentos-direccion-tecnica/form-descuentos-direccion-tecnica.component';
import { FormOrdenGiroSeleccionadaComponent } from './components/form-orden-giro-seleccionada/form-orden-giro-seleccionada.component';
import { ControlSolicitudesTrasladoGbftrecComponent } from './components/control-solicitudes-traslado-gbftrec/control-solicitudes-traslado-gbftrec.component';
import { VerLiberacionSaldosComponent } from './components/ver-liberacion-saldos/ver-liberacion-saldos.component';
import { TablaLiberacionSaldoComponent } from './components/tabla-liberacion-saldo/tabla-liberacion-saldo.component';


@NgModule({
  declarations: [
    GestionarTramiteLiqContractualComponent,
    TablaLiquidacionObraGtlcComponent,
    TablaLiquidacionIntervnGtlcComponent,
    VerificarRequisitosGtlcComponent,
    ActualizacionPolizaGtlcComponent,
    TablaBalanceFinancieroGtlcComponent,
    VerificarBalanceGtlcComponent,
    RecursosComproPagadosGtlcComponent,
    AcordionRecursosComproPagadosGtlcComponent,
    ListaContratistasGtlcComponent,
    TablaValtotalOgGtlcComponent,
    TablaFuentesUsosGtlcComponent,
    DetalleOgGtlcComponent,
    TablaFacturadoOgGtlcComponent,
    TablaDescuentosOgGtlcComponent,
    TablaOtrosDescuentosOgGtlcComponent,
    EjecucionFinancieraGtlcComponent,
    TablaEjpresupuestalGtlcComponent,
    TablaEjfinancieraGtlcComponent,
    TrasladoRecursosGtlcComponent,
    DetalleTrasladoGtlcComponent,
    TablaTrasladoGtlcComponent,
    DatosSolicitudGtlcComponent,
    DatosDdpDrpGtlcComponent,
    TablaPorcParticipacionGtlcComponent,
    TablaInforecursosGtlcComponent,
    TablaDrpGtlcComponent,
    TablaInformeFinalGtlcComponent,
    VerificarInformeGtlcComponent,
    VerDetalleVerificacionGtlcComponent,
    DetalleActuPolizaGtlcComponent,
    DetalleBalanceFinancGtlcComponent,
    DetalleInformeFinalGtlcComponent,
    VerDetalleEditarVerificacionComponent,
    TablaInformeAnexosComponent,
    ObservacionesSupervisorComponent,
    ObservacionesInformeFinalComponent,
    FormObservacionActualizacionPolizaComponent,
    FormObservacionBalanceComponent,
    RegistrarTrasladoGbftrecComponent,
    ControlSolicitudesTrasladoGbftrecComponent,
    FormOrdenGiroComponent,
    DatosDdpDrpGbftrecComponent,
    DatosSolicitudGbftrecComponent,
    TablaInfofrecursosGbftrecComponent,
    TablaPorcParticipacionGbftrecComponent,
    FormTerceroCausacionComponent,
    FormDescuentosDireccionTecnicaComponent,
    FormOrdenGiroSeleccionadaComponent,
    VerLiberacionSaldosComponent,
    TablaLiberacionSaldoComponent
  ],
  imports: [
    CommonModule,
    GestionarTramiteLiqContractualRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
  ]
})
export class GestionarTramiteLiqContractualModule { }
