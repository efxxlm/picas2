import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { VerificarFinancSolicitudPagoComponent } from "./components/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.component";

const routes: Routes = [
  {
    path: '',
    component: VerificarFinancSolicitudPagoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarFinancSolicitudPagoRoutingModule { }
