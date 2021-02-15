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

@NgModule({
  declarations: [RegistrarLiquidacionContratoComponent, TablaSinRegistroLiquidacionRlcComponent, GestionarSolicitudRlcComponent, ActualizacionPolizaRlcComponent, TablaBalanceFinancieroRlcComponent, DetalleBalanceFinancieroRlcComponent, RecursosComproPagadosRlcComponent, AcordionRecursosComproPagadosRlcComponent, TablaFuentesUsosRlcComponent, ListaContratistasRlcComponent, TablaValtotalOgRlcComponent, DetalleOgRlcComponent, TablaFacturadoOgRlcComponent, TablaDescuentosOgRlcComponent, TablaOtrosDescuentosOgRlcComponent, EjecucionFinancieraRlcComponent, TablaEjpresupuestalRlcComponent, TablaEjfinancieraRlcComponent],
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
