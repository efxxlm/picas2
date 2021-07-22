import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AutorizarSolicitudPagoComponent } from "./components/autorizar-solicitud-pago/autorizar-solicitud-pago.component";
import { FormAutorizarSolicitudComponent } from "./components/form-autorizar-solicitud/form-autorizar-solicitud.component";
import { FormEditAutorizarSolicitudComponent } from "./components/form-edit-autorizar-solicitud/form-edit-autorizar-solicitud.component";
import { FormObservacionExpensasComponent } from "./components/form-observacion-expensas/form-observacion-expensas.component";
import { VerDetalleAutorizarSolicitudComponent } from "./components/ver-detalle-autorizar-solicitud/ver-detalle-autorizar-solicitud.component";
import { VerDetalleExpensasComponent } from "./components/ver-detalle-expensas/ver-detalle-expensas.component";

const routes: Routes = [
  {
    path: '',
    component: AutorizarSolicitudPagoComponent
  },
  {
    path:'autorizacionSolicitud/:idContrato/:idSolicitudPago',
    component: FormAutorizarSolicitudComponent
  },
  {
    path: 'observacionExpensas/:id',
    component: FormObservacionExpensasComponent
  },
  {
    path: 'verDetalleExpensas/:id',
    component: VerDetalleExpensasComponent
  },
  {
    path:'verDetalleEditarAutorizarSolicitud/:id',
    component: FormEditAutorizarSolicitudComponent
  },
  {
    path: 'verDetalleAutorizarSolicitud/:idContrato/:idSolicitudPago',
    component: FormAutorizarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AutorizarSolicitudPagoRoutingModule { }
