import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleBalanceFinancieroRlcComponent } from "./components/detalle-balance-financiero-rlc/detalle-balance-financiero-rlc.component";
import { DetalleInformeFinalRlcComponent } from "./components/detalle-informe-final-rlc/detalle-informe-final-rlc.component";
import { DetalleOgRlcComponent } from "./components/detalle-og-rlc/detalle-og-rlc.component";
import { DetalleTrasladoRlcComponent } from "./components/detalle-traslado-rlc/detalle-traslado-rlc.component";
import { EjecucionFinancieraRlcComponent } from "./components/ejecucion-financiera-rlc/ejecucion-financiera-rlc.component";
import { GestionarSolicitudRlcComponent } from "./components/gestionar-solicitud-rlc/gestionar-solicitud-rlc.component";
import { RecursosComproPagadosRlcComponent } from "./components/recursos-compro-pagados-rlc/recursos-compro-pagados-rlc.component";
import { RegistrarLiquidacionContratoComponent } from "./components/registrar-liquidacion-contrato/registrar-liquidacion-contrato.component";
import { TrasladoRecursosRlcComponent } from "./components/traslado-recursos-rlc/traslado-recursos-rlc.component";

export const customCurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  allowZero: true,
  decimal: ",",
  precision: 0,
  prefix: "$ ",
  suffix: "",
  thousands: ".",
  nullable: true,
  min: null,
  max: null,
  inputMode: CurrencyMaskInputMode.FINANCIAL
};
const routes: Routes = [
  {
    path: '',
    component: RegistrarLiquidacionContratoComponent
  },
  {
    path: 'gestionarSolicitud/:id',
    component: GestionarSolicitudRlcComponent
  },
  {
    path: 'detalleBalanceFinanciero/:id',
    component: DetalleBalanceFinancieroRlcComponent
  },
  {
    path: 'recursosComprometidos/:id',
    component: RecursosComproPagadosRlcComponent
  },
  {
    path: 'detalleOrdengiro/:id',
    component: DetalleOgRlcComponent
  },
  {
    path: 'ejecucionFinanciera/:id',
    component: EjecucionFinancieraRlcComponent
  },
  {
    path: 'trasladoRecursos/:id',
    component: TrasladoRecursosRlcComponent
  },
  {
    path: 'detalleTraslado/:id',
    component: DetalleTrasladoRlcComponent
  },
  {
    path: 'detalleInformeFinal/:id',
    component: DetalleInformeFinalRlcComponent
  }
]; 
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class RegistrarLiquidacionContratoRoutingModule { }
