import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { GestionarTramiteLiqContractualComponent } from "./components/gestionar-tramite-liq-contractual/gestionar-tramite-liq-contractual.component";
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
  }
]; 
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarTramiteLiqContractualRoutingModule { }
