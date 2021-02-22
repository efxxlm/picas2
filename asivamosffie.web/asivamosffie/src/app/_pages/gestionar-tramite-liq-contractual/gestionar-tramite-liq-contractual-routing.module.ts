import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleOgGtlcComponent } from "./components/detalle-og-gtlc/detalle-og-gtlc.component";
import { DetalleTrasladoGtlcComponent } from "./components/detalle-traslado-gtlc/detalle-traslado-gtlc.component";
import { EjecucionFinancieraGtlcComponent } from "./components/ejecucion-financiera-gtlc/ejecucion-financiera-gtlc.component";
import { GestionarTramiteLiqContractualComponent } from "./components/gestionar-tramite-liq-contractual/gestionar-tramite-liq-contractual.component";
import { RecursosComproPagadosGtlcComponent } from "./components/recursos-compro-pagados-gtlc/recursos-compro-pagados-gtlc.component";
import { TrasladoRecursosGtlcComponent } from "./components/traslado-recursos-gtlc/traslado-recursos-gtlc.component";
import { VerDetalleVerificacionGtlcComponent } from "./components/ver-detalle-verificacion-gtlc/ver-detalle-verificacion-gtlc.component";
import { VerificarBalanceGtlcComponent } from "./components/verificar-balance-gtlc/verificar-balance-gtlc.component";
import { VerificarInformeGtlcComponent } from "./components/verificar-informe-gtlc/verificar-informe-gtlc.component";
import { VerificarRequisitosGtlcComponent } from "./components/verificar-requisitos-gtlc/verificar-requisitos-gtlc.component";

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
    component: GestionarTramiteLiqContractualComponent
  },
  {
    path : 'verificarRequisitos/:id',
    component: VerificarRequisitosGtlcComponent
  },
  {
    path : 'verificarBalance/:id',
    component: VerificarBalanceGtlcComponent
  },
  {
    path: 'recursosComprometidos/:id',
    component: RecursosComproPagadosGtlcComponent
  },
  {
    path: 'detalleOrdengiro/:id',
    component: DetalleOgGtlcComponent
  },
  {
    path: 'ejecucionFinanciera/:id',
    component: EjecucionFinancieraGtlcComponent
  },
  {
    path: 'trasladoRecursos/:id',
    component: TrasladoRecursosGtlcComponent
  },
  {
    path: 'detalleTraslado/:id',
    component: DetalleTrasladoGtlcComponent
  },
  {
    path:'verificarInformeFinal/:id',
    component: VerificarInformeGtlcComponent
  },
  {
    path: 'verDetalleVerificacion/:id',
    component: VerDetalleVerificacionGtlcComponent
  }
]; 
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarTramiteLiqContractualRoutingModule { }
