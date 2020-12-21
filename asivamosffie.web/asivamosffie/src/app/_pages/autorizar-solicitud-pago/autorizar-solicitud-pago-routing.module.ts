import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AutorizarSolicitudPagoComponent } from "./components/autorizar-solicitud-pago/autorizar-solicitud-pago.component";
import { FormAutorizarSolicitudComponent } from "./components/form-autorizar-solicitud/form-autorizar-solicitud.component";

const routes: Routes = [
  {
    path: '',
    component: AutorizarSolicitudPagoComponent
  },
  {
    path:'autorizacionSolicitud/:id',
    component: FormAutorizarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AutorizarSolicitudPagoRoutingModule { }
