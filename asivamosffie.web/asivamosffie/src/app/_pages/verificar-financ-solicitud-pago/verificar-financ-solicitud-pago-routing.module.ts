import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormEditVerificarSolicitudVfspComponent } from "./components/form-edit-verificar-solicitud-vfsp/form-edit-verificar-solicitud-vfsp.component";
import { FormVerificarSolicitudVfspComponent } from "./components/form-verificar-solicitud-vfsp/form-verificar-solicitud-vfsp.component";
import { VerdetalleVfspComponent } from "./components/verdetalle-vfsp/verdetalle-vfsp.component";
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
  {
    path: 'verDetalleEditarVerificarFinancSolicitud/:id',
    component: FormEditVerificarSolicitudVfspComponent
  },
  {
    path: 'verDetalleVerificarFinancSolicitud/:id',
    component: VerdetalleVfspComponent
  },
  
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarFinancSolicitudPagoRoutingModule { }
