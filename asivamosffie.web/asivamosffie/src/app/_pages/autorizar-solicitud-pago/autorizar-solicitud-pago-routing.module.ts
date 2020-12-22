import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AutorizarSolicitudPagoComponent } from "./components/autorizar-solicitud-pago/autorizar-solicitud-pago.component";
import { FormAutorizarSolicitudComponent } from "./components/form-autorizar-solicitud/form-autorizar-solicitud.component";
import { FormEditAutorizarSolicitudComponent } from "./components/form-edit-autorizar-solicitud/form-edit-autorizar-solicitud.component";

const routes: Routes = [
  {
    path: '',
    component: AutorizarSolicitudPagoComponent
  },
  {
    path:'autorizacionSolicitud/:id',
    component: FormAutorizarSolicitudComponent
  },
  {
    path:'verDetalleEditarAutorizarSolicitud/:id',
    component: FormEditAutorizarSolicitudComponent
  },
  /*
  {
    path: 'verDetalleAprobarSolicitud/:id',
    component: VerDetalleAprobarSolicitudComponent
  }
  */
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AutorizarSolicitudPagoRoutingModule { }
