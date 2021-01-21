import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { RegistrarPagosRendimientosComponent } from "./components/registrar-pagos-rendimientos/registrar-pagos-rendimientos.component";

const routes: Routes = [
  {
    path: '',
    component: RegistrarPagosRendimientosComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistrarPagosRendimientosRoutingModule { }
