import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DetalleOgGbftrecComponent } from "./components/detalle-og-gbftrec/detalle-og-gbftrec.component";
import { GestionarBalanFinancTraslRecComponent } from "./components/gestionar-balan-financ-trasl-rec/gestionar-balan-financ-trasl-rec.component";
import { ValidarBalanceGbftrecComponent } from "./components/validar-balance-gbftrec/validar-balance-gbftrec.component";

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
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GestionarBalanFinancTraslRecursosRoutingModule { }
