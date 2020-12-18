import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AprobarSolicitudesPagoComponent } from "./components/aprobar-solicitudes-pago/aprobar-solicitudes-pago.component";
import { FormAprobarSolicitudComponent } from "./components/form-aprobar-solicitud/form-aprobar-solicitud.component";

const routes: Routes = [
  {
    path: '',
    component: AprobarSolicitudesPagoComponent
  },
  {
    path: 'aprobacionSolicitud/:id',
    component: FormAprobarSolicitudComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudesPagoRoutingModule { }
