import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AprobarSolicitudesPagoComponent } from "./components/aprobar-solicitudes-pago/aprobar-solicitudes-pago.component";
import { FormAprobarSolicitudComponent } from "./components/form-aprobar-solicitud/form-aprobar-solicitud.component";
import { FormEditAprobarSolicitudComponent } from "./components/form-edit-aprobar-solicitud/form-edit-aprobar-solicitud.component";
import { VerDetalleAprobarSolicitudComponent } from "./components/ver-detalle-aprobar-solicitud/ver-detalle-aprobar-solicitud.component";

const routes: Routes = [
  {
    path: '',
    component: AprobarSolicitudesPagoComponent
  },
  {
    path: 'aprobacionSolicitud/:id',
    component: FormAprobarSolicitudComponent
  },
  {
    path: 'verDetalleEditarAprobarSolicitud/:id',
    component: FormEditAprobarSolicitudComponent
  },
  {
    path: 'verDetalleAprobarSolicitud/:id',
    component: VerDetalleAprobarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudesPagoRoutingModule { }
