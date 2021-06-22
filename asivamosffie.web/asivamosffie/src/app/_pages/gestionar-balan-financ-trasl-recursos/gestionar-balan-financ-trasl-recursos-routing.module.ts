import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleBalanceGbftrecComponent } from "./components/detalle-balance-gbftrec/detalle-balance-gbftrec.component";
import { DetalleOgGbftrecComponent } from "./components/detalle-og-gbftrec/detalle-og-gbftrec.component";
import { GestionarBalanFinancTraslRecComponent } from "./components/gestionar-balan-financ-trasl-rec/gestionar-balan-financ-trasl-rec.component";
import { RegistrarTrasladoGbftrecComponent } from "./components/registrar-traslado-gbftrec/registrar-traslado-gbftrec.component";
import { ValidarBalanceGbftrecComponent } from "./components/validar-balance-gbftrec/validar-balance-gbftrec.component";
import { VerdetalleTrasladoGbftrecComponent } from "./components/verdetalle-traslado-gbftrec/verdetalle-traslado-gbftrec.component";
import { VerdetalleeditarBalanceGbftrecComponent } from "./components/verdetalleeditar-balance-gbftrec/verdetalleeditar-balance-gbftrec.component";
import { VerdetalleeditarTrasladoGbftrecComponent } from "./components/verdetalleeditar-traslado-gbftrec/verdetalleeditar-traslado-gbftrec.component";
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
    component: GestionarBalanFinancTraslRecComponent
  },
  {
    path: 'validarBalance/:id',
    component: ValidarBalanceGbftrecComponent
  },
  {
    path: 'verDetalleEditarBalance/:id',
    component: ValidarBalanceGbftrecComponent
  },
  {
    path: 'verDetalleBalance/:id',
    component: ValidarBalanceGbftrecComponent
  },
  {
    path: 'validarBalance/:id/detalleOrdengiro/:id',
    component: DetalleOgGbftrecComponent
  },
  {
    path: 'verDetalleBalance/:id/detalleOrdengiro/:id',
    component: DetalleOgGbftrecComponent
  },
  {
    path: 'verDetalleEditarBalance/:id/detalleOrdengiro/:id',
    component: DetalleOgGbftrecComponent
  },
  /*
  ,
  {
    path: 'verDetalleEditarBalance/:id',
    component:VerdetalleeditarBalanceGbftrecComponent
  }
  */
  {
    path: 'verDetalleBalance/:id',
    component: DetalleBalanceGbftrecComponent
  },
  {
    path: 'registrarTraslado/:id',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verDetalleEditarTraslado/:id/:numeroOrdenGiro/:solicitudPagoId',
    component: RegistrarTrasladoGbftrecComponent
  },
  {
    path: 'verDetalleTraslado/:id/:numeroOrdenGiro/:solicitudPagoId',
    component: RegistrarTrasladoGbftrecComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GestionarBalanFinancTraslRecursosRoutingModule { }
