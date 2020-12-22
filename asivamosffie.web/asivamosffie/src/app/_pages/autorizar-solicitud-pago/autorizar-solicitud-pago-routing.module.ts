import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AutorizarSolicitudPagoComponent } from "./components/autorizar-solicitud-pago/autorizar-solicitud-pago.component";
import { FormAutorizarSolicitudComponent } from "./components/form-autorizar-solicitud/form-autorizar-solicitud.component";
import { FormEditAutorizarSolicitudComponent } from "./components/form-edit-autorizar-solicitud/form-edit-autorizar-solicitud.component";
import { VerDetalleAutorizarSolicitudComponent } from "./components/ver-detalle-autorizar-solicitud/ver-detalle-autorizar-solicitud.component";

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
  {
    path: 'verDetalleAutorizarSolicitud/:id',
    component: VerDetalleAutorizarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AutorizarSolicitudPagoRoutingModule { }
