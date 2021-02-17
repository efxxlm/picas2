import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrarLiquidacionContratoComponent } from './components/registrar-liquidacion-contrato/registrar-liquidacion-contrato.component';
import { RegistrarLiquidacionContratoRoutingModule } from './registrar-liquidacion-contrato-routing.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { QuillModule } from 'ngx-quill';
import { MaterialModule } from 'src/app/material/material.module';
import { TablaSinRegistroLiquidacionRlcComponent } from './components/tabla-sin-registro-liquidacion-rlc/tabla-sin-registro-liquidacion-rlc.component';
import { GestionarSolicitudRlcComponent } from './components/gestionar-solicitud-rlc/gestionar-solicitud-rlc.component';
import { ActualizacionPolizaRlcComponent } from './components/actualizacion-poliza-rlc/actualizacion-poliza-rlc.component';
import { TablaBalanceFinancieroRlcComponent } from './components/tabla-balance-financiero-rlc/tabla-balance-financiero-rlc.component';
import { DetalleBalanceFinancieroRlcComponent } from './components/detalle-balance-financiero-rlc/detalle-balance-financiero-rlc.component';
import { RecursosComproPagadosRlcComponent } from './components/recursos-compro-pagados-rlc/recursos-compro-pagados-rlc.component';
import { AcordionRecursosComproPagadosRlcComponent } from './components/acordion-recursos-compro-pagados-rlc/acordion-recursos-compro-pagados-rlc.component';
import { TablaFuentesUsosRlcComponent } from './components/tabla-fuentes-usos-rlc/tabla-fuentes-usos-rlc.component';
import { ListaContratistasRlcComponent } from './components/lista-contratistas-rlc/lista-contratistas-rlc.component';
import { TablaValtotalOgRlcComponent } from './components/tabla-valtotal-og-rlc/tabla-valtotal-og-rlc.component';
import { DetalleOgRlcComponent } from './components/detalle-og-rlc/detalle-og-rlc.component';
import { TablaFacturadoOgRlcComponent } from './components/tabla-facturado-og-rlc/tabla-facturado-og-rlc.component';
import { TablaDescuentosOgRlcComponent } from './components/tabla-descuentos-og-rlc/tabla-descuentos-og-rlc.component';
import { TablaOtrosDescuentosOgRlcComponent } from './components/tabla-otros-descuentos-og-rlc/tabla-otros-descuentos-og-rlc.component';
import { EjecucionFinancieraRlcComponent } from './components/ejecucion-financiera-rlc/ejecucion-financiera-rlc.component';
import { TablaEjpresupuestalRlcComponent } from './components/tabla-ejpresupuestal-rlc/tabla-ejpresupuestal-rlc.component';
import { TablaEjfinancieraRlcComponent } from './components/tabla-ejfinanciera-rlc/tabla-ejfinanciera-rlc.component';
import { TrasladoRecursosRlcComponent } from './components/traslado-recursos-rlc/traslado-recursos-rlc.component';
import { DetalleTrasladoRlcComponent } from './components/detalle-traslado-rlc/detalle-traslado-rlc.component';
import { TablaTrasladoRlcComponent } from './components/tabla-traslado-rlc/tabla-traslado-rlc.component';
import { DatosSolicitudRlcComponent } from './components/datos-solicitud-rlc/datos-solicitud-rlc.component';
import { DatosDdpDrpRlcComponent } from './components/datos-ddp-drp-rlc/datos-ddp-drp-rlc.component';
import { TablaDrpRlcComponent } from './components/tabla-drp-rlc/tabla-drp-rlc.component';
import { TablaPorcParticipacionRlcComponent } from './components/tabla-porc-participacion-rlc/tabla-porc-participacion-rlc.component';
import { TablaInforecursosRlcComponent } from './components/tabla-inforecursos-rlc/tabla-inforecursos-rlc.component';
import { TablaInformeFinalRlcComponent } from './components/tabla-informe-final-rlc/tabla-informe-final-rlc.component';
import { DetalleInformeFinalRlcComponent } from './components/detalle-informe-final-rlc/detalle-informe-final-rlc.component';
import { VerdetalleEditGestionSolicitudRlcComponent } from './components/verdetalle-edit-gestion-solicitud-rlc/verdetalle-edit-gestion-solicitud-rlc.component';
import { TablaEnProcesoDeFirmasRlcComponent } from './components/tabla-en-proceso-de-firmas-rlc/tabla-en-proceso-de-firmas-rlc.component';
import { TablaLiquidacionRlcComponent } from './components/tabla-liquidacion-rlc/tabla-liquidacion-rlc.component';
import { VerdetalleRlcComponent } from './components/verdetalle-rlc/verdetalle-rlc.component';

@NgModule({
  declarations: [RegistrarLiquidacionContratoComponent, TablaSinRegistroLiquidacionRlcComponent, GestionarSolicitudRlcComponent, ActualizacionPolizaRlcComponent, TablaBalanceFinancieroRlcComponent, DetalleBalanceFinancieroRlcComponent, RecursosComproPagadosRlcComponent, AcordionRecursosComproPagadosRlcComponent, TablaFuentesUsosRlcComponent, ListaContratistasRlcComponent, TablaValtotalOgRlcComponent, DetalleOgRlcComponent, TablaFacturadoOgRlcComponent, TablaDescuentosOgRlcComponent, TablaOtrosDescuentosOgRlcComponent, EjecucionFinancieraRlcComponent, TablaEjpresupuestalRlcComponent, TablaEjfinancieraRlcComponent, TrasladoRecursosRlcComponent, DetalleTrasladoRlcComponent, TablaTrasladoRlcComponent, DatosSolicitudRlcComponent, DatosDdpDrpRlcComponent, TablaDrpRlcComponent, TablaPorcParticipacionRlcComponent, TablaInforecursosRlcComponent, TablaInformeFinalRlcComponent, DetalleInformeFinalRlcComponent, VerdetalleEditGestionSolicitudRlcComponent, TablaEnProcesoDeFirmasRlcComponent, TablaLiquidacionRlcComponent, VerdetalleRlcComponent],
  imports: [
    CommonModule,
    RegistrarLiquidacionContratoRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FormsModule,
    QuillModule.forRoot(),
    CurrencyMaskModule,
  ]
})
export class RegistrarLiquidacionContratoModule { }
