import { VerDetalleExpensasComponent } from './components/ver-detalle-expensas/ver-detalle-expensas.component';
import { FormObservacionExpensasComponent } from './components/form-observacion-expensas/form-observacion-expensas.component';
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AprobarSolicitudesPagoComponent } from "./components/aprobar-solicitudes-pago/aprobar-solicitudes-pago.component";
import { FormAprobarSolicitudComponent } from "./components/form-aprobar-solicitud/form-aprobar-solicitud.component";
import { FormEditAprobarSolicitudComponent } from "./components/form-edit-aprobar-solicitud/form-edit-aprobar-solicitud.component";

const routes: Routes = [
  {
    path: '',
    component: AprobarSolicitudesPagoComponent
  },
  {
    path: 'aprobacionSolicitud/:idContrato/:idSolicitudPago',
    component: FormAprobarSolicitudComponent
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
    path: 'verDetalleEditarAprobarSolicitud/:id',
    component: FormEditAprobarSolicitudComponent
  },
  {
    path: 'verDetalleAprobarSolicitud/:idContrato/:idSolicitudPago',
    component: FormAprobarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudesPagoRoutingModule { }
