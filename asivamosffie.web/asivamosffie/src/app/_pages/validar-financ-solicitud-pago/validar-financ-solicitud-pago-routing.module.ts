import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormValidarSolicitudValidfspComponent } from "./components/form-validar-solicitud-validfsp/form-validar-solicitud-validfsp.component";
import { ValidarFinancSolicitudPagoComponent } from "./components/validar-financ-solicitud-pago/validar-financ-solicitud-pago.component";

const routes: Routes = [
  {
    path: '',
    component: ValidarFinancSolicitudPagoComponent
  },
  {
    path: 'validarFinancSolicitud/:id',
    component: FormValidarSolicitudValidfspComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarFinancSolicitudPagoRoutingModule { }
