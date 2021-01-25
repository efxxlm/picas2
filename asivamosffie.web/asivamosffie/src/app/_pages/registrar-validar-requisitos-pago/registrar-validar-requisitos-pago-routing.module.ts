import { VerDetalleEditarExpensasComponent } from './components/ver-detalle-editar-expensas/ver-detalle-editar-expensas.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrencyMaskInputMode, NgxCurrencyModule } from 'ngx-currency';
import { RegistrarNuevaSolicitudPagoComponent } from './components/registrar-nueva-solicitud-pago/registrar-nueva-solicitud-pago.component';
import { RegistrarValidarRequisitosPagoComponent } from './components/registrar-validar-requisitos-pago/registrar-validar-requisitos-pago.component';
import { VerdetalleEditarSolicitudPagoComponent } from './components/verdetalle-editar-solicitud-pago/verdetalle-editar-solicitud-pago.component';
import { VerdetalleSolicitudPagoComponent } from './components/verdetalle-solicitud-pago/verdetalle-solicitud-pago.component';
export const customCurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  allowZero: true,
  decimal: ",",
  precision: 0,
  prefix: "$ ",
  suffix: "",
  thousands: ".",
  nullable: true,
  min: null,
  max: null,
  inputMode: CurrencyMaskInputMode.FINANCIAL
};
const routes: Routes = [
  {
    path: '',
    component: RegistrarValidarRequisitosPagoComponent
  },
  {
    path: 'registrarNuevaSolicitudPago',
    component: RegistrarNuevaSolicitudPagoComponent
  },
  {
    path: 'verDetalleEditar/:idContrato/:idSolicitud',
    component: VerdetalleEditarSolicitudPagoComponent
  },
  {
    path: 'verDetalleEditarExpensas/:id',
    component: VerDetalleEditarExpensasComponent
  },
  {
    path: 'verDetalle/:id',
    component: VerdetalleSolicitudPagoComponent
  },
];
@NgModule({
  imports: [RouterModule.forChild(routes),NgxCurrencyModule.forRoot(customCurrencyMaskConfig)],
  exports: [RouterModule]
})
export class RegistrarValidarRequisitosPagoRoutingModule { }
