import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DetalleBalanceGbftrecComponent } from "./components/detalle-balance-gbftrec/detalle-balance-gbftrec.component";
import { DetalleOgGbftrecComponent } from "./components/detalle-og-gbftrec/detalle-og-gbftrec.component";
import { GestionarBalanFinancTraslRecComponent } from "./components/gestionar-balan-financ-trasl-rec/gestionar-balan-financ-trasl-rec.component";
import { ValidarBalanceGbftrecComponent } from "./components/validar-balance-gbftrec/validar-balance-gbftrec.component";
import { VerdetalleeditarBalanceGbftrecComponent } from "./components/verdetalleeditar-balance-gbftrec/verdetalleeditar-balance-gbftrec.component";

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
    path: 'detalleOrdengiro',
    component: DetalleOgGbftrecComponent
  },
  {
    path: 'verDetalleEditarBalance/:id',
    component:VerdetalleeditarBalanceGbftrecComponent
  },
  {
    path: 'verDetalleBalance/:id',
    component: DetalleBalanceGbftrecComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarBalanFinancTraslRecursosRoutingModule { }
