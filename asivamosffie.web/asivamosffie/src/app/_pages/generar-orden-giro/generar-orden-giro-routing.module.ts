import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CurrencyMaskInputMode, NgxCurrencyModule } from "ngx-currency";
import { DetalleOrdenGiroGogComponent } from "./components/detalle-orden-giro-gog/detalle-orden-giro-gog.component";
import { FormGenerarOrdenGiroComponent } from "./components/form-generar-orden-giro/form-generar-orden-giro.component";
import { GenerarOrdenGiroComponent } from "./components/generar-orden-giro/generar-orden-giro.component";
import { VerDetalleEditOrdenGiroGogComponent } from "./components/ver-detalle-edit-orden-giro-gog/ver-detalle-edit-orden-giro-gog.component";
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
    component: GenerarOrdenGiroComponent
  },
  {
    path: 'generacionOrdenGiro/:id',
    component: FormGenerarOrdenGiroComponent
  },
  {
    path: 'verDetalleEditarOrdenGiro/:id',
    component: VerDetalleEditOrdenGiroGogComponent
  },
  {
    path: 'verDetalleOrdenGiro/:id',
    component: DetalleOrdenGiroGogComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class GenerarOrdenGiroRoutingModule { }
