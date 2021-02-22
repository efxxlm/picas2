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



@NgModule({
  declarations: [GestionarTramiteLiqContractualComponent, TablaLiquidacionObraGtlcComponent, TablaLiquidacionIntervnGtlcComponent, VerificarRequisitosGtlcComponent, ActualizacionPolizaGtlcComponent, TablaBalanceFinancieroGtlcComponent, VerificarBalanceGtlcComponent, RecursosComproPagadosGtlcComponent, AcordionRecursosComproPagadosGtlcComponent, ListaContratistasGtlcComponent, TablaValtotalOgGtlcComponent, TablaFuentesUsosGtlcComponent, DetalleOgGtlcComponent, TablaFacturadoOgGtlcComponent, TablaDescuentosOgGtlcComponent, TablaOtrosDescuentosOgGtlcComponent, EjecucionFinancieraGtlcComponent, TablaEjpresupuestalGtlcComponent, TablaEjfinancieraGtlcComponent],
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
