import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ValidarFinancSolicitudPagoComponent } from "./components/validar-financ-solicitud-pago/validar-financ-solicitud-pago.component";

const routes: Routes = [
  {
    path: '',
    component: ValidarFinancSolicitudPagoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarFinancSolicitudPagoRoutingModule { }
