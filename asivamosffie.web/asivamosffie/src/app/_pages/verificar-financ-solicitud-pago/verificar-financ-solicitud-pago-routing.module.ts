import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormVerificarSolicitudVfspComponent } from "./components/form-verificar-solicitud-vfsp/form-verificar-solicitud-vfsp.component";
import { VerificarFinancSolicitudPagoComponent } from "./components/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.component";

const routes: Routes = [
  {
    path: '',
    component: VerificarFinancSolicitudPagoComponent
  },
  {
    path: 'verificarFinancSolicitud/:id',
    component: FormVerificarSolicitudVfspComponent
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarFinancSolicitudPagoRoutingModule { }
