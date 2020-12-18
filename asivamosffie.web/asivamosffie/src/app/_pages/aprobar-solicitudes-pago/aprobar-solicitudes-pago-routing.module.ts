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
    path: 'aprobacionSolicitud/:id',
    component: FormAprobarSolicitudComponent
  },
  {
    path: 'verDetalleEditar/:id',
    component: FormEditAprobarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudesPagoRoutingModule { }
