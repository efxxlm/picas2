import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleBalanceFinancieroRlcComponent } from "./components/detalle-balance-financiero-rlc/detalle-balance-financiero-rlc.component";
import { GestionarSolicitudRlcComponent } from "./components/gestionar-solicitud-rlc/gestionar-solicitud-rlc.component";
import { RegistrarLiquidacionContratoComponent } from "./components/registrar-liquidacion-contrato/registrar-liquidacion-contrato.component";

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
]; 
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class RegistrarLiquidacionContratoRoutingModule { }
