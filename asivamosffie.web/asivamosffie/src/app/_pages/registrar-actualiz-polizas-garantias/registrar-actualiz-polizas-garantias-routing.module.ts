import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { ActualizarPolizaRapgComponent } from "./components/actualizar-poliza-rapg/actualizar-poliza-rapg.component";
import { RegistrarActualizPolizasGarantiasComponent } from "./components/registrar-actualiz-polizas-garantias/registrar-actualiz-polizas-garantias.component";

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
    component: RegistrarActualizPolizasGarantiasComponent
  },
  {
    path: 'actualizarPoliza/:id',
    component: ActualizarPolizaRapgComponent
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class RegistrarActualizPolizasGarantiasRoutingModule { }
