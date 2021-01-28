import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormEditValidarSolicitudValidfspComponent } from "./components/form-edit-validar-solicitud-validfsp/form-edit-validar-solicitud-validfsp.component";
import { FormValidarSolicitudValidfspComponent } from "./components/form-validar-solicitud-validfsp/form-validar-solicitud-validfsp.component";
import { ValidarFinancSolicitudPagoComponent } from "./components/validar-financ-solicitud-pago/validar-financ-solicitud-pago.component";
import { VerdetalleValidfspComponent } from "./components/verdetalle-validfsp/verdetalle-validfsp.component";

const routes: Routes = [
  {
    path: '',
    component: ValidarFinancSolicitudPagoComponent
  },
  {
    path: 'validarFinancSolicitud/:id',
    component: FormValidarSolicitudValidfspComponent
  },
  {
    path: 'verDetalleEditarValidarFinancSolicitud/:id',
    component: FormEditValidarSolicitudValidfspComponent
  },
  {
    path: 'verDetalleValidarFinancSolicitud/:id',
    component: VerdetalleValidfspComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ValidarFinancSolicitudPagoRoutingModule { }
