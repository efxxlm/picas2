import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AprobarSolicitudesPagoComponent } from "./components/aprobar-solicitudes-pago/aprobar-solicitudes-pago.component";

const routes: Routes = [
  {
    path: '',
    component: AprobarSolicitudesPagoComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AprobarSolicitudesPagoRoutingModule { }
