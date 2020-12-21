import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AutorizarSolicitudPagoComponent } from "./components/autorizar-solicitud-pago/autorizar-solicitud-pago.component";

const routes: Routes = [
  {
    path: '',
    component: AutorizarSolicitudPagoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AutorizarSolicitudPagoRoutingModule { }
