import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormEditVerificarSolicitudVfspComponent } from "./components/form-edit-verificar-solicitud-vfsp/form-edit-verificar-solicitud-vfsp.component";
import { FormVerificarSolicitudVfspComponent } from "./components/form-verificar-solicitud-vfsp/form-verificar-solicitud-vfsp.component";
import { VerDetalleEditarExpensasComponent } from "./components/ver-detalle-editar-expensas/ver-detalle-editar-expensas.component";
import { VerDetalleExpensasComponent } from "./components/ver-detalle-expensas/ver-detalle-expensas.component";
import { VerdetalleVfspComponent } from "./components/verdetalle-vfsp/verdetalle-vfsp.component";
import { VerificarFinancSolicitudPagoComponent } from "./components/verificar-financ-solicitud-pago/verificar-financ-solicitud-pago.component";

const routes: Routes = [
  {
    path: '',
    component: VerificarFinancSolicitudPagoComponent
  },
  {
    path: 'verificarFinancSolicitud/:idContrato/:idSolicitudPago',
    component: FormVerificarSolicitudVfspComponent
  },
  {
    path: 'verDetalleEditarVerificarFinancSolicitud/:id',
    component: FormEditVerificarSolicitudVfspComponent
  },
  {
    path: 'verDetalleVerificarFinancSolicitud/:idContrato/:idSolicitudPago',
    component: VerdetalleVfspComponent
  },
  {
    path: 'verificarExpensas/:id',
    component: VerDetalleEditarExpensasComponent
  },
  {
    path: 'verDetalleExpensas/:id',
    component: VerDetalleExpensasComponent
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VerificarFinancSolicitudPagoRoutingModule { }
