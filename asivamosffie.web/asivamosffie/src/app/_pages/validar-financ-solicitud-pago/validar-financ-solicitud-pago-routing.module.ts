import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormEditValidarSolicitudValidfspComponent } from "./components/form-edit-validar-solicitud-validfsp/form-edit-validar-solicitud-validfsp.component";
import { FormValidarSolicitudValidfspComponent } from "./components/form-validar-solicitud-validfsp/form-validar-solicitud-validfsp.component";
import { ValidarFinancSolicitudPagoComponent } from "./components/validar-financ-solicitud-pago/validar-financ-solicitud-pago.component";
import { VerDetalleEditarExpensasComponent } from "./components/ver-detalle-editar-expensas/ver-detalle-editar-expensas.component";
import { VerDetalleExpensasComponent } from "./components/ver-detalle-expensas/ver-detalle-expensas.component";
import { VerdetalleValidfspComponent } from "./components/verdetalle-validfsp/verdetalle-validfsp.component";

const routes: Routes = [
  {
    path: '',
    component: ValidarFinancSolicitudPagoComponent
  },
  {
    path: 'validarFinancSolicitud/:idContrato/:idSolicitudPago',
    component: FormValidarSolicitudValidfspComponent
  },
  {
    path: 'verDetalleEditarValidarFinancSolicitud/:id',
    component: FormEditValidarSolicitudValidfspComponent
  },
  {
    path: 'verDetalleValidarFinancSolicitud/:idContrato/:idSolicitudPago',
    component: VerdetalleValidfspComponent
  },
  {
    path: 'validarExpensas/:id',
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
export class ValidarFinancSolicitudPagoRoutingModule { }
